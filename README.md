# Overview

The purpose of this application is to provide a simple way to keep an eye on several simultaneously running EVE Online clients and to easily switch between them. While running it shows a set of live thumbnails for each of the active EVE Online clients. These thumbnails allow fast switch to the corresponding EVE Online client either using mouse or a configurable hotkey.

It's essentially a task switcher, it does not relay any keyboard/mouse events and suchlike. The app works with EVE, EVE through Steam, or any combination thereof.

The program does NOT (and will NOT ever) do the following things:

* modify EVE Online interface
* display modified EVE Online interface
* broadcast any keyboard or mouse events
* anyhow interact with EVE Online except of bringing its main window to foreground or resizing/minimizing it

**Under any conditions you should NOT use EVE-O Preview for any actions that break EULA or ToS of EVE Online.**

If you have find out that some of the features or their combination of EVE-O Preview might cause actions that can be considered as breaking EULA or ToS of EVE Online you should consider them as a bug and immediately notify the Developer ( Phrynohyas Tig-Rah ) via in-game mail.

# System Requirements

* Windows Vista, Windows 7, Windows 8/8.1, Windows 10
* Windows Aero Enabled
* Microsoft .NET Framework 4.5+


# How To Install & Use

1. Download and extract the contents of the .zip archive to a location of your choice (ie: Desktop, CCP folder, etc)
..* **Note**: Please do not install the application into the *Program Files* or *Program files (x86)* folders. These folders in general do not allow applications to write anything there while EVE-O Preview now stores its configuration file next to its executable, thus requiring the write access to the folder it is installed into.
2. Start up both EVE-O Preview and your EVE Clients (the order does not matter)
3. Adjust settings as you see fit. Program options are described below

# EVE Online EULA/ToS

This application is legal under the EULA/ToS:

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

# Application Options

## Startup Parameters

| Parameter | Description |
| --- | --- |
| **config** | This option allows to start the application with a custom configuration file. If the provided file doesn't exists it will be created with default values.<br />For example **"Eve-O&nbsp;Preview.exe"&nbsp;--config:TestSetup.json** |

## Application Options Available Via GUI

| Tab | Option | Description |
| --- | --- | --- |
| **General** | Minimize to System Tray | Determines whether the main window form be minimized to windows tray when it is closed |
| General | Track client locations | Determines whether the client's window position should be restored when it is activated or started |
| General | Hide preview of active EVE client | Determines whether the thumbnail corresponding to the active EVE client is not displayed |
| General | Previews always on top | Determines whether EVE client thumbnails should stay on top of all other windows |
| General | Hide previews when EVE client is not active | Determines whether all thumbnails should be visible only when an EVE client is active |
| General | Unique layout for each EVE client | Determines whether thumbnails positions are different depending on the EVE client being active (f.e. links char have thumbnails of the Falcon and DPS char in the right bottom corner while DPS and Falcon alts have them placed at the top of the main EVE window ) |
| **Thumbnail** | Opacity | Determines the inactive EVE thumbnails opacity (from almost invisible 20% to 100% solid) |
| Thumbnail | Thumbnail Width | Thumbnails width. Can be set to any value from **100** to **640** points |
| Thumbnail | Thumbnail Height | Thumbnails Height. Can be set to any value from **80** to **400** points |
| **Zoom** | Zoom on hover | Determines whether a thumbnail should be zoomed when the mouse pointer is over it  |
| Zoom | Zoom factor | Thumbnail zoom factor. Can be set to any value from **2** to **10** |
| Zoom | Zoom anchor | Sets the starting point of the thumbnail zoom |
| **Overlay** | Show overlay | Determines whether a name of the corresponding EVE cliet should be displayed on the thumbnail |
| Overlay | Show frames | Determines whether thumbnails should be displayd with window caption and borders |
| Overlay | Highlight active client | Determines whether the thumbnail of the active EVE client should be highlighted with a bright border |
| Overlay | Color | Color used to highlight the active client's thumbnail in case the corresponding option is set |
| **Active Clients** | Thumbnails list | List of currently active EVE client thumbnails. Checking an element in this list will hide the corresponding thumbnail. However these checks are not persisted and on the next EVE client or EVE-O Preview run the thumbnail will be visible again |

## Mouse Gestures and Actions

Mouse gestures are applied to the thumbnail window currently being hovered over.

| Action | Gesture |
| --- | --- |
| Activate the EVE Online client and bring it to front  | Click the thumbnail |
| Minimize the EVE Online client | Hold Control key and click the thumbnail |
| Move thumbnail to a new position | Press right mouse button and move the mouse |
| Adjust thumbnail height | Press both left and right mouse buttons and move the mouse up or down |
| Adjust thumbnail width | Press both left and right mouse buttons and move the mouse left or right |

## Configuration File-Only Options

Some of the application options are not exposed in the GUI. They can be ajusted directly in the configuration file.

**Note:** Do any changes to the configuration file only while the EVE-O Preview itself is closed. Otherwise the changes you made might be lost.

| Option | Description |
| --- | --- |
| **ActiveClientHighlightThickness** | Thickness of the border used to highlight the active client's thumbnail.<br />Allowed values are **1**...**6**.<br />The default value is **3**<br />For example: **"ActiveClientHighlightThickness": 3,** |
| **ThumbnailMinimumSize** | Minimum thumbnail size that can be set either via GUI or by resizing a thumbnail window. Value is witten in the form "width, height"<br />The default value is **"100, 80"**.<br />For example: **"ThumbnailMinimumSize": "100, 80",** |
| **ThumbnailMaximumSize** | Maximum thumbnail size that can be set either via GUI or by resizing a thumbnail window. Value is witten in the form "width, height"<br />The default value is **"640, 400"**.<br />For example: **"ThumbnailMaximumSize": "640, 400",** |

## Hotkey Setup

It is possible to set a key kombinations to immediately jump to cetrain EVE window. However currently EVE-O Preview doesn't provide any GUI to set the these hotkeys. It should be done via editind the configuration file directly. Don't forget to make a backup copy of the file before editing it.

**Note**: Don't forget to make a backup copy of the file before editing it.

Open the file using any text editor. find the entry **ClientHotkey**. Most probably it will look like

    "ClientHotkey": {},

This means that no hotkeys are defined. Edit it to be like

    "ClientHotkey": {
      "EVE - Phrynohyas Tig-Rah": "F1",
      "EVE - Ondatra Patrouette": "F2"
    }

This simple edit will assign **F1** as a hotkey for Phrynohyas Tig-Rah and **F2** as a hotkey for Ondatra Patrouette, so pressing F1 anywhere in Windows will immediately open EVE client for Phrynohyas Tig-Rah if he is logged on.

The following hotkey is described as `modifier+key` where `modifier` can be **Control**, **Alt**, **Shift**, or their combination. F.e. it is possible to setup the hotkey as

    "ClientHotkey": {
      "EVE - Phrynohyas Tig-Rah": "F1",
      "EVE - Ondatra Patrouette": "Control+Shift+F4"
    }

**Note:** Do not set hotkeys to use the key combinations already used by EVE. It won't work as "_I set hotkey for my DPS char to F1 and when I'll press F1 it will automatically open the DPS char's window and activate guns_". Key combination will be swallowed by EVE-O Preview and NOT retranslated to EVE window. So it will be only "_it will automatically open the DPS char's window_".


---

# Credits

## Created by

* StinkRay


## Maintained by

* Phrynohyas Tig-Rah


## Previous maintainers
 
* Makari Aeron

* StinkRay


## With contributions from

* CCP FoxFour


## Forum thread

https://meta.eveonline.com/t/4202


## Original threads

* https://forums.eveonline.com/default.aspx?g=posts&t=389086

* https://forums.eveonline.com/default.aspx?g=posts&t=246157

* https://forums.eveonline.com/default.aspx?g=posts&t=484927


## Original repository

https://bitbucket.org/ulph/eve-o-preview-git
