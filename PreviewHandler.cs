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

namespace PreviewToy
{
    public partial class PreviewToyHandler : Form
    {
        private Dictionary<IntPtr, Preview> thumbnails;
        private DispatcherTimer dispatcherTimer;

        private IntPtr last_known_active_window_;

        private Dictionary<IntPtr, Dictionary<IntPtr, Point>> layouts;

        private bool is_initialized;

        private bool frames_were_hidden = false;
        private Stopwatch ignoring_size_sync;

        public PreviewToyHandler()
        {
            is_initialized = false;

            thumbnails = new Dictionary<IntPtr, Preview>();

            layouts = new Dictionary<IntPtr, Dictionary<IntPtr, Point>>();

            ignoring_size_sync = new Stopwatch();

            InitializeComponent();
            init_options();

            //  DispatcherTimer setup
            dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
            dispatcherTimer.Start();

           

            is_initialized = true;
        }


        private void GlassForm_Load(object sender, EventArgs e)
        {
            refresh_thumbnails();
        }


        private void init_options()
        {
            option_always_on_top.Checked = Properties.Settings.Default.always_on_top;
            option_hide_active.Checked = Properties.Settings.Default.hide_active;
            option_hide_all_if_not_right_type.Checked = Properties.Settings.Default.hide_all;
            option_unique_layout.Checked = Properties.Settings.Default.unique_layout;
            option_sync_size.Checked = Properties.Settings.Default.sync_resize;
            option_show_thumbnail_frames.Checked = Properties.Settings.Default.show_thumb_frames;
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

                if (!thumbnails.ContainsKey(process.MainWindowHandle) && process.MainWindowTitle != "")
                {
                    thumbnails[process.MainWindowHandle] = new Preview(process.MainWindowHandle, "...", this, sync_size);
                    thumbnails[process.MainWindowHandle].set_render_area_size(sync_size);
 
                    // apply more thumbnail specific options
                    thumbnails[process.MainWindowHandle].TopMost = Properties.Settings.Default.always_on_top;
                    set_thumbnail_frame_style(thumbnails[process.MainWindowHandle], Properties.Settings.Default.show_thumb_frames);

                }

                else if (thumbnails.ContainsKey(process.MainWindowHandle)) //or update the preview titles
                {
                    thumbnails[process.MainWindowHandle].Text = "-> " + process.MainWindowTitle + " <-";
                }

            }


            // clean up old previews

            List<IntPtr> to_be_pruned = new List<IntPtr>();
            foreach (IntPtr processHandle in thumbnails.Keys)
            {
                if (!(processHandles.Contains(processHandle)))
                {
                    to_be_pruned.Add(processHandle);
                }
            }

            foreach (IntPtr processHandle in to_be_pruned)
            {
                thumbnails[processHandle].Close();
                thumbnails.Remove(processHandle);
                layouts.Remove(processHandle);
            }
        }

        private void handle_unique_layout(Preview preview, IntPtr last_known_active_window)
        {
            Dictionary<IntPtr, Point> layout;
            if (layouts.TryGetValue(last_known_active_window, out layout))
            {
                Point new_loc;
                if (layout.TryGetValue(preview.Handle, out new_loc))
                {
                    preview.Location = new_loc;
                }
                else
                {
                    // create inner dict
                    layout[preview.Handle] = preview.Location;
                }
            }
            else if ((int)last_known_active_window != 0)
            {
                // create outer dict
                layouts[last_known_active_window] = new Dictionary<IntPtr, Point>();
                layouts[last_known_active_window][preview.Handle] = preview.Location;
            }
        }


        private void hide_show_move_thumbnails(IntPtr last_known_active_window, IntPtr sys_activeWindow, bool active_window_is_right_type)
        {
            // hide, show, resize and move
            foreach (KeyValuePair<IntPtr, Preview> entry in thumbnails)
            {
                if (!active_window_is_right_type && option_hide_all_if_not_right_type.Checked)
                {
                    entry.Value.Hide();
                }
                else if (entry.Key == last_known_active_window && option_hide_active.Checked)
                {
                    entry.Value.Hide();
                }
                else
                {
                    entry.Value.Show();

                    if (option_unique_layout.Checked)
                    {
                        handle_unique_layout(entry.Value, last_known_active_window);
                    }
                }
            }
        }

        private bool is_active_window_right_type(IntPtr sys_activeWindow)
        {
            // is the active window an eve window?
            bool active_window_is_right_type = false;
            foreach (KeyValuePair<IntPtr, Preview> entry in thumbnails)
            {
                if (entry.Key == sys_activeWindow || entry.Value.Handle == sys_activeWindow || this.Handle == sys_activeWindow)
                {
                    active_window_is_right_type = true;
                }
            }
            return active_window_is_right_type;
        }


        private void refresh_thumbnails()
        {
            spawn_and_kill_previews();

            IntPtr sys_activeWindow = DwmApi.GetForegroundWindow();
            Preview poo;
            if( thumbnails.TryGetValue(sys_activeWindow, out poo) )
            {
                last_known_active_window_ = sys_activeWindow;
            }
            
            bool active_window_is_right_type = is_active_window_right_type(sys_activeWindow);
            hide_show_move_thumbnails(last_known_active_window_, sys_activeWindow, active_window_is_right_type);

            if (ignoring_size_sync.ElapsedMilliseconds > 500) { ignoring_size_sync.Stop(); };
        }


        public void set_sync_size(Size sync_size)
        {
            if (!is_initialized) { return; }

            if (option_sync_size.Checked && option_show_thumbnail_frames.Checked && ignoring_size_sync.ElapsedMilliseconds > 500)
            {
                ignoring_size_sync.Stop();

                option_sync_size_x.Text = sync_size.Width.ToString();
                option_sync_size_y.Text = sync_size.Height.ToString();

                foreach (KeyValuePair<IntPtr, Preview> entry in thumbnails)
                {
                    if (entry.Value.Handle != DwmApi.GetForegroundWindow())
                    {
                        entry.Value.set_render_area_size(sync_size);
                    }
                }

            }

        }


        public void set_preview_position(IntPtr preview_handle, Point position)
        {
            Dictionary<IntPtr, Point> layout;
            if (layouts.TryGetValue(last_known_active_window_, out layout))
            {
                layout[preview_handle] = position;
            }
            else if ((int)last_known_active_window_ != 0)
            {
                layouts[last_known_active_window_] = new Dictionary<IntPtr, Point>();
                layouts[last_known_active_window_][preview_handle] = position;
            }
        }

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            refresh_thumbnails();
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

        private bool try_save_size_xy()
        {
            if (Properties.Settings.Default.sync_resize)
            {
                UInt32 x = 0, y = 0;

                try
                {
                    y = Convert.ToUInt32(option_sync_size_y.Text);
                    x = Convert.ToUInt32(option_sync_size_x.Text);
                }
                catch (System.FormatException)
                {
                    return false;
                }

                if (y < 64 || x < 64)
                {
                    return false;
                }

                Properties.Settings.Default.sync_resize_y = y;
                Properties.Settings.Default.sync_resize_x = x;
                Properties.Settings.Default.Save();

                set_sync_size(new Size((int)x, (int)y));
            }
            return true;
        }


        private void option_sync_size_x_TextChanged(object sender, EventArgs e)
        {
            if (try_save_size_xy()) { }
        }


        private void option_sync_size_y_TextChanged(object sender, EventArgs e)
        {
            if (try_save_size_xy()) { }
        }
        

        private void option_always_on_top_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.always_on_top = option_always_on_top.Checked;
            Properties.Settings.Default.Save();
            foreach (var thumbnail in thumbnails)
            {
                thumbnail.Value.TopMost = Properties.Settings.Default.always_on_top;
            }
        }


        private void set_thumbnail_frame_style(Preview preview, bool show_frames)
        {
            if (show_frames)
            {
                preview.FormBorderStyle = FormBorderStyle.SizableToolWindow;
                ignoring_size_sync.Stop();
                ignoring_size_sync.Reset();
                ignoring_size_sync.Start();
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
            foreach (var thumbnail in thumbnails)
            {
                set_thumbnail_frame_style(thumbnail.Value, Properties.Settings.Default.show_thumb_frames);
            }
        }


        private void list_running_clients_SelectedIndexChanged(object sender, EventArgs e){}


        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            string url = "https://forums.eveonline.com/default.aspx?g=posts&t=246157";
            ProcessStartInfo sInfo = new ProcessStartInfo(new Uri(url).AbsoluteUri);
            Process.Start(sInfo);
        }


        private void previewToyMainBindingSource_CurrentChanged(object sender, EventArgs e)
        {

        }


    }
}