using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Diagnostics;
using System.Globalization;
using System.Windows.Threading;
using System.Xml.Linq;
using System.IO;

namespace EveOPreview
{
	public partial class MainForm : Form
	{
		public event EventHandler Minimized;
		public event EventHandler Maximized;
		public event EventHandler Restored;

		private readonly bool _isInitialized;
		private readonly ThumbnailManager _manager;


		private Dictionary<ZoomAnchor, RadioButton> _zoomAnchorButtonMap;

		public MainForm()
		{
			_isInitialized = false;

			InitializeComponent();
			init_options();

			// TODO Fix this
			previews_check_listbox.DisplayMember = "Text";

			this._manager = new ThumbnailManager(add_thumbnail,remove_thumbnail, set_aero_status, set_size);

			_isInitialized = true;

			this._manager.Activate();
		}

		private void add_thumbnail(IList<string> thumbnails)
		{
			this.previews_check_listbox.BeginUpdate();
			foreach (string th in thumbnails)
			{
				previews_check_listbox.Items.Add(th);
			}
			this.previews_check_listbox.EndUpdate();
		}

		private void remove_thumbnail(IList<string> thumbnails)
		{
			this.previews_check_listbox.BeginUpdate();
			foreach (string th in thumbnails)
			{
				previews_check_listbox.Items.Remove(th);
			}
			this.previews_check_listbox.EndUpdate();
		}

		private void set_aero_status(bool value)
		{

			if (value)
			{
				aero_status_label.Text = "AERO is ON";
				aero_status_label.ForeColor = Color.Black;
			}
			else
			{
				aero_status_label.Text = "AERO is OFF";
				aero_status_label.ForeColor = Color.Red;
			}
		}

		private void set_size(int x, int y)
		{
			option_sync_size_x.Text = x.ToString();
			option_sync_size_y.Text = y.ToString();
		}

		private void GlassForm_Load(object sender, EventArgs e)
		{
			this._manager.refresh_thumbnails();
		}

		private void init_options()
		{
			this.Minimized += MainForm_Minimized;

			option_zoom_on_hover.Checked = Properties.Settings.Default.zoom_on_hover;
			_zoomAnchorButtonMap = new Dictionary<ZoomAnchor, RadioButton>();
			_zoomAnchorButtonMap[ZoomAnchor.NW] = option_zoom_anchor_NW;
			_zoomAnchorButtonMap[ZoomAnchor.N] = option_zoom_anchor_N;
			_zoomAnchorButtonMap[ZoomAnchor.NE] = option_zoom_anchor_NE;
			_zoomAnchorButtonMap[ZoomAnchor.W] = option_zoom_anchor_W;
			_zoomAnchorButtonMap[ZoomAnchor.C] = option_zoom_anchor_C;
			_zoomAnchorButtonMap[ZoomAnchor.E] = option_zoom_anchor_E;
			_zoomAnchorButtonMap[ZoomAnchor.SW] = option_zoom_anchor_SW;
			_zoomAnchorButtonMap[ZoomAnchor.S] = option_zoom_anchor_S;
			_zoomAnchorButtonMap[ZoomAnchor.SE] = option_zoom_anchor_SE;
			_zoomAnchorButtonMap[(ZoomAnchor)Properties.Settings.Default.zoom_anchor].Checked = true;
			option_zoom_factor.Text = Properties.Settings.Default.zoom_amount.ToString(CultureInfo.InvariantCulture);

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

			option_minToTray.Checked = Properties.Settings.Default.minimizeToTray;

			// disable/enable zoom suboptions
			option_zoom_factor.Enabled = Properties.Settings.Default.zoom_on_hover;
			foreach (var kv in _zoomAnchorButtonMap)
			{
				kv.Value.Enabled = Properties.Settings.Default.zoom_on_hover;
			}

			opacity_bar.Value = Math.Min(100, (int)(100.0 * Properties.Settings.Default.opacity));
		}




		private void option_hide_all_if_noneve_CheckedChanged(object sender, EventArgs e)
		{
			if (!_isInitialized)
			{
				return;
			}

			Properties.Settings.Default.hide_all = option_hide_all_if_not_right_type.Checked;
			Properties.Settings.Default.Save();
			this._manager.refresh_thumbnails();
		}


		private void option_unique_layout_CheckedChanged(object sender, EventArgs e)
		{
			if (!_isInitialized)
			{
				return;
			}

			Properties.Settings.Default.unique_layout = option_unique_layout.Checked;
			Properties.Settings.Default.Save();
			this._manager.refresh_thumbnails();
		}


		private void option_hide_active_CheckedChanged(object sender, EventArgs e)
		{
			if (!_isInitialized)
			{
				return;
			}

			Properties.Settings.Default.hide_active = option_hide_active.Checked;
			Properties.Settings.Default.Save();
			this._manager.refresh_thumbnails();
		}


		private void option_sync_size_CheckedChanged(object sender, EventArgs e)
		{
			if (!_isInitialized)
			{
				return;
			}

			Properties.Settings.Default.sync_resize = option_sync_size.Checked;
			Properties.Settings.Default.Save();
			this._manager.refresh_thumbnails();
		}


		private void parse_size_entry()
		{
			UInt32 x = 0, y = 0;

			try
			{
				y = Convert.ToUInt32(option_sync_size_y.Text);
				x = Convert.ToUInt32(option_sync_size_x.Text);
			}
			catch (System.FormatException)
			{
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
			this._manager.SyncPreviewSize(new Size((int)Properties.Settings.Default.sync_resize_x,
											 (int)Properties.Settings.Default.sync_resize_y));
		}


		private void option_sync_size_x_TextChanged(object sender, EventArgs e)
		{
			if (!_isInitialized)
			{
				return;
			}

			parse_size_entry();
		}


		private void option_sync_size_y_TextChanged(object sender, EventArgs e)
		{
			if (!_isInitialized)
			{
				return;
			}

			parse_size_entry();
		}


		private void option_always_on_top_CheckedChanged(object sender, EventArgs e)
		{
			if (!_isInitialized)
			{
				return;
			}

			Properties.Settings.Default.always_on_top = option_always_on_top.Checked;
			Properties.Settings.Default.Save();
			this._manager.refresh_thumbnails();
		}


		private void option_show_thumbnail_frames_CheckedChanged(object sender, EventArgs e)
		{
			if (!_isInitialized)
			{
				return;
			}

			Properties.Settings.Default.show_thumb_frames = option_show_thumbnail_frames.Checked;
			Properties.Settings.Default.Save();

			this._manager.set_frames();
		}

		private void list_running_clients_SelectedIndexChanged(object sender, EventArgs e) { }


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
			if (!_isInitialized) return;

			Properties.Settings.Default.zoom_on_hover = option_zoom_on_hover.Checked;
			Properties.Settings.Default.Save();
			this._manager.refresh_thumbnails();
			option_zoom_factor.Enabled = Properties.Settings.Default.zoom_on_hover;
	
				foreach (var kv in _zoomAnchorButtonMap)
				{
					kv.Value.Enabled = Properties.Settings.Default.zoom_on_hover;
				}
			
		}

		private void option_show_overlay_CheckedChanged(object sender, EventArgs e)
		{
			Properties.Settings.Default.show_overlay = option_show_overlay.Checked;
			Properties.Settings.Default.Save();
			this._manager.refresh_thumbnails();
		}


		private void handle_zoom_anchor_setting()
		{
			foreach (var kv in _zoomAnchorButtonMap)
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
				if (tmp < 1)
				{
					tmp = 1;
				}
				else if (tmp > 10)
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
			this._manager.refresh_thumbnails();
		}

		private void checkedListBox1_SelectedIndexChanged2(object sender, EventArgs e)
		{
			System.Windows.Forms.ItemCheckEventArgs arg = (System.Windows.Forms.ItemCheckEventArgs)e;
			((ThumbnailWindow)this.previews_check_listbox.Items[arg.Index]).IsPreviewEnabled = (arg.NewValue != System.Windows.Forms.CheckState.Checked);
			this._manager.refresh_thumbnails();
		}

		private void flowLayoutPanel1_Paint(object sender, PaintEventArgs e)
		{

		}

		private void checkBox1_CheckedChanged(object sender, EventArgs e)
		{
			if (!_isInitialized)
			{
				return;
			}
			Properties.Settings.Default.track_client_windows = option_track_client_windows.Checked;
			Properties.Settings.Default.Save();
			this._manager.refresh_thumbnails();
		}


		private void opacity_bar_Scroll(object sender, ScrollEventArgs e)
		{
			if (!_isInitialized)
			{
				return;
			}
			// fire off opacity change
			Properties.Settings.Default.opacity = Math.Min((float)e.NewValue / 100.0f, 1.0f);
			Properties.Settings.Default.Save();
			this._manager.refresh_thumbnails();
		}


		private void OnMinimized(EventArgs e)
		{
			if (Minimized != null && Properties.Settings.Default.minimizeToTray)
			{
				this.Hide();
			}
			else if (Minimized != null && !Properties.Settings.Default.minimizeToTray)
			{
				Minimized(this, e);
			}
		}

		private void OnMaximized(EventArgs e)
		{
			if (Maximized != null)
			{
				Maximized(this, e);
			}
		}

		private void OnRestored(EventArgs e)
		{
			Restored?.Invoke(this, e);
		}

		protected override void WndProc(ref Message m)
		{
			switch (m.Msg)
			{
				case DwmApiNativeMethods.WM_SIZE:
					switch (m.WParam.ToInt32())
					{
						case DwmApiNativeMethods.SIZE_RESTORED:
							OnRestored(EventArgs.Empty);
							break;
						case DwmApiNativeMethods.SIZE_MINIMIZED:
							OnMinimized(EventArgs.Empty);
							break;
						case DwmApiNativeMethods.SIZE_MAXIMIZED:
							OnMaximized(EventArgs.Empty);
							break;
					}
					break;
			}

			base.WndProc(ref m);
		}

		void MainForm_Minimized(object sender, EventArgs e)
		{
			// TODO: do something here
		}

		private void option_minToTray_CheckedChanged(object sender, EventArgs e)
		{
			Properties.Settings.Default.minimizeToTray = option_minToTray.Checked;
			Properties.Settings.Default.Save();
		}

		private void exitToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Application.Exit();
		}

		private void restoreToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (!this.Visible)
			{
				this.Show();
			}
			else if (Restored != null)
			{
				Restored(this, e);
			}
			else
			{
				this.BringToFront();
			}
		}

		private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
		{
			if (!this.Visible)
			{
				this.Show();
			}
			else if (Restored != null)
			{
				Restored(this, e);
			}
			else
			{
				this.BringToFront();
			}
		}
	}
}