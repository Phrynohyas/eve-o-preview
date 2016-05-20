using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Threading;
using System.Xml.Linq;

namespace EveOPreview
{
	public class ThumbnailManager
	{
		private readonly Stopwatch _ignoringSizeSync;
		private DispatcherTimer _dispatcherTimer;
		private readonly ThumbnailFactory _thumbnailFactory;
		private readonly Dictionary<IntPtr, IThumbnail> _previews;

		private IntPtr _activeClientHandle;
		private string _activeClientTitle;

		private readonly Dictionary<string, Dictionary<string, Point>> _uniqueLayouts;
		private readonly Dictionary<string, Point> _flatLayout;
		private readonly Dictionary<string, string> _flatLayoutShortcuts;
		private readonly Dictionary<string, ClientLocation> _clientLayout;

		private readonly Dictionary<string, string> _xmlBadToOkChars;

		private readonly Action<IList<string>> _addThumbnail;
		private readonly Action<IList<string>> _removeThumbnail;
		private readonly Action<bool> _setAeroStatus;
		private readonly Action<int,int> _sizeChange;

		public ThumbnailManager(Action<IList<string>> addThumbnail, Action<IList<string>> removeThumbnail, Action<bool> setAeroStatus, Action<int,int> sizeChange)
		{
			_addThumbnail = addThumbnail;
			_removeThumbnail = removeThumbnail;
			_setAeroStatus = setAeroStatus;
			_sizeChange = sizeChange;

			_ignoringSizeSync = new Stopwatch();
			_ignoringSizeSync.Start();

			this._activeClientHandle = (IntPtr)0;
			this._activeClientTitle = "";

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

			this._previews = new Dictionary<IntPtr, IThumbnail>();

			//  DispatcherTimer setup
			_dispatcherTimer = new DispatcherTimer();
			_dispatcherTimer.Tick += dispatcherTimer_Tick;
			_dispatcherTimer.Interval = new TimeSpan(0, 0, 1);

			this._thumbnailFactory = new ThumbnailFactory();
		}

		public void Activate()
		{
			this.load_layout();
			this._dispatcherTimer.Start();
		}

		public void Deactivate()
		{
			this._dispatcherTimer.Stop();
		}

		private void spawn_and_kill_previews()
		{
			// TODO Extract this!
			Process[] processes = Process.GetProcessesByName("ExeFile");
			List<IntPtr> processHandles = new List<IntPtr>();
			List<string> addedList=new List<string>();
			List<string> removedList = new List<string>();
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
					addedList.Add(_previews[process.MainWindowHandle].GetLabel());

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

			// TODO Check for empty list
			_addThumbnail(addedList);

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
				removedList.Add(_previews[processHandle].GetLabel());

				_previews[processHandle].CloseThumbnail();
				_previews.Remove(processHandle);
			}

			_removeThumbnail(removedList);
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

		private void dispatcherTimer_Tick(object sender, EventArgs e)
		{
			spawn_and_kill_previews();
			refresh_thumbnails();
			if (_ignoringSizeSync.ElapsedMilliseconds > 500) { _ignoringSizeSync.Stop(); };

			// TODO Do this once in 10 seconds
			_setAeroStatus(DwmApiNativeMethods.DwmIsCompositionEnabled());
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


		public void SyncPreviewSize(Size sync_size)
		{
			if (Properties.Settings.Default.sync_resize &&
				Properties.Settings.Default.show_thumb_frames &&
				_ignoringSizeSync.ElapsedMilliseconds > 500)
			{
				_ignoringSizeSync.Stop();

				_sizeChange(sync_size.Width, sync_size.Height);

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

		public void refresh_thumbnails()
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

		public void set_frames()
		{
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

	}
}