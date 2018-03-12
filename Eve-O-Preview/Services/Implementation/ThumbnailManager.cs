using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Threading;
using EveOPreview.Configuration;
using EveOPreview.Mediator.Messages;
using EveOPreview.View;
using MediatR;

namespace EveOPreview.Services
{
	class ThumbnailManager : IThumbnailManager
	{
		#region Private constants
		private const int WindowPositionThresholdLow = -10_000;
		private const int WindowPositionThresholdHigh = 31_000;
		private const int WindowSizeThreshold = 10;
		private const int ForcedRefreshCycleThreshold = 1;

		private const string DefaultClientTitle = "EVE";
		#endregion

		#region Private fields
		private readonly IMediator _mediator;
		private readonly IProcessMonitor _processMonitor;
		private readonly IWindowManager _windowManager;
		private readonly IThumbnailConfiguration _configuration;
		private readonly DispatcherTimer _thumbnailUpdateTimer;
		private readonly IThumbnailViewFactory _thumbnailViewFactory;
		private readonly Dictionary<IntPtr, IThumbnailView> _thumbnailViews;

		private IntPtr _activeClientHandle;
		private string _activeClientTitle;

		private bool _ignoreViewEvents;
		private bool _isHoverEffectActive;

		private int _refreshCycleCount;
		#endregion

		public ThumbnailManager(IMediator mediator, IThumbnailConfiguration configuration, IProcessMonitor processMonitor, IWindowManager windowManager, IThumbnailViewFactory factory)
		{
			this._mediator = mediator;
			this._processMonitor = processMonitor;
			this._windowManager = windowManager;
			this._configuration = configuration;
			this._thumbnailViewFactory = factory;

			this._activeClientHandle = (IntPtr)0;
			this._activeClientTitle = ThumbnailManager.DefaultClientTitle;

			this.EnableViewEvents();
			this._isHoverEffectActive = false;

			this._refreshCycleCount = 0;

			this._thumbnailViews = new Dictionary<IntPtr, IThumbnailView>();

			//  DispatcherTimer setup
			this._thumbnailUpdateTimer = new DispatcherTimer();
			this._thumbnailUpdateTimer.Tick += ThumbnailUpdateTimerTick;
			this._thumbnailUpdateTimer.Interval = new TimeSpan(0, 0, 0, 0, configuration.ThumbnailRefreshPeriod);
		}

		public void Start()
		{
			this._thumbnailUpdateTimer.Start();

			this.RefreshThumbnails();
		}

		public void Stop()
		{
			this._thumbnailUpdateTimer.Stop();
		}

		public void UpdateThumbnailsSize()
		{
			this.SetThumbnailsSize(this._configuration.ThumbnailSize);
		}

		private void SetThumbnailsSize(Size size)
		{
			this.DisableViewEvents();

			foreach (KeyValuePair<IntPtr, IThumbnailView> entry in this._thumbnailViews)
			{
				entry.Value.ThumbnailSize = size;
				entry.Value.Refresh(false);
			}

			this.EnableViewEvents();
		}

		private void RefreshThumbnails()
		{
			IntPtr foregroundWindowHandle = this._windowManager.GetForegroundWindowHandle();
			string foregroundWindowTitle = null;

			if (foregroundWindowHandle == this._activeClientHandle)
			{
				foregroundWindowTitle = this._activeClientTitle;
			}
			else if (this._thumbnailViews.TryGetValue(foregroundWindowHandle, out IThumbnailView foregroundView))
			{
				// This code will work only on Alt+Tab switch between clients
				foregroundWindowTitle = foregroundView.Title;
			}

			// No need to minimize EVE clients when switching out to non-EVE window (like thumbnail)
			if (!string.IsNullOrEmpty(foregroundWindowTitle))
			{
				this.SwitchActiveClient(foregroundWindowHandle, foregroundWindowTitle);
			}

			bool hideAllThumbnails = this._configuration.HideThumbnailsOnLostFocus && !(string.IsNullOrEmpty(foregroundWindowTitle) || this.IsClientWindowActive(foregroundWindowHandle));

			bool forceRefresh;
			if (this._refreshCycleCount > ThumbnailManager.ForcedRefreshCycleThreshold)
			{
				this._refreshCycleCount = 0;
				forceRefresh = true;
			}
			else
			{
				this._refreshCycleCount++;
				forceRefresh = false;
			}

			this.DisableViewEvents();

			// Hide, show, resize and move
			foreach (KeyValuePair<IntPtr, IThumbnailView> entry in this._thumbnailViews)
			{
				IThumbnailView view = entry.Value;

				if (hideAllThumbnails || this._configuration.IsThumbnailDisabled(view.Title))
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
					if (this.IsManageableThumbnail(view))
					{
						view.ThumbnailLocation = this._configuration.GetThumbnailLocation(view.Title, this._activeClientTitle, view.ThumbnailLocation);
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
					view.Refresh(forceRefresh);
				}
			}

			this.EnableViewEvents();
		}

		public void UpdateThumbnailFrames()
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

		private async void UpdateThumbnailsList()
		{
			this._processMonitor.GetUpdatedProcesses(out ICollection<IProcessInfo> addedProcesses, out ICollection<IProcessInfo> updatedProcesses, out ICollection<IProcessInfo> removedProcesses);

			List<string> viewsAdded = new List<string>();
			List<string> viewsRemoved = new List<string>();

			foreach (IProcessInfo process in addedProcesses)
			{
				IThumbnailView view = this._thumbnailViewFactory.Create(process.Handle, process.Title, this._configuration.ThumbnailSize);
				view.IsOverlayEnabled = this._configuration.ShowThumbnailOverlays;
				view.SetFrames(this._configuration.ShowThumbnailFrames);
				// Max/Min size limitations should be set AFTER the frames are disabled
				// Otherwise thumbnail window will be unnecessary resized
				view.SetSizeLimitations(this._configuration.ThumbnailMinimumSize, this._configuration.ThumbnailMaximumSize);
				view.SetTopMost(this._configuration.ShowThumbnailsAlwaysOnTop);

				view.ThumbnailLocation = this.IsManageableThumbnail(view)
											? this._configuration.GetThumbnailLocation(view.Title, this._activeClientTitle, view.ThumbnailLocation)
											: this._configuration.GetDefaultThumbnailLocation();

				this._thumbnailViews.Add(view.Id, view);

				view.ThumbnailResized = this.ThumbnailViewResized;
				view.ThumbnailMoved = this.ThumbnailViewMoved;
				view.ThumbnailFocused = this.ThumbnailViewFocused;
				view.ThumbnailLostFocus = this.ThumbnailViewLostFocus;
				view.ThumbnailActivated = this.ThumbnailActivated;
				view.ThumbnailDeactivated = this.ThumbnailDeactivated;

				view.RegisterHotkey(this._configuration.GetClientHotkey(view.Title));

				this.ApplyClientLayout(view.Id, view.Title);

				// TODO Add extension filter here later
				if (view.Title != ThumbnailManager.DefaultClientTitle)
				{
					viewsAdded.Add(view.Title);
				}
			}

			foreach (IProcessInfo process in updatedProcesses)
			{
				this._thumbnailViews.TryGetValue(process.Handle, out IThumbnailView view);

				if (view == null)
				{
					// Something went terribly wrong
					continue;
				}

				if (process.Title != view.Title) // update thumbnail title
				{
					viewsRemoved.Add(view.Title);
					view.Title = process.Title;
					viewsAdded.Add(view.Title);

					view.RegisterHotkey(this._configuration.GetClientHotkey(process.Title));

					this.ApplyClientLayout(view.Id, view.Title);
				}
			}

			foreach (IProcessInfo process in removedProcesses)
			{
				IThumbnailView view = this._thumbnailViews[process.Handle];

				this._thumbnailViews.Remove(view.Id);
				if (view.Title != ThumbnailManager.DefaultClientTitle)
				{
					viewsRemoved.Add(view.Title);
				}

				view.UnregisterHotkey();

				view.ThumbnailResized = null;
				view.ThumbnailMoved = null;
				view.ThumbnailFocused = null;
				view.ThumbnailLostFocus = null;
				view.ThumbnailActivated = null;

				view.Close();
			}

			if ((viewsAdded.Count > 0) || (viewsRemoved.Count > 0))
			{
				await this._mediator.Publish(new ThumbnailListUpdated(viewsAdded, viewsRemoved));
			}
		}

		private void SwitchActiveClient(IntPtr foregroungClientHandle, string foregroundClientTitle)
		{
			// Check if any actions are needed
			if (this._activeClientHandle == foregroungClientHandle)
			{
				return;
			}

			// Minimize the currently active client if needed
			if (this._configuration.MinimizeInactiveClients)
			{
				this._windowManager.MinimizeWindow(this._activeClientHandle, false);
			}

			this._activeClientHandle = foregroungClientHandle;
			this._activeClientTitle = foregroundClientTitle;
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
			// View is always available because this method is actually being called by
			// a view callback
			IThumbnailView view = this._thumbnailViews[id];

			this._windowManager.ActivateWindow(view.Id);
			this.SwitchActiveClient(view.Id, view.Title);
			this.UpdateClientLayouts();
			this.RefreshThumbnails();
			view.Refresh(true);
		}

		private void ThumbnailDeactivated(IntPtr id)
		{
			if (!this._thumbnailViews.TryGetValue(id, out IThumbnailView view))
			{
				return;
			}

			this._windowManager.MinimizeWindow(view.Id, true);
			this.RefreshThumbnails();
		}

		private async void ThumbnailViewResized(IntPtr id)
		{
			if (this._ignoreViewEvents)
			{
				return;
			}

			IThumbnailView view = this._thumbnailViews[id];

			this.SetThumbnailsSize(view.ThumbnailSize);

			view.Refresh(false);

			await this._mediator.Publish(new ThumbnailActiveSizeUpdated(view.ThumbnailSize));
		}

		private async void ThumbnailViewMoved(IntPtr id)
		{
			if (this._ignoreViewEvents)
			{
				return;
			}

			IThumbnailView view = this._thumbnailViews[id];

			if (this.IsManageableThumbnail(view))
			{
				await this._mediator.Publish(new ThumbnailLocationUpdated(view.Title, this._activeClientTitle, view.ThumbnailLocation));
			}

			view.Refresh(false);
		}

		private bool IsClientWindowActive(IntPtr windowHandle)
		{
			if (windowHandle == IntPtr.Zero)
			{
				return false;
			}

			foreach (KeyValuePair<IntPtr, IThumbnailView> entry in this._thumbnailViews)
			{
				IThumbnailView view = entry.Value;

				if (view.IsKnownHandle(windowHandle))
				{
					return true;
				}
			}

			return false;
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

			this._windowManager.MoveWindow(clientHandle, clientLayout.X, clientLayout.Y, clientLayout.Width, clientLayout.Height);
		}

		private void UpdateClientLayouts()
		{
			if (!this._configuration.EnableClientLayoutTracking)
			{
				return;
			}

			foreach (KeyValuePair<IntPtr, IThumbnailView> entry in this._thumbnailViews)
			{
				IThumbnailView view = entry.Value;
				this._windowManager.GetWindowCoordinates(view.Id, out int left, out int top, out int right, out int bottom);

				int width = Math.Abs(right - left);
				int height = Math.Abs(bottom - top);

				if (!this.IsValidWindowPosition(left, top, width, height))
				{
					continue;
				}

				this._configuration.SetClientLayout(view.Title, new ClientLayout(left, top, width, height));
			}
		}

		// We shouldn't manage some thumbnails (like thumbnail of the EVE client sitting on the login screen)
		// TODO Move to a service (?)
		private bool IsManageableThumbnail(IThumbnailView view)
		{
			return view.Title != ThumbnailManager.DefaultClientTitle;
		}

		// Quick sanity check that the window is not minimized
		private bool IsValidWindowPosition(int letf, int top, int width, int height)
		{
			return (letf > ThumbnailManager.WindowPositionThresholdLow) && (letf < ThumbnailManager.WindowPositionThresholdHigh)
					&& (top > ThumbnailManager.WindowPositionThresholdLow) && (top < ThumbnailManager.WindowPositionThresholdHigh)
					&& (width > ThumbnailManager.WindowSizeThreshold) && (height > ThumbnailManager.WindowSizeThreshold);
		}
	}
}