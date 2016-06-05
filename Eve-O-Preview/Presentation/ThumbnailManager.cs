using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Threading;
using EveOPreview.Configuration;

namespace EveOPreview.UI
{
	public class ThumbnailManager : IThumbnailManager
	{
		#region Private constants
		private const string ClientProcessName = "ExeFile";
		private const string DefaultThumbnailTitle = "...";
		#endregion

		#region Private fields
		private readonly IApplicationConfiguration _configuration;
		private readonly IConfigurationStorage _configurationStorage;
		private readonly DispatcherTimer _thumbnailUpdateTimer;
		private readonly IThumbnailViewFactory _thumbnailViewFactory;
		private readonly Dictionary<IntPtr, IThumbnailView> _thumbnailViews;

		private IntPtr _activeClientHandle;
		private string _activeClientTitle;

		private bool _ignoreViewEvents;
		private bool _isHoverEffectActive;
		private Size _thumbnailBaseSize;
		private Point _thumbnailBaseLocation;
		#endregion

		public ThumbnailManager(IApplicationConfiguration configuration, IConfigurationStorage configurationStorage, IThumbnailViewFactory factory)
		{
			this._configuration = configuration;
			this._configurationStorage = configurationStorage;
			this._thumbnailViewFactory = factory;

			this._activeClientHandle = (IntPtr)0;
			this._activeClientTitle = "";

			this.EnableViewEvents();
			this._isHoverEffectActive = false;

			this._thumbnailViews = new Dictionary<IntPtr, IThumbnailView>();

			//  DispatcherTimer setup
			this._thumbnailUpdateTimer = new DispatcherTimer();
			this._thumbnailUpdateTimer.Tick += ThumbnailUpdateTimerTick;
			this._thumbnailUpdateTimer.Interval = new TimeSpan(0, 0, 0, 0, configuration.ThumbnailRefreshPeriod);
		}

		public event Action<IList<IThumbnailView>> ThumbnailsAdded;
		public event Action<IList<IThumbnailView>> ThumbnailsUpdated;
		public event Action<IList<IThumbnailView>> ThumbnailsRemoved;
		public event Action<Size> ThumbnailSizeChanged;

		public void Activate()
		{
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
			this.DisableViewEvents();

			foreach (KeyValuePair<IntPtr, IThumbnailView> entry in this._thumbnailViews)
			{
				entry.Value.Size = size;
				entry.Value.Refresh();
			}

			this.ThumbnailSizeChanged?.Invoke(size);

			this.EnableViewEvents();
		}

		public void RefreshThumbnails()
		{
			IntPtr foregroundWindowHandle = DwmApiNativeMethods.GetForegroundWindow();
			Boolean hideAllThumbnails = (this._configuration.HideThumbnailsOnLostFocus && !this.IsClientWindowActive(foregroundWindowHandle)) || !DwmApiNativeMethods.DwmIsCompositionEnabled();

			this.DisableViewEvents();

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

				if (this._configuration.HideActiveClientThumbnail && (view.Id == this._activeClientHandle))
				{
					if (view.IsActive)
					{
						view.Hide();
					}
					continue;
				}

				view.Location = this._configuration.GetThumbnailLocation(view.Title, this._activeClientTitle, view.Location);
				view.IsOverlayEnabled = this._configuration.ShowThumbnailOverlays;
				if (!this._isHoverEffectActive)
				{
					view.SetOpacity(this._configuration.ThumbnailsOpacity);
				}

				if (!view.IsActive)
				{
					view.Show();
				}
			}

			this.EnableViewEvents();
		}

		public void SetupThumbnailFrames()
		{
			this.DisableViewEvents();

			foreach (KeyValuePair<IntPtr, IThumbnailView> entry in this._thumbnailViews)
			{
				entry.Value.SetWindowFrames(this._configuration.ShowThumbnailFrames);
			}

			this.EnableViewEvents();
		}

		private void ThumbnailUpdateTimerTick(object sender, EventArgs e)
		{
			this.UpdateThumbnailsList();
			this.RefreshThumbnails();
		}

		private void EnableViewEvents()
		{
			this._ignoreViewEvents = false;
		}

		private void DisableViewEvents()
		{
			this._ignoreViewEvents = true;
		}

		private static Process[] GetClientProcesses()
		{
			return Process.GetProcessesByName(ThumbnailManager.ClientProcessName);
		}

		private void UpdateThumbnailsList()
		{
			Process[] clientProcesses = ThumbnailManager.GetClientProcesses();
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
					thumbnailSize.Width = this._configuration.ThumbnailsWidth;
					thumbnailSize.Height = this._configuration.ThumbnailsHeight;

					view = this._thumbnailViewFactory.Create(processHandle, ThumbnailManager.DefaultThumbnailTitle, thumbnailSize);
					view.IsEnabled = true;
					view.IsOverlayEnabled = this._configuration.ShowThumbnailOverlays;
					view.SetTopMost(this._configuration.ShowThumbnailsAlwaysOnTop);
					view.SetWindowFrames(this._configuration.ShowThumbnailFrames);

					view.ThumbnailResized += ThumbnailViewResized;
					view.ThumbnailMoved += ThumbnailViewMoved;
					view.ThumbnailFocused += ThumbnailViewFocused;
					view.ThumbnailLostFocus += ThumbnailViewLostFocus;
					view.ThumbnailActivated += ThumbnailActivated;

					this._thumbnailViews.Add(processHandle, view);

					this.ApplyClientLayout(processHandle, processTitle);

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

					this.ApplyClientLayout(processHandle, processTitle);

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

			view.SetOpacity(this._configuration.ThumbnailsOpacity);

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

			if (this._configuration.EnableClientLayoutTracking)
			{
				this.UpdateClientLayouts();
			}

			this.RefreshThumbnails();

			this._configurationStorage.Save();
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

			this._configuration.SetThumbnailLocation(view.Title, this._activeClientTitle, view.Location);

			view.Refresh();
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

		private void ThumbnailZoomIn(IThumbnailView view)
		{
			int zoomFactor = this._configuration.ThumbnailZoomFactor;

			this._thumbnailBaseSize = view.Size;
			this._thumbnailBaseLocation = view.Location;

			this.DisableViewEvents();

			view.Size = new Size(zoomFactor * view.Size.Width, zoomFactor * view.Size.Height);

			int locationX = view.Location.X;
			int locationY = view.Location.Y;

			int newWidth = view.Size.Width;
			int newHeight = view.Size.Height;

			int oldWidth = this._thumbnailBaseSize.Width;
			int oldHeight = this._thumbnailBaseSize.Height;

			switch (this._configuration.ThumbnailZoomAnchor)
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

			this.EnableViewEvents();
		}

		private void ThumbnailZoomOut(IThumbnailView view)
		{
			this.DisableViewEvents();

			view.Size = this._thumbnailBaseSize;
			view.Location = this._thumbnailBaseLocation;

			view.Refresh();

			this.EnableViewEvents();
		}

		private void ApplyClientLayout(IntPtr clientHandle, string clientTitle)
		{
			ClientLayout clientLayout = this._configuration.GetClientLayout(clientTitle);

			if (clientLayout == null)
			{
				return;
			}

			DwmApiNativeMethods.MoveWindow(clientHandle, clientLayout.X, clientLayout.Y, clientLayout.Width, clientLayout.Height, true);
		}

		private void UpdateClientLayouts()
		{
			Process[] clientProcesses = ThumbnailManager.GetClientProcesses();

			foreach (Process process in clientProcesses)
			{
				RECT rect;
				DwmApiNativeMethods.GetWindowRect(process.MainWindowHandle, out rect);

				int left = Math.Abs(rect.Left);
				int right = Math.Abs(rect.Right);
				int clientWidth = Math.Abs(left - right);

				int top = Math.Abs(rect.Top);
				int bottom = Math.Abs(rect.Bottom);
				int clientHeight = Math.Abs(top - bottom);

				ClientLayout clientLayout = new ClientLayout();
				clientLayout.X = rect.Left;
				clientLayout.Y = rect.Top;
				clientLayout.Width = clientWidth;
				clientLayout.Height = clientHeight;

				this._configuration.SetClientLayout(process.MainWindowTitle, clientLayout);
			}
		}
	}
}