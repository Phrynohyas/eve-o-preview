using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace PreviewToy
{
    public partial class Preview : Form
    {
        public bool show_overlay = true;
        public bool hover_zoom = true;
        public float hover_zoom_factor = 3.0f;

        private bool mouse_over_lock = false;
        private Size old_size;

        private IntPtr m_hThumbnail;
        public IntPtr sourceWindow;
        private DwmApi.DWM_THUMBNAIL_PROPERTIES m_ThumbnailProperties;
        private bool has_been_set_up = false;
        private PreviewToyHandler spawner;

        public PreviewOverlay overlay;

        public Preview(IntPtr sourceWindow, String title, PreviewToyHandler spawner, Size size) 
        {
            has_been_set_up = false;

            this.sourceWindow = sourceWindow;
            this.spawner = spawner; 

            InitializeComponent(); 
            SetUp();

            this.Text = title;

            this.overlay = new PreviewOverlay(this);

            this.MouseHover += new System.EventHandler(this.preview_MouseHover);
            this.render_area.MouseHover += new System.EventHandler(this.preview_MouseHover);
            this.overlay.MouseHover += new System.EventHandler(this.preview_MouseHover);
            this.overlay.overlay_area.MouseHover += new System.EventHandler(this.preview_MouseHover);

            this.MouseLeave += new System.EventHandler(this.preview_MouseLeave);
            this.render_area.MouseLeave += new System.EventHandler(this.preview_MouseLeave);
            this.overlay.MouseLeave += new System.EventHandler(this.preview_MouseLeave);
            this.overlay.overlay_area.MouseLeave += new System.EventHandler(this.preview_MouseLeave);

            old_size = this.Size;

            has_been_set_up = true;

        }

        public void preview_MouseHover(object sender, System.EventArgs e)
        {
            if (!mouse_over_lock)
            {
                if (hover_zoom)
                {
                    old_size = Size;
                    Size = new Size((int)(hover_zoom_factor * (float)Size.Width),
                                    (int)(hover_zoom_factor * (float)Size.Height));
                }
                TopMost = true;
                overlay.TopMost = true;
                mouse_over_lock = true;
            }
            RefreshPreview();
        }

        public void preview_MouseLeave(object sender, System.EventArgs e)
        {
            if (mouse_over_lock)
            {
                if (hover_zoom)
                {
                    Size = old_size;
                }
                mouse_over_lock = false;
            }
            RefreshPreview();
        }

        protected override void OnResize(EventArgs e)
        {
            RefreshPreview();
            base.OnResize(e);
            if (has_been_set_up)
            {
                this.spawner.syncronize_preview_size(this.Size);
            }
        }

        protected override void OnMove(EventArgs e)
        {
            base.OnMove(e);
            this.spawner.register_preview_position(this.Text, this.Location);
            RefreshPreview();
        }

        public void SetLabel(String label)
        {
            this.Text = "-> " + label + " <-";
            this.overlay.client_label.Text = label;
        }

        public void RefreshPreview()
        {
            if (has_been_set_up)
            {
                m_ThumbnailProperties.rcDestination = new DwmApi.RECT(0, 0, ClientRectangle.Right, ClientRectangle.Bottom);
                DwmApi.DwmUpdateThumbnailProperties(m_hThumbnail, m_ThumbnailProperties);

                Size overlay_size = this.render_area.Size;
                overlay_size.Width -= 2*5;
                overlay_size.Height -= 2*5;

                Point overlay_location = this.Location;
                overlay_location.X += 5 + (this.Size.Width - this.render_area.Size.Width)/2;
                overlay_location.Y += 5 + (this.Size.Height - this.render_area.Size.Height) - (this.Size.Width - this.render_area.Size.Width)/2;

                this.overlay.Size = overlay_size;
                this.overlay.Location = overlay_location;
                this.overlay.TopMost = this.TopMost;
            }
        }

        public void Show()
        {
            base.Show();
            if (show_overlay)
            {
                this.overlay.Show();
            }
            else
            {
                this.overlay.Hide();
            }
        }

        public void Hide()
        {
            base.Hide();
            this.overlay.Hide();
        }

        public void SetUp()
        {
            m_hThumbnail = DwmApi.DwmRegisterThumbnail(this.Handle, sourceWindow);

            m_ThumbnailProperties = new DwmApi.DWM_THUMBNAIL_PROPERTIES();
            m_ThumbnailProperties.dwFlags = DwmApi.DWM_THUMBNAIL_PROPERTIES.DWM_TNP_VISIBLE
                + DwmApi.DWM_THUMBNAIL_PROPERTIES.DWM_TNP_OPACITY
                + DwmApi.DWM_THUMBNAIL_PROPERTIES.DWM_TNP_RECTDESTINATION
                + DwmApi.DWM_THUMBNAIL_PROPERTIES.DWM_TNP_SOURCECLIENTAREAONLY;
            m_ThumbnailProperties.opacity = 255;
            m_ThumbnailProperties.fVisible = true;
            m_ThumbnailProperties.fSourceClientAreaOnly = true;
            m_ThumbnailProperties.rcDestination = new DwmApi.RECT(0, 0, ClientRectangle.Right, ClientRectangle.Bottom);
            
            DwmApi.DwmUpdateThumbnailProperties(m_hThumbnail, m_ThumbnailProperties);
            
        }

        private void Preview_Load(object sender, EventArgs e)
        {

        }

        public void bring_client_to_foreground()
        {
            DwmApi.SetForegroundWindow(sourceWindow);
            int style = DwmApi.GetWindowLong(sourceWindow, DwmApi.GWL_STYLE);
            if ((style & DwmApi.WS_MAXIMIZE) == DwmApi.WS_MAXIMIZE)
            {
                //It's maximized
            }
            else if ((style & DwmApi.WS_MINIMIZE) == DwmApi.WS_MINIMIZE)
            {
                DwmApi.ShowWindowAsync(sourceWindow, DwmApi.SW_SHOWNORMAL);
            }
        }

        public void render_area_Click(object sender, EventArgs e)
        {
            bring_client_to_foreground();
            spawner.preview_did_switch();
        }

        public void set_render_area_size(Size size)
        {
            this.Size = size;
        }

    }
}