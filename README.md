The purpose of this application is to provide a simple way to keep an eye on several simultaneously running EVE Online clients and to easily switch between them. While running it shows a set of live thumbnails for each of the active EVE Online clients. These thumbnails allow fast switch to the corresponding EVE Online client either using mouse or a configurable hotkey.

It's essentially a task switcher, it does not relay any keyboard/mouse events and suchlike. The app works with EVE, EVE through Steam, or any combination thereof.

The program does NOT (and will NOT ever) do the following things:
* modify EVE Online interface
* display modified EVE Online interface
* broadcast any keyboard or mouse events
* anyhow interact with EVE Online except of bringing its main window to foreground or resizing it


**System Requirements**

* Windows Vista, Windows 7, Windows 8/8.1, Windows 10
* Windows Aero Enabled
* Microsoft .NET Framework 4.5+


**How To Install & Use**

1. Download and extract the contents of the .zip archive to a location of your choice (ie: Desktop, CCP folder, etc)
..* **Note**: Please do not install the program into the *Program Files* or *Program files (x86)* folders. These folders in general do not allow applications to write anything there while EVE-O Preview now stores its configuration file next to its executable, thus requiring the write access to the folder it is installed into.
2. Start up both EVE-O Preview and your EVE Clients (the order does not matter)
3. Adjust settings as you see fit. Program options are described below

***

This program is legal under the EULA/ToS:

CCP FoxFour wrote:
> Please keep the discussion on topic. The legitimacy of this software has already been discussed
> and doesn't need to be again. Assuming the functionality of the software doesn't change, it is
> allowed in its current state.

[CCP Grimmi wrote](https://forums.eveonline.com/default.aspx?g=posts&m=6362936#post6362936):
> Overlays which contain a full, unchanged, EVE Client instance in a view only mode, no matter
> how large or small they are scaled, like it is done by EVE-O Preview as of today, are fine
> with us. These overlays do not allow any direct interaction with the EVE Client and you have
> to bring the respective EVE Client to the front/put the window focus on it, in order to
> interact with it.

***
---

**Startup Parameters**

| Parameter | Description |
| --- | --- |
| **config** | This option allows to start the application with a custom configuration file. If the provided file doesn't exists it will be created with default values.<br />For example **"Eve-O&nbsp;Preview.exe"&nbsp;--config:TestSetup.json** |

**Program options**

| Option | Description |
| --- | --- |
| Minimize to System Tray | Determines whether the main window form be minimized to windows tray when it is closed |
| Opacity | Determines the inactive EVE thumbnails opacity (from almost invisible 20% to 100% solid) |
| Track client locations | Determines whether the client's window position should be restored when it is activated or started |
| Hide preview of active EVE client | Determines whether the thumbnail corresponding to the active EVE client is not displayed |
| Previews always on top | Determines whether EVE client thumbnails should stay on top of all other windows |
| Hide previews when EVE client is not active | Determines whether all thumbnails should be visible only when an EVE client is active |
| Unique layout for each EVE client | Determines whether thumbnails positions are different depending on the EVE client being active (f.e. links char have thumbnails of the Falcon and DPS char in the right bottom corner while DPS and Falcon alts have them placed at the top of the main EVE window ) |
| Thumbnail width | Thumbnails width. Can be set to any value from **100** to **640** points |
| Thumbnail height | Thumbnails Height. Can be set to any value from **80** to **400** points |
| Zoom on hover | Determines whether a thumbnail should be zoomed when the mouse pointer is over it  |
| Zoom factor | Thumbnail zoom factor. Can be set to any value from **2** to **10** |
| Zoom anchor | Sets the starting point of the thumbnail zoom |
| Show overlay | Determines whether a name of the corresponding EVE cliet should be displayed on the thumbnail |
| Show frames | Determines whether thumbnails should be displayd with window caption and borders |
| Highlight active client | Determines whether the thumbnail of the active EVE client should be highlighted with a bright border |
| Color | Color used to highlight the active client's thumbnail in case the corresponding option is set |
| Thumbnails list | List of currently active EVE client thumbnails. Checking an element in this list will hide the corresponding thumbnail. However these checks are not persisted and on the next EVE client or EVE-O Preview run the thumbnail will be visible again |

**Mouse Gestures**

Mouse gestures are applied to the thumbnail window currently being hovered over.

| Action | Gesture |
| --- | --- |
| Move thumbnail to a new position | Press right mouse button and move the mouse |
| Adjust thumbnail height | Press both left and right mouse buttons and move the mouse up or down |
| Adjust thumbnail width | Press both left and right mouse buttons and move the mouse left or right |

**Configuration File Options**

_Left Blank_

**Hotkey Setup**

_Left Blank_

---

**Created by**

* StinkRay


**Maintained by**

* Phrynohyas Tig-Rah
 
* Makari Aeron

* StinkRay


**With contributions from**

* CCP FoxFour


**Original threads**

https://forums.eveonline.com/default.aspx?g=posts&t=389086
https://forums.eveonline.com/default.aspx?g=posts&t=246157


**Original repository**

https://bitbucket.org/ulph/eve-o-preview-git
