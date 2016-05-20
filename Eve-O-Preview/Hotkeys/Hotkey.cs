using System;
using System.Windows.Forms;
using System.ComponentModel;
using System.Xml.Serialization;
using System.Runtime.InteropServices;

namespace EveOPreview
{
	public class Hotkey : IMessageFilter
	{
		private static int _currentId;
		private const int MaxId = 0xBFFF;

		[XmlElement("keyCode")]
		private Keys _keyCode;
		[XmlElement("shift")]
		private bool _shift;
		[XmlElement("control")]
		private bool _control;
		[XmlElement("alt")]
		private bool _alt;
		[XmlElement("windows")]
		private bool _windows;

		[XmlIgnore]
		private int _id;
		[XmlIgnore]
		private bool _isRegistered;
		[XmlIgnore]
		private Control _windowControl;

		public event HandledEventHandler Pressed;

		public Hotkey()
			: this(Keys.None, false, false, false, false)
		{
		}

		public Hotkey(Keys keyCode, bool shift, bool control, bool alt, bool windows)
		{
			// Assign properties
			this.KeyCode = keyCode;
			this.Shift = shift;
			this.Control = control;
			this.Alt = alt;
			this.Windows = windows;

			// Register us as a message filter
			Application.AddMessageFilter(this);
		}

		~Hotkey()
		{
			// Unregister the hotkey if necessary
			if (this.IsRegistered)
			{
				this.Unregister();
			}
		}

		public Hotkey Clone()
		{
			// Clone the whole object
			return new Hotkey(this._keyCode, this._shift, this._control, this._alt, this._windows);
		}

		public bool GetCanRegister(Control windowControl)
		{
			// Handle any exceptions: they mean "no, you can't register" :)
			try
			{
				// Attempt to register
				if (this.Register(windowControl))
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

		public bool Register(Control windowControl)
		{
			// Check that we have not registered
			if (this._isRegistered)
			{
				throw new NotSupportedException("You cannot register a hotkey that is already registered");
			}

			// We can't register an empty hotkey
			if (this.IsEmpty)
			{
				throw new NotSupportedException("You cannot register an empty hotkey");
			}

			// Get an ID for the hotkey and increase current ID
			this._id = Hotkey._currentId;
			Hotkey._currentId = Hotkey._currentId + 1 % Hotkey.MaxId;

			// Translate modifier keys into unmanaged version
			uint modifiers = (this.Alt ? HotkeyNativeMethods.MOD_ALT : 0) | (this.Control ? HotkeyNativeMethods.MOD_CONTROL : 0) |
							(this.Shift ? HotkeyNativeMethods.MOD_SHIFT : 0) | (this.Windows ? HotkeyNativeMethods.MOD_WIN : 0);

			// Register the hotkey
			if (HotkeyNativeMethods.RegisterHotKey(windowControl.Handle, this._id, modifiers, _keyCode) == 0)
			{
				// Is the error that the hotkey is registered?
				if (Marshal.GetLastWin32Error() != HotkeyNativeMethods.ERROR_HOTKEY_ALREADY_REGISTERED)
				{
					throw new Win32Exception();
				}

				return false;
			}

			// Save the control reference and register state
			this._isRegistered = true;
			this._windowControl = windowControl;

			// We successfully registered
			return true;
		}

		public void Unregister()
		{
			// Check that we have registered
			if (!this._isRegistered)
			{
				throw new NotSupportedException("You cannot unregister a hotkey that is not registered");
			}

			// It's possible that the control itself has died: in that case, no need to unregister!
			if (!this._windowControl.IsDisposed)
			{
				// Clean up after ourselves
				if (HotkeyNativeMethods.UnregisterHotKey(this._windowControl.Handle, this._id) == 0)
				{
					throw new Win32Exception();
				}
			}

			// Clear the control reference and register state
			this._isRegistered = false;
			this._windowControl = null;
		}

		private void Reregister()
		{
			// Only do something if the key is already registered
			if (!this._isRegistered)
			{
				return;
			}

			// Save control reference
			Control windowControl = this._windowControl;

			// Unregister and then reregister again
			this.Unregister();
			this.Register(windowControl);
		}

		public bool PreFilterMessage(ref Message message)
		{
			// Only process WM_HOTKEY messages
			if (message.Msg != HotkeyNativeMethods.WM_HOTKEY)
			{
				return false;
			}

			// Check that the ID is our key and we are registerd
			return this._isRegistered && (message.WParam.ToInt32() == this._id) && this.OnPressed();
		}

		private bool OnPressed()
		{
			// Fire the event if we can
			HandledEventArgs handledEventArgs = new HandledEventArgs(false);
			this.Pressed?.Invoke(this, handledEventArgs);

			// Return whether we handled the event or not
			return handledEventArgs.Handled;
		}

		public override string ToString()
		{
			// We can be empty
			if (this.IsEmpty)
			{
				return "(none)";
			}

			// Build key name
			string keyName = Enum.GetName(typeof(Keys), this._keyCode) ?? " ";
			switch (this._keyCode)
			{
				case Keys.D0:
				case Keys.D1:
				case Keys.D2:
				case Keys.D3:
				case Keys.D4:
				case Keys.D5:
				case Keys.D6:
				case Keys.D7:
				case Keys.D8:
				case Keys.D9:
					// Strip the first character
					keyName = keyName.Substring(1);
					break;
			}

			// Build modifiers
			string modifiers = "";
			if (this._shift)
			{
				modifiers += "Shift+";
			}

			if (this._control)
			{
				modifiers += "Control+";
			}

			if (this._alt)
			{
				modifiers += "Alt+";
			}

			if (this._windows)
			{
				modifiers += "Windows+";
			}

			// Return result
			return modifiers + keyName;
		}

		public bool IsEmpty
		{
			get
			{
				return this._keyCode == Keys.None;
			}
		}

		public bool IsRegistered
		{
			get
			{
				return this._isRegistered;
			}
		}

		public Keys KeyCode
		{
			get
			{
				return this._keyCode;
			}
			set
			{
				// Save and reregister
				this._keyCode = value;
				this.Reregister();
			}
		}

		public bool Shift
		{
			get
			{
				return this._shift;
			}
			set
			{
				// Save and reregister
				this._shift = value;
				this.Reregister();
			}
		}

		public bool Control
		{
			get
			{
				return this._control;
			}
			set
			{
				// Save and reregister
				this._control = value;
				this.Reregister();
			}
		}

		public bool Alt
		{
			get
			{
				return this._alt;
			}
			set
			{
				// Save and reregister
				this._alt = value;
				this.Reregister();
			}
		}

		public bool Windows
		{
			get
			{
				return this._windows;
			}
			set
			{
				// Save and reregister
				this._windows = value;
				this.Reregister();
			}
		}
	}
}