using System;
using EveOPreview.DwmInterop;

namespace EveOPreview.DwmAPI
{
	class DwmThumbnail : IDwmThumbnail
	{
		#region Private fields
		private readonly IWindowManager _windowManager;
		private IntPtr _handle;
		private DWM_THUMBNAIL_PROPERTIES _properties;
		#endregion

		public DwmThumbnail(IWindowManager windowManager)
		{
			this._windowManager = windowManager;
		}

		public void Register(IntPtr destination, IntPtr source)
		{
			this._properties = new DWM_THUMBNAIL_PROPERTIES();
			this._properties.dwFlags = DWM_TNP_CONSTANTS.DWM_TNP_VISIBLE
									  + DWM_TNP_CONSTANTS.DWM_TNP_OPACITY
									  + DWM_TNP_CONSTANTS.DWM_TNP_RECTDESTINATION
									  + DWM_TNP_CONSTANTS.DWM_TNP_SOURCECLIENTAREAONLY;
			this._properties.opacity = 255;
			this._properties.fVisible = true;
			this._properties.fSourceClientAreaOnly = true;

			if (!this._windowManager.IsCompositionEnabled)
			{
				return;
			}

			this._handle = DwmApiNativeMethods.DwmRegisterThumbnail(destination, source);
		}

		public void Unregister()
		{
			if (!this._windowManager.IsCompositionEnabled)
			{
				return;
			}

			try
			{
				DwmApiNativeMethods.DwmUnregisterThumbnail(this._handle);
			}
			catch (ArgumentException)
			{
			}
		}

		public void Move(int left, int top, int right, int bottom)
		{
			this._properties.rcDestination = new RECT(left, top, right, bottom);
		}

		public void Update()
		{
			if (!this._windowManager.IsCompositionEnabled)
			{
				return;
			}

			try
			{
				DwmApiNativeMethods.DwmUpdateThumbnailProperties(this._handle, this._properties);
			}
			catch (ArgumentException)
			{
				//This exception will be thrown if the EVE client disappears while this method is running
			}
		}
	}
}
