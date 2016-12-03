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
		private const string DefaultClientTitle = "EVE";
		#endregion

		#region Private fields
		private readonly IThumbnailConfig _configuration;
		private readonly DispatcherTimer _thumbnailUpdateTimer;
		private readonly IThumbnailViewFactory _thumbnailViewFactory;
		private readonly Dictionary<IntPtr, IThumbnailView> _thumbnailViews;

		private IntPtr _activeClientHandle;
		private string _activeClientTitle;

		private bool _ignoreViewEvents;
		private bool _isHoverEffectActive;
		#endregion

		public ThumbnailManager(IThumbnailConfig configuration, IThumbnailViewFactory factory)
		{
			this._configuration = configuration;
			this._thumbnailViewFactory = factory;

			this._activeClientHandle = (IntPtr)0;
			this._activeClientTitle = ThumbnailManager.DefaultClientTitle;

			this.EnableViewEvents();
			this._isHoverEffectActive = false;

			this._thumbnailViews = new Dictionary<IntPtr, IThumbnailView>();

			//  DispatcherTimer setup
			this._thumbnailUpdateTimer = new DispatcherTimer();
			this._thumbnailUpdateTimer.Tick += ThumbnailUpdateTimerTick;
			this._thumbnailUpdateTimer.Interval = new TimeSpan(0, 0, 0, 0, configuration.ThumbnailRefreshPeriod);
		}

		public Action<IList<IThumbnailView>> ThumbnailsAdded { get; set; }

		public Action<IList<IThumbnailView>> ThumbnailsUpdated { get; set; }

		public Action<IList<IThumbnailView>> ThumbnailsRemoved { get; set; }

		public Action<Size> ThumbnailSizeChanged { get; set; }

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

		public void SetThumbnailsSize(IntPtr id, Size size)
		{
			this.DisableViewEvents();

            IThumbnailView view;

            if(this._thumbnailViews.TryGetValue(id, out view))
            {
                view.ThumbnailSize = size;
                view.Refresh(false);
            }

			this.ThumbnailSizeChanged?.Invoke(size);

			this.EnableViewEvents();
		}

		public void SetThumbnailResizeLock(bool locked)
		{
			foreach(IThumbnailView view in this._thumbnailViews.Values)
			{
				view.SetResizeLocked(locked);
			}
		}

		public void SetThumbnailPositionLock(bool locked)
		{
			foreach (IThumbnailView view in this._thumbnailViews.Values)
			{
				view.SetPositionLocked(locked);
			}
		}

		private void RefreshThumbnails()
		{
			IntPtr foregroundWindowHandle = WindowManagerNativeMethods.GetForegroundWindow();
			Boolean hideAllThumbnails = (this._configuration.HideThumbnailsOnLostFocus && this.IsNonClientWindowActive(foregroundWindowHandle)) || !WindowManagerNativeMethods.DwmIsCompositionEnabled();

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

				// No need to update Thumbnails while one of them is highlighted
				if (!this._isHoverEffectActive)
				{
					// Do not even move thumbnails with default caption
					if (view.Title != ThumbnailManager.DefaultClientTitle)
					{
						view.ThumbnailLocation = this._configuration.GetThumbnailLocation(view.Title, this._activeClientTitle, view.ThumbnailLocation);
                        view.ThumbnailSize = this._configuration.SyncThumbnailSizes ? this._configuration.ThumbnailDefaultSize : this._configuration.GetThumbnailSize(view.Title, this._activeClientTitle, view.ThumbnailSize);
					}

					view.SetOpacity(this._configuration.ThumbnailOpacity);
					view.SetTopMost(this._configuration.ShowThumbnailsAlwaysOnTop);
				}

				view.IsOverlayEnabled = this._configuration.ShowThumbnailOverlays;

				view.SetHighlight(this._configuration.EnableActiveClientHighlight && (view.Id == this._activeClientHandle),
										this._configuration.ActiveClientHighlightColor, this._configuration.ActiveClientHighlightThickness);

				if (!view.IsActive)
				{
					view.Show();
				}
				else
				{
					view.Refresh(false);
				}
			}

			this.EnableViewEvents();
		}

		public void SetupThumbnailFrames()
		{
			this.DisableViewEvents();

			foreach (KeyValuePair<IntPtr, IThumbnailView> entry in this._thumbnailViews)
			{
				entry.Value.SetFrames(this._configuration.ShowThumbnailFrames);
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

			IntPtr foregroundWindowHandle = WindowManagerNativeMethods.GetForegroundWindow();

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
					view = this._thumbnailViewFactory.Create(processHandle, processTitle, this._configuration.ThumbnailDefaultSize);
					view.IsEnabled = true;
					view.IsOverlayEnabled = this._configuration.ShowThumbnailOverlays;
					view.SetFrames(this._configuration.ShowThumbnailFrames);
					// Max/Min size limitations should be set AFTER the frames are disabled
					// Otherwise thumbnail window will be unnecessary resized
					view.SetSizeLimitations(this._configuration.ThumbnailMinimumSize, this._configuration.ThumbnailMaximumSize);
					view.SetTopMost(this._configuration.ShowThumbnailsAlwaysOnTop);

                    view.ThumbnailLocation = this._configuration.GetThumbnailLocation(processTitle, this._activeClientTitle, view.ThumbnailLocation);
                    view.ThumbnailSize = this._configuration.SyncThumbnailSizes ? this._configuration.ThumbnailDefaultSize : this._configuration.GetThumbnailSize(processTitle, this._activeClientTitle, view.ThumbnailSize);

					this._thumbnailViews.Add(processHandle, view);

					view.ThumbnailResized = this.ThumbnailViewResized;
					view.ThumbnailMoved = this.ThumbnailViewMoved;
					view.ThumbnailFocused = this.ThumbnailViewFocused;
					view.ThumbnailLostFocus = this.ThumbnailViewLostFocus;
					view.ThumbnailActivated = this.ThumbnailActivated;

					view.RegisterHotkey(this._configuration.GetClientHotkey(processTitle));

					this.ApplyClientLayout(processHandle, processTitle);

					viewsAdded.Add(view);
				}
				else if ((view != null) && (processTitle != view.Title)) // update thumbnail title
				{
					view.Title = processTitle;
					view.RegisterHotkey(this._configuration.GetClientHotkey(processTitle));

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

			foreach (IntPtr processHandle in this._thumbnailViews.Keys)
			{
				if (!processHandles.Contains(processHandle))
				{
					obsoleteThumbnails.Add(processHandle);
				}
			}

			foreach (IntPtr processHandle in obsoleteThumbnails)
			{
				IThumbnailView view = this._thumbnailViews[processHandle];

				this._thumbnailViews.Remove(processHandle);

				view.UnregisterHotkey();

				view.ThumbnailResized = null;
				view.ThumbnailMoved = null;
				view.ThumbnailFocused = null;
				view.ThumbnailLostFocus = null;
				view.ThumbnailActivated = null;

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

			view.SetTopMost(true);
			view.SetOpacity(1.0);

			if (this._configuration.ThumbnailZoomEnabled)
			{
				this.ThumbnailZoomIn(view);
			}
		}

		private void ThumbnailViewLostFocus(IntPtr id)
		{
			if (!this._isHoverEffectActive)
			{
				return;
			}

			IThumbnailView view = this._thumbnailViews[id];

			if (this._configuration.ThumbnailZoomEnabled)
			{
				this.ThumbnailZoomOut(view);
			}

			view.SetOpacity(this._configuration.ThumbnailOpacity);

			this._isHoverEffectActive = false;
		}

		private void ThumbnailActivated(IntPtr id)
		{
			IThumbnailView view;
			if (this._thumbnailViews.TryGetValue(id, out view))
			{
				this._activeClientHandle = view.Id;
				this._activeClientTitle = view.Title;
			}

			WindowManagerNativeMethods.SetForegroundWindow(id);

			int style = WindowManagerNativeMethods.GetWindowLong(id, WindowManagerNativeMethods.GWL_STYLE);
			// If the window was minimized then its thumbnail should be reset
			if ((style & WindowManagerNativeMethods.WS_MINIMIZE) == WindowManagerNativeMethods.WS_MINIMIZE)
			{
				WindowManagerNativeMethods.ShowWindowAsync(id, WindowManagerNativeMethods.SW_SHOWNORMAL);
			}

			if (this._configuration.EnableClientLayoutTracking)
			{
				this.UpdateClientLayouts();
			}

			this.RefreshThumbnails();

			view?.Refresh(true);
		}

		private void ThumbnailViewResized(IntPtr id)
		{
			if (this._ignoreViewEvents)
			{
				return;
			}

			IThumbnailView view = this._thumbnailViews[id];

			this.SetThumbnailsSize(id, view.ThumbnailSize);

			if (!this._configuration.SyncThumbnailSizes)
			{
				this._configuration.SetThumbnailSize(view.Title, this._activeClientTitle, view.ThumbnailSize);
			}

			view.Refresh(false);
		}

		private void ThumbnailViewMoved(IntPtr id)
		{
			if (this._ignoreViewEvents)
			{
				return;
			}

			IThumbnailView view = this._thumbnailViews[id];

			this._configuration.SetThumbnailLocation(view.Title, this._activeClientTitle, view.ThumbnailLocation);

			view.Refresh(false);
		}

		private bool IsNonClientWindowActive(IntPtr windowHandle)
		{
			// We just don't know ATM
			if (windowHandle == IntPtr.Zero)
			{
				return false;
			}

			foreach (KeyValuePair<IntPtr, IThumbnailView> entry in this._thumbnailViews)
			{
				IThumbnailView view = entry.Value;

				if (view.IsKnownHandle(windowHandle))
				{
					return false;
				}
			}

			return true;
		}

		private void ThumbnailZoomIn(IThumbnailView view)
		{
			this.DisableViewEvents();

			view.ZoomIn(ViewZoomAnchorConverter.Convert(this._configuration.ThumbnailZoomAnchor), this._configuration.ThumbnailZoomFactor);
			view.Refresh(false);

			this.EnableViewEvents();
		}

		private void ThumbnailZoomOut(IThumbnailView view)
		{
			this.DisableViewEvents();

			view.ZoomOut();
			view.Refresh(false);

			this.EnableViewEvents();
		}

		private void ApplyClientLayout(IntPtr clientHandle, string clientTitle)
		{
			ClientLayout clientLayout = this._configuration.GetClientLayout(clientTitle);

			if (clientLayout == null)
			{
				return;
			}

			WindowManagerNativeMethods.MoveWindow(clientHandle, clientLayout.X, clientLayout.Y, clientLayout.Width, clientLayout.Height, true);
		}

		private void UpdateClientLayouts()
		{
			Process[] clientProcesses = ThumbnailManager.GetClientProcesses();

			foreach (Process process in clientProcesses)
			{
				RECT rect;
				WindowManagerNativeMethods.GetWindowRect(process.MainWindowHandle, out rect);

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