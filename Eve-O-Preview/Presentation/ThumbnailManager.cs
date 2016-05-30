using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Threading;
using System.Xml.Linq;

namespace EveOPreview.UI
{
	public class ThumbnailManager : IThumbnailManager
	{
		private const string ClientProcessName = "ExeFile";
		private const string DefaultThumbnailTitle = "...";

		private readonly DispatcherTimer _thumbnailUpdateTimer;
		private readonly IThumbnailViewFactory _thumbnailViewFactory;
		private readonly Dictionary<IntPtr, IThumbnailView> _thumbnailViews;

		private IntPtr _activeClientHandle;
		private string _activeClientTitle;

		private bool _ignoreViewEvents;
		private bool _isHoverEffectActive;
		private Size _thumbnailBaseSize;
		private Point _thumbnailBaseLocation;

		// TODO To be moved into a separate class
		private readonly Dictionary<string, Dictionary<string, Point>> _uniqueLayouts;
		private readonly Dictionary<string, Point> _flatLayout;
		private readonly Dictionary<string, string> _flatLayoutShortcuts;
		private readonly Dictionary<string, WindowProperties> _clientLayout;

		// TODO To be removed
		private readonly Dictionary<string, string> _xmlBadToOkChars;

		// TODO Drop dependency on the configuration object
		public ThumbnailManager(IThumbnailViewFactory factory)
		{
			this._thumbnailViewFactory = factory;

			this._activeClientHandle = (IntPtr)0;
			this._activeClientTitle = "";

			this._ignoreViewEvents = false;
			this._isHoverEffectActive = false;

			this._thumbnailViews = new Dictionary<IntPtr, IThumbnailView>();

			//  DispatcherTimer setup
			this._thumbnailUpdateTimer = new DispatcherTimer();
			this._thumbnailUpdateTimer.Tick += ThumbnailUpdateTimerTick;
			this._thumbnailUpdateTimer.Interval = new TimeSpan(0, 0, 0, 0, 500); // TODO Make it configurable

			// TODO Move mayouts stuff out
			_uniqueLayouts = new Dictionary<string, Dictionary<string, Point>>();
			_flatLayout = new Dictionary<string, Point>();
			_flatLayoutShortcuts = new Dictionary<string, string>();
			_clientLayout = new Dictionary<string, WindowProperties>();

			// TODO To be removed
			_xmlBadToOkChars = new Dictionary<string, string>();
			_xmlBadToOkChars["<"] = "---lt---";
			_xmlBadToOkChars["&"] = "---amp---";
			_xmlBadToOkChars[">"] = "---gt---";
			_xmlBadToOkChars["\""] = "---quot---";
			_xmlBadToOkChars["\'"] = "---apos---";
			_xmlBadToOkChars[","] = "---comma---";
			_xmlBadToOkChars["."] = "---dot---";
		}

		public event Action<IList<IThumbnailView>> ThumbnailsAdded;
		public event Action<IList<IThumbnailView>> ThumbnailsUpdated;
		public event Action<IList<IThumbnailView>> ThumbnailsRemoved;
		public event Action<Size> ThumbnailSizeChanged;

		public void Activate()
		{
			this.LoadLayout();

			this._thumbnailUpdateTimer.Start();

			this.RefreshThumbnails();
		}

		public void Deactivate()
		{
			this._thumbnailUpdateTimer.Stop();
		}

		public void SetThumbnailState(IntPtr thumbnailId, bool hideAlways)
		{
			IThumbnailView thumbnail;
			if (!this._thumbnailViews.TryGetValue(thumbnailId, out thumbnail))
			{
				return;
			}

			thumbnail.IsEnabled = !hideAlways;
		}

		public void SetThumbnailsSize(Size size)
		{
			this._ignoreViewEvents = true;

			foreach (KeyValuePair<IntPtr, IThumbnailView> entry in this._thumbnailViews)
			{
				entry.Value.Size = size;
				entry.Value.Refresh();
			}

			this.ThumbnailSizeChanged?.Invoke(size);

			this._ignoreViewEvents = false;
		}

		// TODO Drop dependency on the configuration object
		public void RefreshThumbnails()
		{
			IntPtr foregroundWindowHandle = DwmApiNativeMethods.GetForegroundWindow();
			Boolean hideAllThumbnails = (Properties.Settings.Default.hide_all && !this.IsClientWindowActive(foregroundWindowHandle)) || !DwmApiNativeMethods.DwmIsCompositionEnabled();

			this._ignoreViewEvents = true;

			// Hide, show, resize and move
			foreach (KeyValuePair<IntPtr, IThumbnailView> entry in this._thumbnailViews)
			{
				IThumbnailView view = entry.Value;

				if (hideAllThumbnails || !view.IsEnabled)
				{
					if (view.IsActive)
					{
						view.Hide();
					}
					continue;
				}

				if (view.Id == this._activeClientHandle && Properties.Settings.Default.hide_active)
				{
					if (view.IsActive)
					{
						view.Hide();
					}
					continue;
				}

				if (Properties.Settings.Default.unique_layout)
				{
					this.ApplyPerClientLayout(view, this._activeClientTitle);
				}
				else
				{
					this.ApplyFlatLayout(view);
				}

				view.IsOverlayEnabled = Properties.Settings.Default.show_overlay;
				if (!this._isHoverEffectActive)
				{
					view.SetOpacity(Properties.Settings.Default.opacity);
				}

				if (!view.IsActive)
				{
					view.Show();
				}
			}

			this._ignoreViewEvents = false;
		}

		public void SetupThumbnailFrames()
		{
			// TODO Drop config dependency
			this._ignoreViewEvents = true;

			foreach (KeyValuePair<IntPtr, IThumbnailView> entry in this._thumbnailViews)
			{
				entry.Value.SetWindowFrames(Properties.Settings.Default.show_thumb_frames);
			}

			this._ignoreViewEvents = false;
		}

		private void ThumbnailUpdateTimerTick(object sender, EventArgs e)
		{
			this.UpdateThumbnailsList();
			this.RefreshThumbnails();
		}

		private Process[] GetClientProcesses()
		{
			return Process.GetProcessesByName(ThumbnailManager.ClientProcessName);
		}

		private void UpdateThumbnailsList()
		{
			Process[] clientProcesses = this.GetClientProcesses();
			List<IntPtr> processHandles = new List<IntPtr>(clientProcesses.Length);

			IntPtr foregroundWindowHandle = DwmApiNativeMethods.GetForegroundWindow();

			List<IThumbnailView> viewsAdded = new List<IThumbnailView>();
			List<IThumbnailView> viewsUpdated = new List<IThumbnailView>();
			List<IThumbnailView> viewsRemoved = new List<IThumbnailView>();

			foreach (Process process in clientProcesses)
			{
				IntPtr processHandle = process.MainWindowHandle;
				string processTitle = process.MainWindowTitle;
				processHandles.Add(processHandle);

				IThumbnailView view;
				this._thumbnailViews.TryGetValue(processHandle, out view);

				if ((view == null) && (processTitle != ""))
				{
					Size thumbnailSize = new Size();
					thumbnailSize.Width = (int)Properties.Settings.Default.sync_resize_x;
					thumbnailSize.Height = (int)Properties.Settings.Default.sync_resize_y;

					view = this._thumbnailViewFactory.Create(processHandle, ThumbnailManager.DefaultThumbnailTitle, thumbnailSize);
					view.IsEnabled = true;
					view.IsOverlayEnabled = Properties.Settings.Default.show_overlay;
					view.SetTopMost(Properties.Settings.Default.always_on_top);
					view.SetWindowFrames(Properties.Settings.Default.show_thumb_frames);

					view.ThumbnailResized += ThumbnailViewResized;
					view.ThumbnailMoved += ThumbnailViewMoved;
					view.ThumbnailFocused += ThumbnailViewFocused;
					view.ThumbnailLostFocus += ThumbnailViewLostFocus;
					view.ThumbnailActivated += ThumbnailActivated;

					this._thumbnailViews.Add(processHandle, view);

					this.SetupClientWindow(processHandle, processTitle);

					viewsAdded.Add(view);
				}
				else if ((view != null) && (processTitle != view.Title)) // update thumbnail title
				{
					view.Title = processTitle;

					// TODO Shortcuts should be handled at manager level
					//string value;
					//if (_flatLayoutShortcuts.TryGetValue(processTitle, out value))
					//{
					//	view.RegisterShortcut(value); 
					//}

					this.SetupClientWindow(processHandle, processTitle);

					viewsUpdated.Add(view);
				}

				if (process.MainWindowHandle == foregroundWindowHandle)
				{
					this._activeClientHandle = process.MainWindowHandle;
					this._activeClientTitle = process.MainWindowTitle;
				}
			}

			// Cleanup
			IList<IntPtr> obsoleteThumbnails = new List<IntPtr>();

			foreach (IntPtr processHandle in _thumbnailViews.Keys)
			{
				if (!processHandles.Contains(processHandle))
				{
					obsoleteThumbnails.Add(processHandle);
				}
			}

			foreach (IntPtr processHandle in obsoleteThumbnails)
			{
				IThumbnailView view = this._thumbnailViews[processHandle];

				_thumbnailViews.Remove(processHandle);

				// TODO Remove hotkey here
				view.ThumbnailResized -= ThumbnailViewResized;
				view.ThumbnailMoved -= ThumbnailViewMoved;
				view.ThumbnailFocused -= ThumbnailViewFocused;
				view.ThumbnailLostFocus -= ThumbnailViewLostFocus;
				view.ThumbnailActivated -= ThumbnailActivated;

				view.Close();

				viewsRemoved.Add(view);
			}

			this.ThumbnailsAdded?.Invoke(viewsAdded);
			this.ThumbnailsUpdated?.Invoke(viewsUpdated);
			this.ThumbnailsRemoved?.Invoke(viewsRemoved);
		}

		private void ThumbnailViewFocused(IntPtr id)
		{
			if (this._isHoverEffectActive)
			{
				return;
			}

			this._isHoverEffectActive = true;

			IThumbnailView view = this._thumbnailViews[id];

			this.ThumbnailZoomIn(view);
			view.SetTopMost(true);

			view.SetOpacity(1.0);
		}

		private void ThumbnailViewLostFocus(IntPtr id)
		{
			if (!this._isHoverEffectActive)
			{
				return;
			}

			IThumbnailView view = this._thumbnailViews[id];

			this.ThumbnailZoomOut(view);

			view.SetOpacity(Properties.Settings.Default.opacity);

			this._isHoverEffectActive = false;
		}

		private void ThumbnailActivated(IntPtr id)
		{
			DwmApiNativeMethods.SetForegroundWindow(id);

			int style = DwmApiNativeMethods.GetWindowLong(id, DwmApiNativeMethods.GWL_STYLE);
			if ((style & DwmApiNativeMethods.WS_MINIMIZE) == DwmApiNativeMethods.WS_MINIMIZE)
			{
				DwmApiNativeMethods.ShowWindowAsync(id, DwmApiNativeMethods.SW_SHOWNORMAL);
			}

			this.UpdateClientLocation();

			this.SaveLayout(); // Stores info about client window locations

			foreach (KeyValuePair<IntPtr, IThumbnailView> entry in this._thumbnailViews)
			{
				IThumbnailView view = entry.Value;
				view.SetTopMost(Properties.Settings.Default.always_on_top);
			}
		}

		private void ThumbnailViewResized(IntPtr id)
		{
			if (this._ignoreViewEvents)
			{
				return;
			}

			IThumbnailView view = this._thumbnailViews[id];

			this.SetThumbnailsSize(view.Size);

			view.Refresh();
		}

		private void ThumbnailViewMoved(IntPtr id)
		{
			if (this._ignoreViewEvents)
			{
				return;
			}

			IThumbnailView view = this._thumbnailViews[id];

			this.UpdateThumbnailPosition(view.Title, view.Location);

			view.Refresh();
		}

		private void SetupClientWindow(IntPtr clientHandle, string clientTitle)
		{
			if (!Properties.Settings.Default.track_client_windows)
			{
				return;
			}

			WindowProperties windowProperties;
			if (!this._clientLayout.TryGetValue(clientTitle, out windowProperties))
			{
				return;
			}

			DwmApiNativeMethods.MoveWindow(clientHandle, windowProperties.X, windowProperties.Y, windowProperties.Width, windowProperties.Height, true);
		}

		private bool IsClientWindowActive(IntPtr windowHandle)
		{
			foreach (KeyValuePair<IntPtr, IThumbnailView> entry in _thumbnailViews)
			{
				if (entry.Value.IsKnownHandle(windowHandle))
				{
					return true;
				}
			}

			return false;
		}

		private void UpdateThumbnailPosition(string title, Point position)
		{
			if (Properties.Settings.Default.unique_layout)
			{
				Dictionary<string, Point> layout;
				if (_uniqueLayouts.TryGetValue(_activeClientTitle, out layout))
				{
					layout[title] = position;
				}
				else if (_activeClientTitle == "")
				{
					_uniqueLayouts[_activeClientTitle] = new Dictionary<string, Point>();
					_uniqueLayouts[_activeClientTitle][title] = position;
				}
			}
			else
			{
				_flatLayout[title] = position;
			}
		}

		private void ThumbnailZoomIn(IThumbnailView view)
		{
			// TODO Use global settings object
			float zoomFactor = Properties.Settings.Default.zoom_amount;

			this._thumbnailBaseSize = view.Size;
			this._thumbnailBaseLocation = view.Location;

			this._ignoreViewEvents = true;
			view.Size = new Size((int)(zoomFactor * view.Size.Width), (int)(zoomFactor * view.Size.Height));

			int locationX = view.Location.X;
			int locationY = view.Location.Y;

			int newWidth = view.Size.Width;
			int newHeight = view.Size.Height;

			int oldWidth = this._thumbnailBaseSize.Width;
			int oldHeight = this._thumbnailBaseSize.Height;

			// TODO Use global settings object
			switch ((ZoomAnchor)Properties.Settings.Default.zoom_anchor)
			{
				case ZoomAnchor.NW:
					break;
				case ZoomAnchor.N:
					view.Location = new Point(locationX - newWidth / 2 + oldWidth / 2, locationY);
					break;
				case ZoomAnchor.NE:
					view.Location = new Point(locationX - newWidth + oldWidth, locationY);
					break;

				case ZoomAnchor.W:
					view.Location = new Point(locationX, locationY - newHeight / 2 + oldHeight / 2);
					break;
				case ZoomAnchor.C:
					view.Location = new Point(locationX - newWidth / 2 + oldWidth / 2, locationY - newHeight / 2 + oldHeight / 2);
					break;
				case ZoomAnchor.E:
					view.Location = new Point(locationX - newWidth + oldWidth, locationY - newHeight / 2 + oldHeight / 2);
					break;

				case ZoomAnchor.SW:
					view.Location = new Point(locationX, locationY - newHeight + this._thumbnailBaseSize.Height);
					break;
				case ZoomAnchor.S:
					view.Location = new Point(locationX - newWidth / 2 + oldWidth / 2, locationY - newHeight + oldHeight);
					break;
				case ZoomAnchor.SE:
					view.Location = new Point(locationX - newWidth + oldWidth, locationY - newHeight + oldHeight);
					break;
			}

			view.Refresh();

			this._ignoreViewEvents = false;
		}

		private void ThumbnailZoomOut(IThumbnailView view)
		{
			this._ignoreViewEvents = true;

			view.Size = this._thumbnailBaseSize;
			view.Location = this._thumbnailBaseLocation;

			view.Refresh();

			this._ignoreViewEvents = false;
		}

		// ************************************************************************
		// ************************************************************************
		// ************************************************************************
		// ************************************************************************
		// ************************************************************************
		// ************************************************************************































		// TODO Reenable this method
		private void UpdateClientLocation()
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

				WindowProperties location = new WindowProperties();
				location.X = rect.Left;
				location.Y = rect.Top;
				location.Width = client_width;
				location.Height = client_height;


				_clientLayout[process.MainWindowTitle] = location;
			}
		}

		// TODO Layouting and stuff should be renewed later
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

		private void LoadLayout()
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
					WindowProperties location = new WindowProperties();
					location.X = Convert.ToInt32(el.Element("x").Value);
					location.Y = Convert.ToInt32(el.Element("y").Value);
					location.Width = Convert.ToInt32(el.Element("width").Value);
					location.Height = Convert.ToInt32(el.Element("height").Value);

					_clientLayout[ParseXElement(el)] = location;
				}
			}
		}

		private void SaveLayout()
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

		private void ApplyPerClientLayout(IThumbnailView thumbnailWindow, string last_known_active_window)
		{
			Dictionary<string, Point> layout;
			if (_uniqueLayouts.TryGetValue(last_known_active_window, out layout))
			{
				Point new_loc;
				if (Properties.Settings.Default.unique_layout && layout.TryGetValue(thumbnailWindow.Title, out new_loc))
				{
					thumbnailWindow.Location = new_loc;
				}
				else
				{
					// create inner dict
					layout[thumbnailWindow.Title] = thumbnailWindow.Location;
				}
			}
			else if (last_known_active_window != "")
			{
				// create outer dict
				_uniqueLayouts[last_known_active_window] = new Dictionary<string, Point>();
				_uniqueLayouts[last_known_active_window][thumbnailWindow.Title] = thumbnailWindow.Location;
			}
		}

		private void ApplyFlatLayout(IThumbnailView thumbnailWindow)
		{
			Point layout;
			if (_flatLayout.TryGetValue(thumbnailWindow.Title, out layout))
			{
				thumbnailWindow.Location = layout;
			}
			else if (thumbnailWindow.Title != "")
			{
				_flatLayout[thumbnailWindow.Title] = thumbnailWindow.Location;
			}
		}
	}
}