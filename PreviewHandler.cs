using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Drawing.Text;
using System.Windows.Threading;
using System.Xml.Linq;
using System.Linq;

using System.IO;

namespace PreviewToy
{

    public partial class PreviewToyHandler : Form
    {
        private Dictionary<IntPtr, Preview> previews;
        private DispatcherTimer dispatcherTimer;

        private IntPtr active_client_handle = (IntPtr)0;
        private String active_client_title = "";

        private Dictionary<String, Dictionary<String, Point>> unique_layouts;
        private Dictionary<String, Point> flat_layout;
        private Dictionary<String, ClientLocation> client_layout;

        private bool is_initialized;

        private Stopwatch ignoring_size_sync;

        Dictionary<string, string> xml_bad_to_ok_chars;

        [DllImport("user32.dll")]
        private static extern int GetWindowRect(IntPtr hwnd, out Rect rect);

        [DllImport("user32.dll")]
        public static extern bool MoveWindow(IntPtr hWnd, int X, int Y, int nWidth, int nHeight, bool bRepaint);

        private struct Rect
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }

        private struct ClientLocation
        {
            public int X;
            public int Y;
            public int Width;
            public int Height;
        }

        public enum zoom_anchor_t
        {
            NW = 0,
            N,
            NE,
            W,
            C,
            E,
            SW,
            S,
            SE
        };

        private Dictionary<zoom_anchor_t, RadioButton> zoom_anchor_button_map;
        
        public PreviewToyHandler()
        {
            is_initialized = false;

            previews = new Dictionary<IntPtr, Preview>();

            xml_bad_to_ok_chars = new Dictionary<string, string>();
            xml_bad_to_ok_chars["<"] = "---lt---";
            xml_bad_to_ok_chars["&"] = "---amp---";
            xml_bad_to_ok_chars[">"] = "---gt---";
            xml_bad_to_ok_chars["\""] = "---quot---";
            xml_bad_to_ok_chars["\'"] = "---apos---";
            xml_bad_to_ok_chars[","] = "---comma---";
            xml_bad_to_ok_chars["."] = "---dot---";

            unique_layouts = new Dictionary<String, Dictionary<String, Point>>();
            flat_layout = new Dictionary<String, Point>();
            client_layout = new Dictionary<String, ClientLocation>();

            ignoring_size_sync = new Stopwatch();
            ignoring_size_sync.Start();

            InitializeComponent();
            init_options();

            //  DispatcherTimer setup
            dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
            dispatcherTimer.Start();

            is_initialized = true;

            previews_check_listbox.DisplayMember = "Text";

        }


        private void GlassForm_Load(object sender, EventArgs e)
        {
            refresh_thumbnails();
        }


        private void init_options()
        {
            option_zoom_on_hover.Checked = Properties.Settings.Default.zoom_on_hover;
            zoom_anchor_button_map = new Dictionary<zoom_anchor_t,RadioButton>();
            zoom_anchor_button_map[zoom_anchor_t.NW] = option_zoom_anchor_NW;
            zoom_anchor_button_map[zoom_anchor_t.N] = option_zoom_anchor_N;
            zoom_anchor_button_map[zoom_anchor_t.NE] = option_zoom_anchor_NE;
            zoom_anchor_button_map[zoom_anchor_t.W] = option_zoom_anchor_W;
            zoom_anchor_button_map[zoom_anchor_t.C] = option_zoom_anchor_C;
            zoom_anchor_button_map[zoom_anchor_t.E] = option_zoom_anchor_E;
            zoom_anchor_button_map[zoom_anchor_t.SW] = option_zoom_anchor_SW;
            zoom_anchor_button_map[zoom_anchor_t.S] = option_zoom_anchor_S;
            zoom_anchor_button_map[zoom_anchor_t.SE] = option_zoom_anchor_SE;
            zoom_anchor_button_map[(zoom_anchor_t)Properties.Settings.Default.zoom_anchor].Checked = true;
            option_zoom_factor.Text = Properties.Settings.Default.zoom_amount.ToString();
                        
            option_always_on_top.Checked = Properties.Settings.Default.always_on_top;
            option_hide_active.Checked = Properties.Settings.Default.hide_active;
            option_hide_all_if_not_right_type.Checked = Properties.Settings.Default.hide_all;
            
            option_unique_layout.Checked = Properties.Settings.Default.unique_layout;
            
            option_sync_size.Checked = Properties.Settings.Default.sync_resize;
            option_sync_size_x.Text = Properties.Settings.Default.sync_resize_x.ToString();
            option_sync_size_y.Text = Properties.Settings.Default.sync_resize_y.ToString();
            
            option_show_thumbnail_frames.Checked = Properties.Settings.Default.show_thumb_frames;
                      
            option_show_overlay.Checked = Properties.Settings.Default.show_overlay;

            option_track_client_windows.Checked = Properties.Settings.Default.track_client_windows;

            // disable/enable zoom suboptions
            option_zoom_factor.Enabled = Properties.Settings.Default.zoom_on_hover;
            foreach (var kv in zoom_anchor_button_map)
            {
                kv.Value.Enabled = Properties.Settings.Default.zoom_on_hover;
            }

            opacity_bar.Value = Math.Min(100, (int)(100.0*Properties.Settings.Default.opacity));

            load_layout();
        }


        private void spawn_and_kill_previews()
        {
            if (!is_initialized) { return; }

            Process[] processes = Process.GetProcessesByName("ExeFile");
            List<IntPtr> processHandles = new List<IntPtr>();

            // pop new previews

            foreach (Process process in processes)
            {
                processHandles.Add(process.MainWindowHandle);

                Size sync_size = new Size();
                sync_size.Width = (int)Properties.Settings.Default.sync_resize_x;
                sync_size.Height = (int)Properties.Settings.Default.sync_resize_y;

                if (!previews.ContainsKey(process.MainWindowHandle) && process.MainWindowTitle != "")
                {
                    previews[process.MainWindowHandle] = new Preview(process.MainWindowHandle, "...", this, sync_size);
                    previews[process.MainWindowHandle].set_render_area_size(sync_size);
 
                    // apply more thumbnail specific options
                    previews[process.MainWindowHandle].MakeTopMost(Properties.Settings.Default.always_on_top);
                    set_thumbnail_frame_style(previews[process.MainWindowHandle], Properties.Settings.Default.show_thumb_frames);

                    // add a preview also
                    previews_check_listbox.BeginUpdate();
                    previews_check_listbox.Items.Add(previews[process.MainWindowHandle]);
                    previews_check_listbox.EndUpdate();

                    refresh_client_window_locations(process);
                }

                else if (previews.ContainsKey(process.MainWindowHandle) && process.MainWindowTitle != previews[process.MainWindowHandle].Text) //or update the preview titles
                {
                    previews[process.MainWindowHandle].SetLabel(process.MainWindowTitle);
                    refresh_client_window_locations(process);
                }

                if (process.MainWindowHandle == DwmApi.GetForegroundWindow())
                {
                    active_client_handle = process.MainWindowHandle;
                    active_client_title = process.MainWindowTitle;
                }

            }

            // clean up old previews
            List<IntPtr> to_be_pruned = new List<IntPtr>();
            foreach (IntPtr processHandle in previews.Keys)
            {
                if (!(processHandles.Contains(processHandle)))
                {
                    to_be_pruned.Add(processHandle);
                }
            }

            foreach (IntPtr processHandle in to_be_pruned)
            {
                previews_check_listbox.BeginUpdate();
                previews_check_listbox.Items.Remove(previews[processHandle]);
                previews_check_listbox.EndUpdate();

                previews[processHandle].Close();
                previews.Remove(processHandle);
            }

            previews_check_listbox.Update();

        }

        private void refresh_client_window_locations(Process process)
        {
            if (Properties.Settings.Default.track_client_windows && client_layout.ContainsKey(process.MainWindowTitle))
            {
                MoveWindow(
                    process.MainWindowHandle,
                    client_layout[process.MainWindowTitle].X,
                    client_layout[process.MainWindowTitle].Y,
                    client_layout[process.MainWindowTitle].Width,
                    client_layout[process.MainWindowTitle].Height,
                    true);
            }
        }


        private string remove_nonconform_xml_characters(string entry)
        {
            foreach(var kv in xml_bad_to_ok_chars)
            {
                entry = entry.Replace(kv.Key, kv.Value);
            }
            return entry;
        }

        private string restore_nonconform_xml_characters(string entry)
        {
            foreach (var kv in xml_bad_to_ok_chars)
            {
                entry = entry.Replace(kv.Value, kv.Key);
            }
            return entry;
        }

        private XElement MakeXElement(string input)
        {
            string clean = remove_nonconform_xml_characters(input).Replace(" ", "_");
            return new XElement(clean);
        }

        private string ParseXElement(XElement input)
        {
            return restore_nonconform_xml_characters(input.Name.ToString()).Replace("_", " ");
        }

        private void load_layout()
        {
            if (File.Exists("layout.xml"))
            {
                XElement rootElement = XElement.Load("layout.xml");
                foreach (var el in rootElement.Elements())
                {
                    Dictionary<String, Point> inner = new Dictionary<String, Point>();
                    foreach (var inner_el in el.Elements())
                    {
                        inner[ParseXElement(inner_el)] = new Point(Convert.ToInt32(inner_el.Element("x").Value), Convert.ToInt32(inner_el.Element("y").Value));
                    }
                    unique_layouts[ParseXElement(el)] = inner;
                }
            }

            if (File.Exists("flat_layout.xml"))
            {
                XElement rootElement = XElement.Load("flat_layout.xml");
                foreach (var el in rootElement.Elements())
                {
                    flat_layout[ParseXElement(el)] = new Point(Convert.ToInt32(el.Element("x").Value), Convert.ToInt32(el.Element("y").Value));
                }
            }

            if (File.Exists("client_layout.xml"))
            {
                XElement rootElement = XElement.Load("client_layout.xml");
                foreach (var el in rootElement.Elements())
                {
                    ClientLocation clientLocation = new ClientLocation();
                    clientLocation.X = Convert.ToInt32(el.Element("x").Value);
                    clientLocation.Y = Convert.ToInt32(el.Element("y").Value);
                    clientLocation.Width = Convert.ToInt32(el.Element("width").Value);
                    clientLocation.Height = Convert.ToInt32(el.Element("height").Value);

                    client_layout[ParseXElement(el)] = clientLocation;
                }
            }
        }

        private void store_layout()
        {
            XElement el = new XElement("layouts");
            foreach (var client in unique_layouts.Keys)
            {
                if (client == "")
                {
                    continue;
                }
                XElement layout = MakeXElement(client);
                foreach (var thumbnail_ in unique_layouts[client])
                {
                    String thumbnail = thumbnail_.Key;
                    if (thumbnail == "" || thumbnail == "...")
                    {
                        continue;
                    }
                    XElement position = MakeXElement(thumbnail);
                    position.Add(new XElement("x", thumbnail_.Value.X));
                    position.Add(new XElement("y", thumbnail_.Value.Y));
                    layout.Add(position);
                }
                el.Add(layout);
            }

            el.Save("layout.xml");

            XElement el2 = new XElement("flat_layout");
            foreach (var clientKV in flat_layout)
            {
                if (clientKV.Key == "" || clientKV.Key == "...")
                {
                    continue;
                }
                XElement layout = MakeXElement(clientKV.Key);
                layout.Add(new XElement("x", clientKV.Value.X));
                layout.Add(new XElement("y", clientKV.Value.Y));
                el2.Add(layout);
            }

            el2.Save("flat_layout.xml");

            XElement el3 = new XElement("client_layout");
            foreach (var clientKV in client_layout)
            {
                if (clientKV.Key == "" || clientKV.Key == "...")
                {
                    continue;
                }
                XElement layout = MakeXElement(clientKV.Key);
                layout.Add(new XElement("x", clientKV.Value.X));
                layout.Add(new XElement("y", clientKV.Value.Y));
                layout.Add(new XElement("width", clientKV.Value.Width));
                layout.Add(new XElement("height", clientKV.Value.Height));
                el3.Add(layout);
            }

            el3.Save("client_layout.xml");
        }

        private void handle_unique_layout(Preview preview, String last_known_active_window)
        {
            Dictionary<String, Point> layout;
            if (unique_layouts.TryGetValue(last_known_active_window, out layout))
            {
                Point new_loc;
                if ( Properties.Settings.Default.unique_layout && layout.TryGetValue(preview.Text, out new_loc))
                {
                    preview.doMove(new_loc);
                }
                else
                {
                    // create inner dict
                    layout[preview.Text] = preview.Location;
                }
            }
            else if (last_known_active_window != "")
            {
                // create outer dict
                unique_layouts[last_known_active_window] = new Dictionary<String, Point>();
                unique_layouts[last_known_active_window][preview.Text] = preview.Location;
            }
        }


        private void update_client_locations()
        {
            Process[] processes = Process.GetProcessesByName("ExeFile");
            List<IntPtr> processHandles = new List<IntPtr>();

            foreach (Process process in processes)
            {
                Rect rect = new Rect();
                GetWindowRect(process.MainWindowHandle, out rect);

                int left = Math.Abs(rect.Left);
                int right = Math.Abs(rect.Right);
                int client_width = Math.Abs(left - right);

                int top = Math.Abs(rect.Top);
                int bottom = Math.Abs(rect.Bottom);
                int client_height = Math.Abs(top - bottom);

                ClientLocation clientLocation = new ClientLocation();
                clientLocation.X = rect.Left;
                clientLocation.Y = rect.Top;
                clientLocation.Width = client_width;
                clientLocation.Height = client_height;


                client_layout[process.MainWindowTitle] = clientLocation;
            }
        }


        public void preview_did_switch()
        {
            update_client_locations();
            store_layout(); //todo: check if it actually changed ...
            foreach (KeyValuePair<IntPtr, Preview> entry in previews)
            {
                entry.Value.MakeTopMost(Properties.Settings.Default.always_on_top);
            }
        }


        private void handle_flat_layout(Preview preview)
        {
            Point layout;
            if (flat_layout.TryGetValue(preview.Text, out layout))
            {
                 preview.doMove( layout );
            }
            else if (preview.Text != "")
            {
                flat_layout[preview.Text] = preview.Location;
            }
        }

        private bool window_is_preview_or_client(IntPtr window)
        {
            bool active_window_is_right_type = false;
            foreach (KeyValuePair<IntPtr, Preview> entry in previews)
            {
                if (entry.Key == window || entry.Value.Handle == window || this.Handle == window || entry.Value.overlay.Handle == window)
                {
                    active_window_is_right_type = true;
                }
            }
            return active_window_is_right_type;
        }


        private void refresh_thumbnails()
        {

            IntPtr active_window = DwmApi.GetForegroundWindow();
            
            // hide, show, resize and move
            foreach (KeyValuePair<IntPtr, Preview> entry in previews)
            {
                if (!window_is_preview_or_client(active_window) && Properties.Settings.Default.hide_all)
                {
                    entry.Value.Hide();
                }
                else if (entry.Key == active_client_handle && Properties.Settings.Default.hide_active)
                {
                    entry.Value.Hide();
                }
                else
                {
                    entry.Value.Show();
                    if (Properties.Settings.Default.unique_layout)
                    {
                        handle_unique_layout(entry.Value, active_client_title);
                    }
                    else
                    {
                        handle_flat_layout(entry.Value);
                    }
                }
                entry.Value.hover_zoom = Properties.Settings.Default.zoom_on_hover;
                entry.Value.show_overlay = Properties.Settings.Default.show_overlay;
                if (!entry.Value.is_hovered_over)
                {
                    entry.Value.Opacity = Properties.Settings.Default.opacity;
                }
            }

            DwmApi.DwmIsCompositionEnabled();
        }


        public void syncronize_preview_size(Size sync_size)
        {
            if (!is_initialized) { return; }

            if (Properties.Settings.Default.sync_resize && 
                Properties.Settings.Default.show_thumb_frames && 
                ignoring_size_sync.ElapsedMilliseconds > 500)
            {
                ignoring_size_sync.Stop();

                option_sync_size_x.Text = sync_size.Width.ToString();
                option_sync_size_y.Text = sync_size.Height.ToString();

                foreach (KeyValuePair<IntPtr, Preview> entry in previews)
                {
                    if (entry.Value.Handle != DwmApi.GetForegroundWindow())
                    {
                        entry.Value.set_render_area_size(sync_size);
                    }
                }

            }

        }


        public void register_preview_position(String preview_title, Point position)
        {
            
            if (Properties.Settings.Default.unique_layout)
            {
                Dictionary<String, Point> layout;
                if (unique_layouts.TryGetValue(active_client_title, out layout))
                {
                    layout[preview_title] = position;
                }
                else if (active_client_title == "")
                {
                    unique_layouts[active_client_title] = new Dictionary<String, Point>();
                    unique_layouts[active_client_title][preview_title] = position;
                }
            }
            else
            {
                flat_layout[preview_title] = position;
            }
             
        }


        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            spawn_and_kill_previews();
            refresh_thumbnails();
            if (ignoring_size_sync.ElapsedMilliseconds > 500) { ignoring_size_sync.Stop(); };

            if(DwmApi.DwmIsCompositionEnabled())
            {
                aero_status_label.Text = "AERO is ON";
                aero_status_label.ForeColor = Color.Black;
            }
            else{
                aero_status_label.Text = "AERO is OFF";
                aero_status_label.ForeColor = Color.Red;
            }
            
        }


        private void option_hide_all_if_noneve_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.hide_all = option_hide_all_if_not_right_type.Checked;
            Properties.Settings.Default.Save();
            refresh_thumbnails();
        }


        private void option_unique_layout_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.unique_layout = option_unique_layout.Checked;
            Properties.Settings.Default.Save();
            refresh_thumbnails();
        }


        private void option_hide_active_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.hide_active = option_hide_active.Checked;
            Properties.Settings.Default.Save();
            refresh_thumbnails();
        }


        private void option_sync_size_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.sync_resize = option_sync_size.Checked;
            Properties.Settings.Default.Save();
            refresh_thumbnails();
        }


        private void parse_size_entry()
        {
            UInt32 x = 0, y = 0;

            try
            {
                y = Convert.ToUInt32(option_sync_size_y.Text);
                x = Convert.ToUInt32(option_sync_size_x.Text);
            }
            catch (System.FormatException) {
                return;
            }

            if (x < 64 || y < 64)
            {
                return;
            }

            Properties.Settings.Default.sync_resize_y = y;
            Properties.Settings.Default.sync_resize_x = x;
            Properties.Settings.Default.Save();

            // resize
            syncronize_preview_size(new Size((int)Properties.Settings.Default.sync_resize_x,
                                             (int)Properties.Settings.Default.sync_resize_y));
        }


        private void option_sync_size_x_TextChanged(object sender, EventArgs e)
        {
            parse_size_entry();
        }


        private void option_sync_size_y_TextChanged(object sender, EventArgs e)
        {
            parse_size_entry();
        }
        

        private void option_always_on_top_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.always_on_top = option_always_on_top.Checked;
            Properties.Settings.Default.Save();
            refresh_thumbnails();
        }


        void set_thumbnail_frame_style(Preview preview, bool show_frames)
        {
            if (show_frames)
            {
                preview.FormBorderStyle = FormBorderStyle.SizableToolWindow;
            }
            else
            {
                preview.FormBorderStyle = FormBorderStyle.None;
            }
        }

        private void option_show_thumbnail_frames_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.show_thumb_frames = option_show_thumbnail_frames.Checked;
            Properties.Settings.Default.Save();

            if (Properties.Settings.Default.show_thumb_frames)
            {
                ignoring_size_sync.Stop();
                ignoring_size_sync.Reset();
                ignoring_size_sync.Start();
            }

            foreach (var thumbnail in previews)
            {
                set_thumbnail_frame_style(thumbnail.Value, Properties.Settings.Default.show_thumb_frames);
            }

        }


        private void list_running_clients_SelectedIndexChanged(object sender, EventArgs e){}


        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            string url = "https://bitbucket.org/ulph/eve-o-preview-git";
            ProcessStartInfo sInfo = new ProcessStartInfo(new Uri(url).AbsoluteUri);
            Process.Start(sInfo);
        }


        private void previewToyMainBindingSource_CurrentChanged(object sender, EventArgs e)
        {

        }

        private void option_zoom_on_hover_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.zoom_on_hover = option_zoom_on_hover.Checked;
            Properties.Settings.Default.Save();
            refresh_thumbnails();
            option_zoom_factor.Enabled = Properties.Settings.Default.zoom_on_hover;
            if (is_initialized)
            {
                foreach (var kv in zoom_anchor_button_map)
                {
                    kv.Value.Enabled = Properties.Settings.Default.zoom_on_hover;
                }
            }
        }

        private void option_show_overlay_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.show_overlay = option_show_overlay.Checked;
            Properties.Settings.Default.Save();
            refresh_thumbnails();
        }


        private void handle_zoom_anchor_setting()
        {
            foreach (var kv in zoom_anchor_button_map)
            {
                if (kv.Value.Checked == true)
                    Properties.Settings.Default.zoom_anchor = (byte)kv.Key;
            }
        }

        private void option_zoom_anchor_X_CheckedChanged(object sender, EventArgs e)
        {
            handle_zoom_anchor_setting();
            Properties.Settings.Default.Save();
        }

        private void option_zoom_factor_TextChanged(object sender, EventArgs e)
        {
            try
            {
                float tmp = (float)Convert.ToDouble(option_zoom_factor.Text);
                if(tmp < 1)
                {
                    tmp = 1;
                }
                else if(tmp > 10)
                {
                    tmp = 10;
                }
                Properties.Settings.Default.zoom_amount = tmp;
                option_zoom_factor.Text = tmp.ToString();
                Properties.Settings.Default.Save();
            }
            catch
            {
                // do naught
            }
        }

        private void checkedListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            refresh_thumbnails();
        }

        private void checkedListBox1_SelectedIndexChanged2(object sender, EventArgs e)
        {
            System.Windows.Forms.ItemCheckEventArgs arg = (System.Windows.Forms.ItemCheckEventArgs)e;
            ((Preview)this.previews_check_listbox.Items[arg.Index]).MakeHidden(arg.NewValue == System.Windows.Forms.CheckState.Checked);
            refresh_thumbnails();
        }

        private void flowLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.track_client_windows = option_track_client_windows.Checked;
            Properties.Settings.Default.Save();
            refresh_thumbnails();
        }

        private void opacityCheckbox_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void opacity_bar_Scroll(object sender, ScrollEventArgs e)
        {
            // fire off opacity change
            Properties.Settings.Default.opacity = Math.Min((float)e.NewValue / 100.0f, 1.0f);
            Properties.Settings.Default.Save();
            refresh_thumbnails();
        }

    }
}