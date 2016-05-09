using System;
using System.Drawing;
using System.Windows.Forms;

namespace EveOPreview
{
    public partial class Preview : Form
    {
        public bool show_overlay = true;
        public bool hover_zoom = true;
        public bool is_zoomed = false;
        public bool is_hovered_over = false;

        private bool mouse_over_lock = false;
        private Size old_size;
        private Point old_position;

        private IntPtr m_hThumbnail;
        public IntPtr sourceWindow;
        private DWM_THUMBNAIL_PROPERTIES m_ThumbnailProperties;
        private bool has_been_set_up = false;
        private bool thumbnail_has_been_set_up = false;
        private MainForm spawner;
        private Hotkey hotkey;

        private bool hide = false;

        public PreviewOverlay overlay;

        public void MakeHidden(bool wha)
        {
            hide = wha;
        }

        public override string ToString()
        {
            return this.Text;
        }



        public void MakeTopMost(bool topmost)
        {
            this.TopMost = topmost && !(this.hide);
        }

        public Preview(IntPtr sourceWindow, String title, MainForm spawner, Size size) 
        {
            has_been_set_up = false;

            this.sourceWindow = sourceWindow;
            this.spawner = spawner; 

            InitializeComponent(); 

            this.Text = title;

            this.overlay = new PreviewOverlay(this);

            this.render_area.MouseHover += new System.EventHandler(this.preview_MouseHover);
            this.render_area.MouseLeave += new System.EventHandler(this.preview_MouseLeave);

            this.old_size = this.Size;
            this.old_position = this.Location;

            has_been_set_up = true;


        }

        public void preview_MouseHover(object sender, System.EventArgs e)
        {
            if (!mouse_over_lock)
            {
                mouse_over_lock = true;
                if (hover_zoom)
                    doZoom();

                TopMost = true;
                overlay.TopMost = true;
            }
            this.Opacity = 1.0f;
            this.is_hovered_over = true;
            RefreshPreview();
        }

        protected override CreateParams CreateParams
        {
            get
            {
                var Params = base.CreateParams;
                Params.ExStyle |= 0x80;
                return Params;
            }
        }

        public void registerShortcut(string shortcut)
        {
            if (shortcut == "")
                return;
            var cvt = new KeysConverter();
            var key = (Keys)cvt.ConvertFrom(shortcut);

            Hotkey hotkey = new Hotkey();

            if ((key & Keys.Shift) == Keys.Shift)
            {
                hotkey.Shift = true;
            }
            if ((key & Keys.Alt) == Keys.Alt) 
            {
                hotkey.Alt = true;
            }
            if ((key & Keys.Control) == Keys.Control) 
            {
                hotkey.Control = true;
            }

            key = key & ~Keys.Shift & ~Keys.Alt & ~Keys.Control;
            hotkey.KeyCode = key;
            hotkey.Register(this);
            hotkey.Pressed += delegate { bring_client_to_foreground(); spawner.preview_did_switch(); };

            this.hotkey = hotkey;
        }

        public void doZoom()
        {
            if (is_zoomed)
                return;
               
            is_zoomed = true;

            float hover_zoom_factor = Properties.Settings.Default.zoom_amount;

            old_size = Size;
            old_position = Location;

            Size = new Size((int)(hover_zoom_factor * (float)Size.Width), (int)(hover_zoom_factor * (float)Size.Height));

            switch ((MainForm.zoom_anchor_t)Properties.Settings.Default.zoom_anchor)
            {
                case (MainForm.zoom_anchor_t.NW):
                    break;
                case (MainForm.zoom_anchor_t.N):
                    Location = new Point(Location.X - Size.Width / 2 + old_size.Width / 2, Location.Y);
                    break;
                case (MainForm.zoom_anchor_t.NE):
                    Location = new Point(Location.X - Size.Width + old_size.Width, Location.Y);
                    break;

                case (MainForm.zoom_anchor_t.W):
                    Location = new Point(Location.X, Location.Y - Size.Height / 2 + old_size.Height / 2);
                    break;
                case (MainForm.zoom_anchor_t.C):
                    Location = new Point(Location.X - Size.Width / 2 + old_size.Width / 2, Location.Y - Size.Height / 2 + old_size.Height / 2);
                    break;
                case (MainForm.zoom_anchor_t.E):
                    Location = new Point(Location.X - Size.Width + old_size.Width, Location.Y - Size.Height / 2 + old_size.Height / 2);
                    break;

                case (MainForm.zoom_anchor_t.SW):
                    Location = new Point(Location.X, Location.Y - Size.Height + old_size.Height);
                    break;
                case (MainForm.zoom_anchor_t.S):
                    Location = new Point(Location.X - Size.Width / 2 + old_size.Width / 2, Location.Y - Size.Height + old_size.Height);
                    break;
                case (MainForm.zoom_anchor_t.SE):
                    Location = new Point(Location.X - Size.Width + old_size.Width, Location.Y - Size.Height + old_size.Height);
                    break;
            }
        }

        public void restoreZoom()
        {
            if (!is_zoomed)
                return;

            Size = old_size;
            Location = old_position;
            is_zoomed = false;
        }

        public void preview_MouseLeave(object sender, System.EventArgs e)
        {
            if (mouse_over_lock)
            {
                if (hover_zoom)
                {
                    restoreZoom();
                }
                mouse_over_lock = false;
            }
            this.is_hovered_over = false;
            this.Opacity = Properties.Settings.Default.opacity;
            RefreshPreview();
        }

        protected override void OnResize(EventArgs e)
        {
            RefreshPreview();
            base.OnResize(e);
            if (has_been_set_up && !mouse_over_lock)
                this.spawner.syncronize_preview_size(this.Size);
        }

        protected override void OnMove(EventArgs e)
        {
            base.OnMove(e);
            if (has_been_set_up && !mouse_over_lock)
                this.spawner.register_preview_position(this.Text, this.Location);

            RefreshPreview();
        }

        public void doMove(Point position)
        {
            if (has_been_set_up && !mouse_over_lock)
                Location = position;

            RefreshPreview();
        }

        public void SetLabel(String label)
        {
            this.Text = label;
            this.overlay.client_label.Text = label;
        }

        public void RefreshPreview()
        {
            if (has_been_set_up)
            {
                if (DwmApiNativeMethods.DwmIsCompositionEnabled())
                {
                    if (thumbnail_has_been_set_up == false)
                    {
                        this.SetUpThumbnail();
                    }
                    m_ThumbnailProperties.rcDestination = new RECT(0, 0, ClientRectangle.Right, ClientRectangle.Bottom);
                    DwmApiNativeMethods.DwmUpdateThumbnailProperties(m_hThumbnail, m_ThumbnailProperties);
                }
                else
                {
                    thumbnail_has_been_set_up = false;
                }

                Size overlay_size = this.render_area.Size;
                overlay_size.Width -= 2 * 5;
                overlay_size.Height -= 2 * 5;

                Point overlay_location = this.Location;
                overlay_location.X += 5 + (this.Size.Width - this.render_area.Size.Width) / 2;
                overlay_location.Y += 5 + (this.Size.Height - this.render_area.Size.Height) - (this.Size.Width - this.render_area.Size.Width) / 2;

                this.overlay.Size = overlay_size;
                this.overlay.Location = overlay_location;

            }
        }

        new public void Show()
        {
            if (!hide)
            {
                base.Show();
                if (show_overlay)
                    this.overlay.Show();
                else
                    this.overlay.Hide();
            }
            else
            {
                this.Hide();
                this.overlay.Hide();
            }
        }

        new public void Hide()
        {
            base.Hide();
            this.overlay.Hide();
        }

        private void SetUpThumbnail()
        {
            if (DwmApiNativeMethods.DwmIsCompositionEnabled() && !thumbnail_has_been_set_up)
            {
                m_hThumbnail = DwmApiNativeMethods.DwmRegisterThumbnail(this.Handle, sourceWindow);

                m_ThumbnailProperties = new DWM_THUMBNAIL_PROPERTIES();
                m_ThumbnailProperties.dwFlags = DWM_THUMBNAIL_PROPERTIES.DWM_TNP_VISIBLE
                    + DWM_THUMBNAIL_PROPERTIES.DWM_TNP_OPACITY
                    + DWM_THUMBNAIL_PROPERTIES.DWM_TNP_RECTDESTINATION
                    + DWM_THUMBNAIL_PROPERTIES.DWM_TNP_SOURCECLIENTAREAONLY;
                m_ThumbnailProperties.opacity = 255;
                m_ThumbnailProperties.fVisible = true;
                m_ThumbnailProperties.fSourceClientAreaOnly = true;
                m_ThumbnailProperties.rcDestination = new RECT(0, 0, ClientRectangle.Right, ClientRectangle.Bottom);

                DwmApiNativeMethods.DwmUpdateThumbnailProperties(m_hThumbnail, m_ThumbnailProperties);

                thumbnail_has_been_set_up = true;
            }
        }

        private void Preview_Load(object sender, EventArgs e)
        {

        }

        public void bring_client_to_foreground()
        {
            DwmApiNativeMethods.SetForegroundWindow(sourceWindow);
            int style = DwmApiNativeMethods.GetWindowLong(sourceWindow, DwmApiNativeMethods.GWL_STYLE);
            if ((style & DwmApiNativeMethods.WS_MAXIMIZE) == DwmApiNativeMethods.WS_MAXIMIZE)
            {
                //It's maximized
            }
            else if ((style & DwmApiNativeMethods.WS_MINIMIZE) == DwmApiNativeMethods.WS_MINIMIZE)
            {
                DwmApiNativeMethods.ShowWindowAsync(sourceWindow, DwmApiNativeMethods.SW_SHOWNORMAL);
            }
        }

        public void render_area_Click(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                bring_client_to_foreground();
                spawner.preview_did_switch();
            }
            if (e.Button == MouseButtons.Right)
            {
                // do smth cool?
            }
            if (e.Button == MouseButtons.Middle)
            {
                // do smth cool?
            }
        }

        public void set_render_area_size(Size size)
        {
            this.Size = size;
        }

    }
}