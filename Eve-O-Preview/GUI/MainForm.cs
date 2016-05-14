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
		private readonly ThumbnailFactory _thumbnailFactory;

		public event EventHandler Minimized;
		public event EventHandler Maximized;
		public event EventHandler Restored;


		private readonly Dictionary<IntPtr, IThumbnail> _previews;
		private DispatcherTimer _dispatcherTimer;

		private IntPtr _activeClientHandle;
		private string _activeClientTitle;

		private readonly Dictionary<string, Dictionary<string, Point>> _uniqueLayouts;
		private readonly Dictionary<string, Point> _flatLayout;
		private readonly Dictionary<string, string> _flatLayoutShortcuts;
		private readonly Dictionary<string, ClientLocation> _clientLayout;

		private readonly bool _isInitialized;

		private readonly Stopwatch _ignoringSizeSync;

		private readonly Dictionary<string, string> _xmlBadToOkChars;

		private Dictionary<ZoomAnchor, RadioButton> _zoomAnchorButtonMap;

		public MainForm()
		{
			_isInitialized = false;

			this._activeClientHandle = (IntPtr)0;
			this._activeClientTitle = "";

			this._previews = new Dictionary<IntPtr, IThumbnail>();

			_xmlBadToOkChars = new Dictionary<string, string>();
			_xmlBadToOkChars["<"] = "---lt---";
			_xmlBadToOkChars["&"] = "---amp---";
			_xmlBadToOkChars[">"] = "---gt---";
			_xmlBadToOkChars["\""] = "---quot---";
			_xmlBadToOkChars["\'"] = "---apos---";
			_xmlBadToOkChars[","] = "---comma---";
			_xmlBadToOkChars["."] = "---dot---";

			_uniqueLayouts = new Dictionary<string, Dictionary<string, Point>>();
			_flatLayout = new Dictionary<string, Point>();
			_flatLayoutShortcuts = new Dictionary<string, string>();
			_clientLayout = new Dictionary<string, ClientLocation>();

			_ignoringSizeSync = new Stopwatch();
			_ignoringSizeSync.Start();

			InitializeComponent();
			init_options();

			//  DispatcherTimer setup
			_dispatcherTimer = new DispatcherTimer();
			_dispatcherTimer.Tick += dispatcherTimer_Tick;
			_dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
			_dispatcherTimer.Start();

			this._thumbnailFactory = new ThumbnailFactory();

			_isInitialized = true;

			previews_check_listbox.DisplayMember = "Text";

		}


		private void GlassForm_Load(object sender, EventArgs e)
		{
			refresh_thumbnails();
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

			load_layout();
		}


		private void spawn_and_kill_previews()
		{
			if (!_isInitialized) { return; }

			Process[] processes = Process.GetProcessesByName("ExeFile");
			List<IntPtr> processHandles = new List<IntPtr>();

			// pop new previews

			foreach (Process process in processes)
			{
				processHandles.Add(process.MainWindowHandle);

				Size sync_size = new Size();
				sync_size.Width = (int)Properties.Settings.Default.sync_resize_x;
				sync_size.Height = (int)Properties.Settings.Default.sync_resize_y;

				if (!_previews.ContainsKey(process.MainWindowHandle) && process.MainWindowTitle != "")
				{
					_previews[process.MainWindowHandle] = this._thumbnailFactory.Create(this, process.MainWindowHandle, "...", sync_size);

					// apply more thumbnail specific options
					_previews[process.MainWindowHandle].SetTopMost(Properties.Settings.Default.always_on_top);
					_previews[process.MainWindowHandle].SetWindowFrames(Properties.Settings.Default.show_thumb_frames);

					// add a preview also
					previews_check_listbox.BeginUpdate();
					previews_check_listbox.Items.Add(_previews[process.MainWindowHandle]);
					previews_check_listbox.EndUpdate();

					refresh_client_window_locations(process);
				}

				else if (_previews.ContainsKey(process.MainWindowHandle) && process.MainWindowTitle != _previews[process.MainWindowHandle].GetLabel()) //or update the preview titles
				{
					_previews[process.MainWindowHandle].SetLabel(process.MainWindowTitle);
					string key = _previews[process.MainWindowHandle].GetLabel();
					string value;
					if (_flatLayoutShortcuts.TryGetValue(key, out value))
					{
						_previews[process.MainWindowHandle].RegisterShortcut(value);
					}
					refresh_client_window_locations(process);
				}

				if (process.MainWindowHandle == DwmApiNativeMethods.GetForegroundWindow())
				{
					_activeClientHandle = process.MainWindowHandle;
					_activeClientTitle = process.MainWindowTitle;
				}

			}

			// clean up old previews
			List<IntPtr> to_be_pruned = new List<IntPtr>();
			foreach (IntPtr processHandle in _previews.Keys)
			{
				if (!(processHandles.Contains(processHandle)))
				{
					to_be_pruned.Add(processHandle);
				}
			}

			foreach (IntPtr processHandle in to_be_pruned)
			{
				previews_check_listbox.BeginUpdate();
				previews_check_listbox.Items.Remove(_previews[processHandle]);
				previews_check_listbox.EndUpdate();

				_previews[processHandle].CloseThumbnail();
				_previews.Remove(processHandle);
			}

			previews_check_listbox.Update();

		}

		private void refresh_client_window_locations(Process process)
		{
			if (Properties.Settings.Default.track_client_windows && _clientLayout.ContainsKey(process.MainWindowTitle))
			{
				DwmApiNativeMethods.MoveWindow(process.MainWindowHandle, _clientLayout[process.MainWindowTitle].X,
					_clientLayout[process.MainWindowTitle].Y, _clientLayout[process.MainWindowTitle].Width,
					_clientLayout[process.MainWindowTitle].Height, true);
			}
		}


		private string remove_nonconform_xml_characters(string entry)
		{
			foreach (var kv in _xmlBadToOkChars)
			{
				entry = entry.Replace(kv.Key, kv.Value);
			}
			return entry;
		}

		private string restore_nonconform_xml_characters(string entry)
		{
			foreach (var kv in _xmlBadToOkChars)
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
					Dictionary<string, Point> inner = new Dictionary<string, Point>();
					foreach (var inner_el in el.Elements())
					{
						inner[ParseXElement(inner_el)] = new Point(Convert.ToInt32(inner_el.Element("x")?.Value), Convert.ToInt32(inner_el.Element("y")?.Value));
					}
					_uniqueLayouts[ParseXElement(el)] = inner;
				}
			}

			if (File.Exists("flat_layout.xml"))
			{
				XElement rootElement = XElement.Load("flat_layout.xml");
				foreach (var el in rootElement.Elements())
				{
					_flatLayout[ParseXElement(el)] = new Point(Convert.ToInt32(el.Element("x").Value), Convert.ToInt32(el.Element("y").Value));
					_flatLayoutShortcuts[ParseXElement(el)] = "";

					if (el.Element("shortcut") != null)
					{
						_flatLayoutShortcuts[ParseXElement(el)] = el.Element("shortcut").Value;
					}
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

					_clientLayout[ParseXElement(el)] = clientLocation;
				}
			}
		}

		private void store_layout()
		{
			XElement el = new XElement("layouts");
			foreach (var client in _uniqueLayouts.Keys)
			{
				if (client == "")
				{
					continue;
				}
				XElement layout = MakeXElement(client);
				foreach (var thumbnail_ in _uniqueLayouts[client])
				{
					string thumbnail = thumbnail_.Key;
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
			foreach (var clientKV in _flatLayout)
			{
				if (clientKV.Key == "" || clientKV.Key == "...")
				{
					continue;
				}
				XElement layout = MakeXElement(clientKV.Key);
				layout.Add(new XElement("x", clientKV.Value.X));
				layout.Add(new XElement("y", clientKV.Value.Y));

				string shortcut;
				if (_flatLayoutShortcuts.TryGetValue(clientKV.Key, out shortcut))
				{
					layout.Add(new XElement("shortcut", shortcut));
				}
				el2.Add(layout);
			}

			el2.Save("flat_layout.xml");

			XElement el3 = new XElement("client_layout");
			foreach (var clientKV in _clientLayout)
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

		private void handle_unique_layout(IThumbnail thumbnailWindow, string last_known_active_window)
		{
			Dictionary<string, Point> layout;
			if (_uniqueLayouts.TryGetValue(last_known_active_window, out layout))
			{
				Point new_loc;
				if (Properties.Settings.Default.unique_layout && layout.TryGetValue(thumbnailWindow.GetLabel(), out new_loc))
				{
					thumbnailWindow.SetLocation(new_loc);
				}
				else
				{
					// create inner dict
					layout[thumbnailWindow.GetLabel()] = thumbnailWindow.GetLocation();
				}
			}
			else if (last_known_active_window != "")
			{
				// create outer dict
				_uniqueLayouts[last_known_active_window] = new Dictionary<string, Point>();
				_uniqueLayouts[last_known_active_window][thumbnailWindow.GetLabel()] = thumbnailWindow.GetLocation();
			}
		}


		private void update_client_locations()
		{
			Process[] processes = Process.GetProcessesByName("ExeFile");
			List<IntPtr> processHandles = new List<IntPtr>();

			foreach (Process process in processes)
			{
				RECT rect = new RECT();
				DwmApiNativeMethods.GetWindowRect(process.MainWindowHandle, out rect);

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


				_clientLayout[process.MainWindowTitle] = clientLocation;
			}
		}


		public void NotifyPreviewSwitch()
		{
			update_client_locations();
			store_layout(); //todo: check if it actually changed ...
			foreach (KeyValuePair<IntPtr, IThumbnail> entry in _previews)
			{
				entry.Value.SetTopMost(Properties.Settings.Default.always_on_top);
			}
		}


		private void handle_flat_layout(IThumbnail thumbnailWindow)
		{
			Point layout;
			if (_flatLayout.TryGetValue(thumbnailWindow.GetLabel(), out layout))
			{
				thumbnailWindow.SetLocation(layout);
			}
			else if (thumbnailWindow.GetLabel() != "")
			{
				_flatLayout[thumbnailWindow.GetLabel()] = thumbnailWindow.GetLocation();
			}
		}

		private bool window_is_preview_or_client(IntPtr window)
		{
			bool active_window_is_right_type = false;
			foreach (KeyValuePair<IntPtr, IThumbnail> entry in _previews)
			{
				if (entry.Key == window || entry.Value.IsPreviewHandle(window))
				{
					active_window_is_right_type = true;
				}
			}
			return active_window_is_right_type;
		}


		private void refresh_thumbnails()
		{

			IntPtr active_window = DwmApiNativeMethods.GetForegroundWindow();

			// hide, show, resize and move
			foreach (KeyValuePair<IntPtr, IThumbnail> entry in _previews)
			{
				if (!window_is_preview_or_client(active_window) && Properties.Settings.Default.hide_all)
				{
					entry.Value.HideThumbnail();
				}
				else if (entry.Key == _activeClientHandle && Properties.Settings.Default.hide_active)
				{
					entry.Value.HideThumbnail();
				}
				else
				{
					entry.Value.ShowThumbnail();
					if (Properties.Settings.Default.unique_layout)
					{
						handle_unique_layout(entry.Value, _activeClientTitle);
					}
					else
					{
						handle_flat_layout(entry.Value);
					}
				}
				entry.Value.IsZoomEnabled = Properties.Settings.Default.zoom_on_hover;
				entry.Value.IsOverlayEnabled = Properties.Settings.Default.show_overlay;
				entry.Value.SetOpacity(Properties.Settings.Default.opacity);
			}

			DwmApiNativeMethods.DwmIsCompositionEnabled();
		}


		public void SyncPreviewSize(Size sync_size)
		{
			if (!_isInitialized) { return; }

			if (Properties.Settings.Default.sync_resize &&
				Properties.Settings.Default.show_thumb_frames &&
				_ignoringSizeSync.ElapsedMilliseconds > 500)
			{
				_ignoringSizeSync.Stop();

				option_sync_size_x.Text = sync_size.Width.ToString();
				option_sync_size_y.Text = sync_size.Height.ToString();

				foreach (KeyValuePair<IntPtr, IThumbnail> entry in _previews)
				{
					if (entry.Value.IsPreviewHandle(DwmApiNativeMethods.GetForegroundWindow()))
					{
						entry.Value.SetSize(sync_size);
					}
				}

			}

		}


		public void UpdatePreviewPosition(string preview_title, Point position)
		{

			if (Properties.Settings.Default.unique_layout)
			{
				Dictionary<string, Point> layout;
				if (_uniqueLayouts.TryGetValue(_activeClientTitle, out layout))
				{
					layout[preview_title] = position;
				}
				else if (_activeClientTitle == "")
				{
					_uniqueLayouts[_activeClientTitle] = new Dictionary<string, Point>();
					_uniqueLayouts[_activeClientTitle][preview_title] = position;
				}
			}
			else
			{
				_flatLayout[preview_title] = position;
			}

		}


		private void dispatcherTimer_Tick(object sender, EventArgs e)
		{
			spawn_and_kill_previews();
			refresh_thumbnails();
			if (_ignoringSizeSync.ElapsedMilliseconds > 500) { _ignoringSizeSync.Stop(); };

			if (DwmApiNativeMethods.DwmIsCompositionEnabled())
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
			SyncPreviewSize(new Size((int)Properties.Settings.Default.sync_resize_x,
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


		private void option_show_thumbnail_frames_CheckedChanged(object sender, EventArgs e)
		{
			Properties.Settings.Default.show_thumb_frames = option_show_thumbnail_frames.Checked;
			Properties.Settings.Default.Save();

			if (Properties.Settings.Default.show_thumb_frames)
			{
				_ignoringSizeSync.Stop();
				_ignoringSizeSync.Reset();
				_ignoringSizeSync.Start();
			}

			foreach (var thumbnail in _previews)
			{
				thumbnail.Value.SetWindowFrames(Properties.Settings.Default.show_thumb_frames);
			}

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
			Properties.Settings.Default.zoom_on_hover = option_zoom_on_hover.Checked;
			Properties.Settings.Default.Save();
			refresh_thumbnails();
			option_zoom_factor.Enabled = Properties.Settings.Default.zoom_on_hover;
			if (_isInitialized)
			{
				foreach (var kv in _zoomAnchorButtonMap)
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
			refresh_thumbnails();
		}

		private void checkedListBox1_SelectedIndexChanged2(object sender, EventArgs e)
		{
			System.Windows.Forms.ItemCheckEventArgs arg = (System.Windows.Forms.ItemCheckEventArgs)e;
			((ThumbnailWindow)this.previews_check_listbox.Items[arg.Index]).IsPreviewEnabled = (arg.NewValue != System.Windows.Forms.CheckState.Checked);
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


		private void opacity_bar_Scroll(object sender, ScrollEventArgs e)
		{
			// fire off opacity change
			Properties.Settings.Default.opacity = Math.Min((float)e.NewValue / 100.0f, 1.0f);
			Properties.Settings.Default.Save();
			refresh_thumbnails();
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