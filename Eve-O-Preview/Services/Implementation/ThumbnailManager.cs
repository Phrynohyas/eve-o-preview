using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
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
		private const int ForcedRefreshCycleThreshold = 2;
		private const int DefaultLocationChangeNotificationDelay = 2;

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

		private (IntPtr Handle, string Title) _activeClient;

		private readonly object _locationChangeNotificationSyncRoot;
		private (IntPtr Handle, string Title, string ActiveClient, Point Location, int Delay) _enqueuedLocationChangeNotification;

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

			this._activeClient = (IntPtr.Zero, ThumbnailManager.DefaultClientTitle);

			this.EnableViewEvents();
			this._isHoverEffectActive = false;

			this._refreshCycleCount = 0;
			this._locationChangeNotificationSyncRoot = new object();
			this._enqueuedLocationChangeNotification = (IntPtr.Zero, null, null, Point.Empty, -1);

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

		private void ThumbnailUpdateTimerTick(object sender, EventArgs e)
		{
			this.UpdateThumbnailsList();
			this.RefreshThumbnails();
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
											? this._configuration.GetThumbnailLocation(view.Title, this._activeClient.Title, view.ThumbnailLocation)
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

		private void RefreshThumbnails()
		{
			// TODO Split this method
			IntPtr foregroundWindowHandle = this._windowManager.GetForegroundWindowHandle();
			string foregroundWindowTitle = null;

			if (foregroundWindowHandle == this._activeClient.Handle)
			{
				foregroundWindowTitle = this._activeClient.Title;
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

			bool hideAllThumbnails = this._configuration.HideThumbnailsOnLostFocus && !this.IsClientWindowActive(foregroundWindowHandle);

			this._refreshCycleCount++;

			bool forceRefresh;
			if (this._refreshCycleCount >= ThumbnailManager.ForcedRefreshCycleThreshold)
			{
				this._refreshCycleCount = 0;
				forceRefresh = true;
			}
			else
			{
				forceRefresh = false;
			}

			this.DisableViewEvents();

			// Snap thumbnail
			// No need to update Thumbnails while one of them is highlighted
			if ((!this._isHoverEffectActive) && this.TryDequeueLocationChange(out var locationChange))
			{
				if ((locationChange.ActiveClient == this._activeClient.Title) && this._thumbnailViews.TryGetValue(locationChange.Handle, out var view))
				{
					this.SnapThumbnailView(view);

					this.RaiseThumbnailLocationUpdatedNotification(view.Title, this._activeClient.Title, view.ThumbnailLocation);
				}
				else
				{
					this.RaiseThumbnailLocationUpdatedNotification(locationChange.Title, locationChange.ActiveClient, locationChange.Location);
				}
			}

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

				if (this._configuration.HideActiveClientThumbnail && (view.Id == this._activeClient.Handle))
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
						view.ThumbnailLocation = this._configuration.GetThumbnailLocation(view.Title, this._activeClient.Title, view.ThumbnailLocation);
					}

					view.SetOpacity(this._configuration.ThumbnailOpacity);
					view.SetTopMost(this._configuration.ShowThumbnailsAlwaysOnTop);
				}

				view.IsOverlayEnabled = this._configuration.ShowThumbnailOverlays;

				view.SetHighlight(this._configuration.EnableActiveClientHighlight && (view.Id == this._activeClient.Handle),
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

		public void UpdateThumbnailFrames()
		{
			this.DisableViewEvents();

			foreach (KeyValuePair<IntPtr, IThumbnailView> entry in this._thumbnailViews)
			{
				entry.Value.SetFrames(this._configuration.ShowThumbnailFrames);
			}

			this.EnableViewEvents();
		}

		private void EnableViewEvents()
		{
			this._ignoreViewEvents = false;
		}

		private void DisableViewEvents()
		{
			this._ignoreViewEvents = true;
		}

		private void SwitchActiveClient(IntPtr foregroungClientHandle, string foregroundClientTitle)
		{
			// Check if any actions are needed
			if (this._activeClient.Handle == foregroungClientHandle)
			{
				return;
			}

			// Minimize the currently active client if needed
			if (this._configuration.MinimizeInactiveClients && !this._configuration.IsPriorityClient(this._activeClient.Title))
			{
				this._windowManager.MinimizeWindow(this._activeClient.Handle, false);
			}

			this._activeClient = (foregroungClientHandle, foregroundClientTitle);
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

			Task.Run(() => this._windowManager.ActivateWindow(view.Id));

			this.UpdateClientLayouts();
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

		private void ThumbnailViewMoved(IntPtr id)
		{
			if (this._ignoreViewEvents)
			{
				return;
			}

			IThumbnailView view = this._thumbnailViews[id];
			view.Refresh(false);
			this.EnqueueLocationChange(view);
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

		private void SnapThumbnailView(IThumbnailView view)
		{
			// Check if this feature is enabled
			if (!this._configuration.EnableThumbnailSnap)
			{
				return;
			}

			// Only borderless thumbnails can be docked
			if (this._configuration.ShowThumbnailFrames)
			{
				return;
			}

			int width = this._configuration.ThumbnailSize.Width;
			int height = this._configuration.ThumbnailSize.Height;

			// TODO Extract method
			int baseX = view.ThumbnailLocation.X;
			int baseY = view.ThumbnailLocation.Y;

			Point[] viewPoints = { new Point(baseX, baseY), new Point(baseX + width, baseY), new Point(baseX, baseY + height), new Point(baseX + width, baseY + height) };

			// TODO Extract constants
			int thresholdX = Math.Max(20, width / 10);
			int thresholdY = Math.Max(20, height / 10);

			foreach (var entry in this._thumbnailViews)
			{
				IThumbnailView testView = entry.Value;

				if (view.Id == testView.Id)
				{
					continue;
				}

				int testX = testView.ThumbnailLocation.X;
				int testY = testView.ThumbnailLocation.Y;

				Point[] testPoints = { new Point(testX, testY), new Point(testX + width, testY), new Point(testX, testY + height), new Point(testX + width, testY + height) };

				var delta = ThumbnailManager.TestViewPoints(viewPoints, testPoints, thresholdX, thresholdY);

				if ((delta.X == 0) && (delta.Y == 0))
				{
					continue;
				}

				view.ThumbnailLocation = new Point(view.ThumbnailLocation.X + delta.X, view.ThumbnailLocation.Y + delta.Y);
				this._configuration.SetThumbnailLocation(view.Title, this._activeClient.Title, view.ThumbnailLocation);
				break;
			}
		}

		private static (int X, int Y) TestViewPoints(Point[] viewPoints, Point[] testPoints, int thresholdX, int thresholdY)
		{
			// Point combinations that we need to check
			// No need to check all 4x4 combinations
			(int ViewOffset, int TestOffset)[] testOffsets =
								{   ( 0, 3 ), ( 0, 2 ), ( 1, 2 ),
									( 0, 1 ), ( 0, 0 ), ( 1, 0 ),
									( 2, 1 ), ( 2, 0 ), ( 3, 0 )};

			foreach (var testOffset in testOffsets)
			{
				Point viewPoint = viewPoints[testOffset.ViewOffset];
				Point testPoint = testPoints[testOffset.TestOffset];

				int deltaX = testPoint.X - viewPoint.X;
				int deltaY = testPoint.Y - viewPoint.Y;

				if ((Math.Abs(deltaX) <= thresholdX) && (Math.Abs(deltaY) <= thresholdY))
				{
					return (deltaX, deltaY);
				}
			}

			return (0, 0);
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
				(int Left, int Top, int Right, int Bottom) position = this._windowManager.GetWindowPosition(view.Id);

				int width = Math.Abs(position.Right - position.Left);
				int height = Math.Abs(position.Bottom - position.Top);

				if (!this.IsValidWindowPosition(position.Left, position.Top, width, height))
				{
					continue;
				}

				this._configuration.SetClientLayout(view.Title, new ClientLayout(position.Left, position.Top, width, height));
			}
		}

		private void EnqueueLocationChange(IThumbnailView view)
		{
			string activeClientTitle = this._activeClient.Title;
			// TODO ??
			this._configuration.SetThumbnailLocation(view.Title, activeClientTitle, view.ThumbnailLocation);

			lock (this._locationChangeNotificationSyncRoot)
			{
				if (this._enqueuedLocationChangeNotification.Handle == IntPtr.Zero)
				{
					this._enqueuedLocationChangeNotification = (view.Id, view.Title, activeClientTitle, view.ThumbnailLocation, ThumbnailManager.DefaultLocationChangeNotificationDelay);
					return;
				}

				// Reset the delay and exit
				if ((this._enqueuedLocationChangeNotification.Handle == view.Id) &&
					(this._enqueuedLocationChangeNotification.ActiveClient == activeClientTitle))
				{
					this._enqueuedLocationChangeNotification.Delay = ThumbnailManager.DefaultLocationChangeNotificationDelay;
					return;
				}

				this.RaiseThumbnailLocationUpdatedNotification(this._enqueuedLocationChangeNotification.Title, activeClientTitle, this._enqueuedLocationChangeNotification.Location);
				this._enqueuedLocationChangeNotification = (view.Id, view.Title, activeClientTitle, view.ThumbnailLocation, ThumbnailManager.DefaultLocationChangeNotificationDelay);
			}
		}

		private bool TryDequeueLocationChange(out (IntPtr Handle, string Title, string ActiveClient, Point Location) change)
		{
			lock (this._locationChangeNotificationSyncRoot)
			{
				change = (IntPtr.Zero, null, null, Point.Empty);

				if (this._enqueuedLocationChangeNotification.Handle == IntPtr.Zero)
				{
					return false;
				}

				this._enqueuedLocationChangeNotification.Delay--;

				if (this._enqueuedLocationChangeNotification.Delay > 0)
				{
					return false;
				}

				change = (this._enqueuedLocationChangeNotification.Handle, this._enqueuedLocationChangeNotification.Title, this._enqueuedLocationChangeNotification.ActiveClient, this._enqueuedLocationChangeNotification.Location);
				this._enqueuedLocationChangeNotification = (IntPtr.Zero, null, null, Point.Empty, -1);

				return true;
			}
		}

		private async void RaiseThumbnailLocationUpdatedNotification(string title, string activeClient, Point location)
		{
			if (string.IsNullOrEmpty(title) || (title == ThumbnailManager.DefaultClientTitle))
			{
				return;
			}

			await this._mediator.Send(new SaveConfiguration());
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