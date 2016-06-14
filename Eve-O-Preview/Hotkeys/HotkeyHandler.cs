using System;
using System.Windows.Forms;
using System.ComponentModel;

namespace EveOPreview.UI
{
	class HotkeyHandler : IMessageFilter, IDisposable
	{
		private static int _currentId;
		private const int MaxId = 0xBFFF;

		#region Private fields
		private readonly int _hotkeyId;
		private readonly IntPtr _hotkeyTarget;
		#endregion

		public HotkeyHandler(IntPtr target, Keys hotkey)
		{
			this._hotkeyId = HotkeyHandler._currentId;
			HotkeyHandler._currentId = (HotkeyHandler._currentId + 1) & HotkeyHandler.MaxId;

			this._hotkeyTarget = target;

			// Assign properties
			this.IsRegistered = false;

			this.KeyCode = hotkey;
		}

		public void Dispose()
		{
			if (this.IsRegistered)
			{
				this.Unregister();
			}

			GC.SuppressFinalize(this);
		}

		~HotkeyHandler()
		{
			// Unregister the hotkey if necessary
			if (this.IsRegistered)
			{
				try
				{
					this.Unregister();
				}
				catch (Exception)
				{
					// Please no exceptions in the finalizer thread
				}
			}
		}

		public bool IsRegistered { get; private set; }

		public Keys KeyCode { get; private set; }

		public event HandledEventHandler Pressed;

		public bool CanRegister()
		{
			// Any exception means "no, you can't register"
			try
			{
				// Attempt to register
				if (this.Register())
				{
					// Unregister and say we managed it
					this.Unregister();
					return true;
				}
			}
			catch (Win32Exception)
			{
			}
			catch (NotSupportedException)
			{
			}

			return false;
		}

		public bool Register()
		{
			// Check that we have not registered
			if (this.IsRegistered)
			{
				throw new NotSupportedException("This hotkey is already registered");
			}

			if (this.KeyCode == Keys.None)
			{
				throw new NotSupportedException("Cannot register an empty hotkey");
			}

			// Remove all modifiers from the 'main' hotkey
			uint key = (uint)this.KeyCode & (~(uint)Keys.Alt) & (~(uint)Keys.Control) & (~(uint)Keys.Shift);

			// Get unmanaged version of the modifiers code
			uint modifiers = (this.KeyCode.HasFlag(Keys.Alt) ? HotkeyHandlerNativeMethods.MOD_ALT : 0)
							 | (this.KeyCode.HasFlag(Keys.Control) ? HotkeyHandlerNativeMethods.MOD_CONTROL : 0)
							 | (this.KeyCode.HasFlag(Keys.Shift) ? HotkeyHandlerNativeMethods.MOD_SHIFT : 0);

			// Register the hotkey
			if (!HotkeyHandlerNativeMethods.RegisterHotKey(this._hotkeyTarget, this._hotkeyId, modifiers, key))
			{
				return false;
			}

			Application.AddMessageFilter(this);

			this.IsRegistered = true;

			// We successfully registered
			return true;
		}

		public void Unregister()
		{
			// Check that we have registered
			if (!this.IsRegistered)
			{
				throw new NotSupportedException("This hotkey was not registered");
			}

			Application.RemoveMessageFilter(this);

			// Clean up after ourselves
			if (!HotkeyHandlerNativeMethods.UnregisterHotKey(this._hotkeyTarget, this._hotkeyId))
			{
				throw new Win32Exception();
			}

			this.IsRegistered = false;
		}

		#region IMessageFilter
		public bool PreFilterMessage(ref Message message)
		{
			return this.IsRegistered
					&& (message.Msg == HotkeyHandlerNativeMethods.WM_HOTKEY)
					&& (message.WParam.ToInt32() == this._hotkeyId)
					&& this.OnPressed();
		}
		#endregion

		private bool OnPressed()
		{
			// Fire the event if we can
			HandledEventArgs handledEventArgs = new HandledEventArgs(false);
			this.Pressed?.Invoke(this, handledEventArgs);

			// Return whether we handled the event or not
			return handledEventArgs.Handled;
		}
	}
}