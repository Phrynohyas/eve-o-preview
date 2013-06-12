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
    public partial class PreviewToyMain : Form
    {
        private Dictionary<IntPtr, Preview> thumbnails;
        private DispatcherTimer dispatcherTimer;

        private IntPtr last_known_active_window;

        private Dictionary<IntPtr, Dictionary<IntPtr, Point>> layouts;

        private Size sync_size;

        private bool is_initialized;

        public PreviewToyMain()
        {
            is_initialized = false;

            thumbnails = new Dictionary<IntPtr, Preview>();

            layouts = new Dictionary<IntPtr, Dictionary<IntPtr, Point>>();

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
            sync_size.Width = (int)Properties.Settings.Default.sync_resize_x;
            sync_size.Height = (int)Properties.Settings.Default.sync_resize_y;
        }

        private void refresh_thumbnails()
        {
            if (!is_initialized) { return; }

            var sys_activeWindow = DwmApi.GetForegroundWindow();

            Process[] processes = Process.GetProcessesByName("ExeFile");
            List<IntPtr> processHandles = new List<IntPtr>();

            // pop new previews
            if (processes.Length > 0)
            {
                foreach (Process process in processes)
                {
                    processHandles.Add(process.MainWindowHandle);

                    if (!thumbnails.ContainsKey(process.MainWindowHandle) && process.MainWindowTitle != "")
                    {
                        Size initial_size = new Size(64, 64);
                        if (option_sync_size.Checked){initial_size = sync_size;}
                        thumbnails[process.MainWindowHandle] = new Preview(process.MainWindowHandle, "-> " + process.MainWindowTitle + " <-", this, initial_size);
                        this.list_running_clients.Items.Add(process.MainWindowTitle + " ("+process.MainWindowHandle+")");
                    }
                    else if (thumbnails.ContainsKey(process.MainWindowHandle)) //or update the preview titles
                    {
                        thumbnails[process.MainWindowHandle].Text = "-> " + process.MainWindowTitle + " <-";
                    }
                        

                    if (sys_activeWindow == process.MainWindowHandle)
                    {
                        last_known_active_window = sys_activeWindow;
                    }
                }
            }

            
            // clean up old previews
            if (thumbnails.Count > 0)
            {
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
                    this.list_running_clients.Items.Remove(processHandle);
                }
            }

            if (thumbnails.Count > 0)
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

                // update the list selection
                if (active_window_is_right_type)
                {
                    int active_index = list_running_clients.Items.IndexOf(last_known_active_window);
                    if(active_index != -1)
                    {
                        list_running_clients.SetSelected(active_index, true);
                    }
                }
                else
                {
                    list_running_clients.ClearSelected();
                }

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

                        if (option_sync_size.Checked && entry.Value.Handle != sys_activeWindow)
                        {
                            entry.Value.Size = this.sync_size;
                        }

                        if (option_unique_layout.Checked)
                        {                           
                            Dictionary<IntPtr, Point> layout;
                            if(layouts.TryGetValue(last_known_active_window, out layout))
                            {
                                Point new_loc;
                                if (layout.TryGetValue(entry.Value.Handle, out new_loc))
                                {
                                    entry.Value.Location = new_loc;
                                }
                                else
                                {
                                    // create inner dict
                                    layout[entry.Value.Handle] = entry.Value.Location;
                                }
                            }
                            else if ((int)last_known_active_window != 0)
                            {
                                // create outer dict
                                layouts[last_known_active_window] = new Dictionary<IntPtr, Point>();
                                layouts[last_known_active_window][entry.Value.Handle] = entry.Value.Location;
                            }
                        }
                    }
                }
            }
        }


        public void set_sync_size(Size sync_size_)
        {
            if (!is_initialized) { return; }

            if (option_sync_size.Checked)
            {
                this.sync_size = sync_size_;
                option_sync_size_x.Text = this.sync_size.Width.ToString();
                option_sync_size_y.Text = this.sync_size.Height.ToString();
            }
        }


        public void set_preview_position(IntPtr preview_handle, Point position)
        {
            Dictionary<IntPtr, Point> layout;
            if (layouts.TryGetValue(last_known_active_window, out layout))
            {
                layout[preview_handle] = position;
            }
            else if((int)last_known_active_window != 0)
            {
                layouts[last_known_active_window] = new Dictionary<IntPtr, Point>();
                layouts[last_known_active_window][preview_handle] = position;
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

                this.sync_size.Height = (int)Properties.Settings.Default.sync_resize_y;
                this.sync_size.Width = (int)Properties.Settings.Default.sync_resize_x;
            }
            return true;
        }


        private void option_sync_size_x_TextChanged(object sender, EventArgs e)
        {
            if (try_save_size_xy()) { refresh_thumbnails(); }
        }


        private void option_sync_size_y_TextChanged(object sender, EventArgs e)
        {
            if (try_save_size_xy()) { refresh_thumbnails(); }
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


        private void option_show_thumbnail_frames_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.show_thumb_frames = option_show_thumbnail_frames.Checked;
            Properties.Settings.Default.Save();
            foreach (var thumbnail in thumbnails)
            {
                if (Properties.Settings.Default.show_thumb_frames)
                {
                    thumbnail.Value.FormBorderStyle = FormBorderStyle.SizableToolWindow;
                }
                else
                {
                    thumbnail.Value.FormBorderStyle = FormBorderStyle.None;
                }
            }
        }

        private void list_running_clients_SelectedIndexChanged(object sender, EventArgs e){}

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            string url = "https://forums.eveonline.com/default.aspx?g=posts&t=246157";
            ProcessStartInfo sInfo = new ProcessStartInfo(new Uri(url).AbsoluteUri);
            Process.Start(sInfo);
        }
    }
}