using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Threading;
using EveOPreview.Configuration;
using EveOPreview.Mediator.Messages;
using EveOPreview.UI.Hotkeys;
using EveOPreview.View;
using System.Linq;
using MediatR;
using System.Diagnostics;

namespace EveOPreview.Services
{
	sealed class ThumbnailManager : IThumbnailManager
	{
		#region Private constants
		private const int WINDOW_POSITION_THRESHOLD_LOW = -10_000;
		private const int WINDOW_POSITION_THRESHOLD_HIGH = 31_000;
		private const int WINDOW_SIZE_THRESHOLD = 10;
		private const int FORCED_REFRESH_CYCLE_THRESHOLD = 2;
		private const int DEFAULT_LOCATION_CHANGE_NOTIFICATION_DELAY = 2;

		private const string DEFAULT_CLIENT_TITLE = "EVE";
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
		private IntPtr _externalApplication;

		private readonly object _locationChangeNotificationSyncRoot;
		private (IntPtr Handle, string Title, string ActiveClient, Point Location, int Delay) _enqueuedLocationChangeNotification;

		private bool _ignoreViewEvents;
		private bool _isHoverEffectActive;

		private int _refreshCycleCount;
		private int _hideThumbnailsDelay;

        private HotkeyHandler _nextClientHotkeyHandler;
        private HotkeyHandler _previousClientHotkeyHandler;

        private HotkeyHandler _nextClientHotkeyGroup2Handler;
        private HotkeyHandler _previousClientHotkeyGroup2Handler;
        #endregion

        public ThumbnailManager(IMediator mediator, IThumbnailConfiguration configuration, IProcessMonitor processMonitor, IWindowManager windowManager, IThumbnailViewFactory factory)
		{
			this._mediator = mediator;
			this._processMonitor = processMonitor;
			this._windowManager = windowManager;
			this._configuration = configuration;
			this._thumbnailViewFactory = factory;

			this._activeClient = (IntPtr.Zero, ThumbnailManager.DEFAULT_CLIENT_TITLE);

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

			this._hideThumbnailsDelay = this._configuration.HideThumbnailsDelay;
            RegisterNextClientHotkey(this._configuration.StringToKey(this._configuration.CycleForwardHotkey));
            RegisterNextClientHotkey(this._configuration.StringToKey(this._configuration.CycleForwardHotkey2));
            RegisterPreviousClientHotkey(this._configuration.StringToKey(this._configuration.CycleBackwardHotkey));
            RegisterPreviousClientHotkey(this._configuration.StringToKey(this._configuration.CycleBackwardHotkey2));

            RegisterGroup2NextClientHotkey(this._configuration.StringToKey(this._configuration.CycleGroup2ForwardHotkey));
            RegisterGroup2NextClientHotkey(this._configuration.StringToKey(this._configuration.CycleGroup2ForwardHotkey2));
            RegisterGroup2PreviousClientHotkey(this._configuration.StringToKey(this._configuration.CycleGroup2BackwardHotkey));
            RegisterGroup2PreviousClientHotkey(this._configuration.StringToKey(this._configuration.CycleGroup2BackwardHotkey2));
        }

        public IThumbnailView GetClientByTitle(string title)
        {
            return _thumbnailViews.FirstOrDefault(x => x.Value.Title == title).Value;
        }

        public IThumbnailView GetClientByPointer(IntPtr ptr)
        {
            return _thumbnailViews.FirstOrDefault(x => x.Key == ptr).Value;
        }

        public IThumbnailView GetActiveClient()
        {
            return GetClientByPointer(this._activeClient.Handle);
        }

        public void SetActive(KeyValuePair<IntPtr, IThumbnailView> newClient)
        {
            this.GetActiveClient()?.ClearBorder();

            this._windowManager.ActivateWindow(newClient.Key);
            this.SwitchActiveClient(newClient.Key, newClient.Value.Title);

            newClient.Value.SetHighlight();
            newClient.Value.Refresh(true);
        }

        public void CycleNextClient(bool isForwards)
        {
            IOrderedEnumerable<KeyValuePair<string, string>> clientOrder;
            if (isForwards)
            {
                clientOrder = _configuration.CycleClientsOrder.OrderBy(x => x.Value);
            }
            else
            {
                clientOrder = _configuration.CycleClientsOrder.OrderByDescending(x => x.Value);
            }

            bool setNextClient = false;
            IThumbnailView lastClient = null;

            foreach (var t in clientOrder)
            {
                if (t.Key == _activeClient.Title)
                {
                    setNextClient = true;
                    lastClient = _thumbnailViews.FirstOrDefault(x => x.Value.Title == t.Key).Value;
                    continue;
                }

                if (!setNextClient)
                {
                    continue;
                }

                if (_thumbnailViews.Any(x => x.Value.Title == t.Key))
                {
                    var ptr = _thumbnailViews.First(x => x.Value.Title == t.Key);
                    SetActive(ptr);
                    return;
                }
            }

            // we didn't get a next one. just get the first one from the start.
            foreach (var t in clientOrder)
            {
                if (_thumbnailViews.Any(x => x.Value.Title == t.Key))
                {
                    var ptr = _thumbnailViews.First(x => x.Value.Title == t.Key);
                    SetActive(ptr);
                    return;
                }
            }
        }

        public void CycleGroup2NextClient(bool isForwards)
        {
            IOrderedEnumerable<KeyValuePair<string, string>> clientOrder;
            if (isForwards)
            {
                clientOrder = _configuration.CycleGroup2ClientsOrder.OrderBy(x => x.Value);
            }
            else
            {
                clientOrder = _configuration.CycleGroup2ClientsOrder.OrderByDescending(x => x.Value);
            }

            bool setNextClient = false;
            IThumbnailView lastClient = null;

            foreach (var t in clientOrder)
            {
                if (t.Key == _activeClient.Title)
                {
                    setNextClient = true;
                    lastClient = _thumbnailViews.FirstOrDefault(x => x.Value.Title == t.Key).Value;
                    continue;
                }

                if (!setNextClient)
                {
                    continue;
                }

                if (_thumbnailViews.Any(x => x.Value.Title == t.Key))
                {
                    var ptr = _thumbnailViews.First(x => x.Value.Title == t.Key);
                    SetActive(ptr);
                    return;
                }
            }

            // we didn't get a next one. just get the first one from the start.
            foreach (var t in clientOrder)
            {
                if (_thumbnailViews.Any(x => x.Value.Title == t.Key))
                {
                    var ptr = _thumbnailViews.First(x => x.Value.Title == t.Key);
                    SetActive(ptr);
                    return;
                }
            }
        }

        public void RegisterNextClientHotkey(Keys hotkey)
        {
            if (hotkey == Keys.None)
            {
                return;
            }

            this._nextClientHotkeyHandler = new HotkeyHandler(default(IntPtr), hotkey);
            this._nextClientHotkeyHandler.Pressed += (object s, HandledEventArgs e) =>
            {
                this.CycleNextClient(true);
                e.Handled = true;
            };

            this._nextClientHotkeyHandler.Register();
        }

        public void RegisterPreviousClientHotkey(Keys hotkey)
        {
            if (hotkey == Keys.None)
            {
                return;
            }

            this._previousClientHotkeyHandler = new HotkeyHandler(default(IntPtr), hotkey);
            this._previousClientHotkeyHandler.Pressed += (object s, HandledEventArgs e) =>
            {
                this.CycleNextClient(false);
                e.Handled = true;
            };

            this._previousClientHotkeyHandler.Register();
        }

        public void RegisterGroup2NextClientHotkey(Keys hotkey)
        {
            if (hotkey == Keys.None)
            {
                return;
            }

            this._nextClientHotkeyGroup2Handler = new HotkeyHandler(default(IntPtr), hotkey);
            this._nextClientHotkeyGroup2Handler.Pressed += (object s, HandledEventArgs e) =>
            {
                this.CycleGroup2NextClient(true);
                e.Handled = true;
            };

            this._nextClientHotkeyGroup2Handler.Register();
        }

        public void RegisterGroup2PreviousClientHotkey(Keys hotkey)
        {
            if (hotkey == Keys.None)
            {
                return;
            }

            this._previousClientHotkeyGroup2Handler = new HotkeyHandler(default(IntPtr), hotkey);
            this._previousClientHotkeyGroup2Handler.Pressed += (object s, HandledEventArgs e) =>
            {
                this.CycleGroup2NextClient(false);
                e.Handled = true;
            };

            this._previousClientHotkeyGroup2Handler.Register();
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
				if (view.Title != ThumbnailManager.DEFAULT_CLIENT_TITLE)
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
				if (view.Title != ThumbnailManager.DEFAULT_CLIENT_TITLE)
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

			// The foreground window can be NULL in certain circumstances, such as when a window is losing activation.
			// It is safer to just skip this refresh round than to do something while the system state is undefined
			if (foregroundWindowHandle == IntPtr.Zero)
			{
				return;
			}

			string foregroundWindowTitle = null;

			// Check if the foreground window handle is one of the known handles for client windows or their thumbnails
			bool isClientWindow = this.IsClientWindowActive(foregroundWindowHandle);
			bool isMainWindowActive = this.IsMainWindowActive(foregroundWindowHandle);

			if (foregroundWindowHandle == this._activeClient.Handle)
			{
				foregroundWindowTitle = this._activeClient.Title;
			}
			else if (this._thumbnailViews.TryGetValue(foregroundWindowHandle, out IThumbnailView foregroundView))
			{
				// This code will work only on Alt+Tab switch between clients
				foregroundWindowTitle = foregroundView.Title;
			}
			else if (!isClientWindow)
			{
				this._externalApplication = foregroundWindowHandle;
			}

			// No need to minimize EVE clients when switching out to non-EVE window (like thumbnail)
			if (!string.IsNullOrEmpty(foregroundWindowTitle))
			{
				this.SwitchActiveClient(foregroundWindowHandle, foregroundWindowTitle);
			}

			bool hideAllThumbnails = this._configuration.HideThumbnailsOnLostFocus && !(isClientWindow || isMainWindowActive);

			// Wait for some time before hiding all previews
			if (hideAllThumbnails)
			{
				this._hideThumbnailsDelay--;
				if (this._hideThumbnailsDelay > 0)
				{
					hideAllThumbnails = false; // Postpone the 'hide all' operation
				}
				else
				{
					this._hideThumbnailsDelay = 0; // Stop the counter
				}
			}
			else
			{
				this._hideThumbnailsDelay = this._configuration.HideThumbnailsDelay; // Reset the counter
			}

			this._refreshCycleCount++;

			bool forceRefresh;
			if (this._refreshCycleCount >= ThumbnailManager.FORCED_REFRESH_CYCLE_THRESHOLD)
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

					this.RaiseThumbnailLocationUpdatedNotification(view.Title);
				}
				else
				{
					this.RaiseThumbnailLocationUpdatedNotification(locationChange.Title);
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
							      this._configuration.ActiveClientHighlightThickness);

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

		private void SwitchActiveClient(IntPtr foregroundClientHandle, string foregroundClientTitle)
		{
			// Check if any actions are needed
			if (this._activeClient.Handle == foregroundClientHandle)
			{
				return;
			}

			// Minimize the currently active client if needed
			if (this._configuration.MinimizeInactiveClients && !this._configuration.IsPriorityClient(this._activeClient.Title))
			{
				this._windowManager.MinimizeWindow(this._activeClient.Handle, false);
			}

			this._activeClient = (foregroundClientHandle, foregroundClientTitle);
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
			IThumbnailView view = this._thumbnailViews[id];

			Task.Run(() =>
				{
					this._windowManager.ActivateWindow(view.Id);
				})
				.ContinueWith((task) =>
				{
					// This code should be executed on UI thread
					this.SwitchActiveClient(view.Id, view.Title);
					this.UpdateClientLayouts();
					this.RefreshThumbnails();
				}, TaskScheduler.FromCurrentSynchronizationContext());
		}

		private void ThumbnailDeactivated(IntPtr id, bool switchOut)
		{
			if (switchOut)
			{
				this._windowManager.ActivateWindow(this._externalApplication);
			}
			else
			{
				if (!this._thumbnailViews.TryGetValue(id, out IThumbnailView view))
				{
					return;
				}

				this._windowManager.MinimizeWindow(view.Id, true);
				this.RefreshThumbnails();
			}
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

		// Checks whether currently active window belongs to an EVE client or its thumbnail
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

		// Check whether the currently active window belongs to EVE-O Preview itself
		private bool IsMainWindowActive(IntPtr windowHandle)
		{
			return (this._processMonitor.GetMainProcess().Handle == windowHandle);
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
			if (!this._configuration.EnableClientLayoutTracking)
			{
				return;
			}

			// No need to apply layout for not yet logged-in clients
			if (clientTitle == ThumbnailManager.DEFAULT_CLIENT_TITLE)
			{
				return;
			}

			ClientLayout clientLayout = this._configuration.GetClientLayout(clientTitle);

			if (clientLayout == null)
			{
				return;
			}

			if (clientLayout.IsMaximized)
			{
				this._windowManager.MaximizeWindow(clientHandle);
			}
			else
			{
				this._windowManager.MoveWindow(clientHandle, clientLayout.X, clientLayout.Y, clientLayout.Width, clientLayout.Height);
			}
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

				// No need to save layout for not yet logged-in clients
				if (view.Title == ThumbnailManager.DEFAULT_CLIENT_TITLE)
				{
					continue;
				}

				(int Left, int Top, int Right, int Bottom) position = this._windowManager.GetWindowPosition(view.Id);
				int width = Math.Abs(position.Right - position.Left);
				int height = Math.Abs(position.Bottom - position.Top);

				var isMaximized = this._windowManager.IsWindowMaximized(view.Id);

				if (!(isMaximized || this.IsValidWindowPosition(position.Left, position.Top, width, height)))
				{
					continue;
				}

				this._configuration.SetClientLayout(view.Title, new ClientLayout(position.Left, position.Top, width, height, isMaximized));
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
					this._enqueuedLocationChangeNotification = (view.Id, view.Title, activeClientTitle, view.ThumbnailLocation, ThumbnailManager.DEFAULT_LOCATION_CHANGE_NOTIFICATION_DELAY);
					return;
				}

				// Reset the delay and exit
				if ((this._enqueuedLocationChangeNotification.Handle == view.Id) &&
					(this._enqueuedLocationChangeNotification.ActiveClient == activeClientTitle))
				{
					this._enqueuedLocationChangeNotification.Delay = ThumbnailManager.DEFAULT_LOCATION_CHANGE_NOTIFICATION_DELAY;
					return;
				}

				this.RaiseThumbnailLocationUpdatedNotification(this._enqueuedLocationChangeNotification.Title);
				this._enqueuedLocationChangeNotification = (view.Id, view.Title, activeClientTitle, view.ThumbnailLocation, ThumbnailManager.DEFAULT_LOCATION_CHANGE_NOTIFICATION_DELAY);
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

		private async void RaiseThumbnailLocationUpdatedNotification(string title)
		{
			if (string.IsNullOrEmpty(title) || (title == ThumbnailManager.DEFAULT_CLIENT_TITLE))
			{
				return;
			}

			await this._mediator.Send(new SaveConfiguration());
		}

		// We shouldn't manage some thumbnails (like thumbnail of the EVE client sitting on the login screen)
		// TODO Move to a service (?)
		private bool IsManageableThumbnail(IThumbnailView view)
		{
			return view.Title != ThumbnailManager.DEFAULT_CLIENT_TITLE;
		}

		// Quick sanity check that the window is not minimized
		private bool IsValidWindowPosition(int left, int top, int width, int height)
		{
			return (left > ThumbnailManager.WINDOW_POSITION_THRESHOLD_LOW) && (left < ThumbnailManager.WINDOW_POSITION_THRESHOLD_HIGH)
					&& (top > ThumbnailManager.WINDOW_POSITION_THRESHOLD_LOW) && (top < ThumbnailManager.WINDOW_POSITION_THRESHOLD_HIGH)
					&& (width > ThumbnailManager.WINDOW_SIZE_THRESHOLD) && (height > ThumbnailManager.WINDOW_SIZE_THRESHOLD);
		}
	}
}