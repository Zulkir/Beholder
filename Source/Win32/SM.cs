/*
Copyright (c) 2010-2014 Beholder Project - Daniil Rodin

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.
*/

namespace Win32
{
    /// <summary>
    /// Flags used with the Windows API (User32.dll):GetSystemMetrics(SystemMetric smIndex)
    ///   
    /// This Enum and declaration signature was written by Gabriel T. Sharp
    /// ai_productions@verizon.net or osirisgothra@hotmail.com
    /// Obtained on pinvoke.net, please contribute your code to support the wiki!
    /// </summary>
    public enum SM
    {
        /// <summary>
        ///  Width of the screen of the primary display monitor, in pixels. This is the same values obtained by calling GetDeviceCaps as follows: GetDeviceCaps( hdcPrimaryMonitor, HORZRES).
        /// </summary>
        CXSCREEN = 0,
        /// <summary>
        /// Height of the screen of the primary display monitor, in pixels. This is the same values obtained by calling GetDeviceCaps as follows: GetDeviceCaps( hdcPrimaryMonitor, VERTRES).
        /// </summary>
        CYSCREEN = 1,
        /// <summary>
        /// Height of the arrow bitmap on a vertical scroll bar, in pixels.
        /// </summary>
        CYVSCROLL = 20,
        /// <summary>
        /// Width of a vertical scroll bar, in pixels.
        /// </summary>
        CXVSCROLL = 2,
        /// <summary>
        /// Height of a caption area, in pixels.
        /// </summary>
        CYCAPTION = 4,
        /// <summary>
        /// Width of a window border, in pixels. This is equivalent to the CXEDGE value for windows with the 3-D look. 
        /// </summary>
        CXBORDER = 5,
        /// <summary>
        /// Height of a window border, in pixels. This is equivalent to the CYEDGE value for windows with the 3-D look. 
        /// </summary>
        CYBORDER = 6,
        /// <summary>
        /// Thickness of the frame around the perimeter of a window that has a caption but is not sizable, in pixels. CXFIXEDFRAME is the height of the horizontal border and CYFIXEDFRAME is the width of the vertical border. 
        /// </summary>
        CXDLGFRAME = 7,
        /// <summary>
        /// Thickness of the frame around the perimeter of a window that has a caption but is not sizable, in pixels. CXFIXEDFRAME is the height of the horizontal border and CYFIXEDFRAME is the width of the vertical border. 
        /// </summary>
        CYDLGFRAME = 8,
        /// <summary>
        /// Height of the thumb box in a vertical scroll bar, in pixels
        /// </summary>
        CYVTHUMB = 9,
        /// <summary>
        /// Width of the thumb box in a horizontal scroll bar, in pixels.
        /// </summary>
        CXHTHUMB = 10,
        /// <summary>
        /// Default width of an icon, in pixels. The LoadIcon function can load only icons with the dimensions specified by CXICON and CYICON
        /// </summary>
        CXICON = 11,
        /// <summary>
        /// Default height of an icon, in pixels. The LoadIcon function can load only icons with the dimensions CXICON and CYICON.
        /// </summary>
        CYICON = 12,
        /// <summary>
        /// Width of a cursor, in pixels. The system cannot create cursors of other sizes.
        /// </summary>
        CXCURSOR = 13,
        /// <summary>
        /// Height of a cursor, in pixels. The system cannot create cursors of other sizes.
        /// </summary>
        CYCURSOR = 14,
        /// <summary>
        /// Height of a single-line menu bar, in pixels.
        /// </summary>
        CYMENU = 15,
        /// <summary>
        /// Width of the client area for a full-screen window on the primary display monitor, in pixels. To get the coordinates of the portion of the screen not obscured by the system taskbar or by application desktop toolbars, call the SystemParametersInfo function with the SPI_GETWORKAREA value.
        /// </summary>
        CXFULLSCREEN = 16,
        /// <summary>
        /// Height of the client area for a full-screen window on the primary display monitor, in pixels. To get the coordinates of the portion of the screen not obscured by the system taskbar or by application desktop toolbars, call the SystemParametersInfo function with the SPI_GETWORKAREA value.
        /// </summary>
        CYFULLSCREEN = 17,
        /// <summary>
        /// For double byte character set versions of the system, this is the height of the Kanji window at the bottom of the screen, in pixels
        /// </summary>
        CYKANJIWINDOW = 18,
        /// <summary>
        /// Nonzero if a mouse with a wheel is installed; zero otherwise
        /// </summary>
        MOUSEWHEELPRESENT = 75,
        /// <summary>
        /// Height of a horizontal scroll bar, in pixels.
        /// </summary>
        CYHSCROLL = 3,
        /// <summary>
        /// Width of the arrow bitmap on a horizontal scroll bar, in pixels.
        /// </summary>
        CXHSCROLL = 21,
        /// <summary>
        /// Nonzero if the debug version of User.exe is installed; zero otherwise.
        /// </summary>
        DEBUG = 22,
        /// <summary>
        /// Nonzero if the left and right mouse buttons are reversed; zero otherwise.
        /// </summary>
        SWAPBUTTON = 23,
        /// <summary>
        /// Reserved for future use
        /// </summary>
        RESERVED1 = 24,
        /// <summary>
        /// Reserved for future use
        /// </summary>
        RESERVED2 = 25,
        /// <summary>
        /// Reserved for future use
        /// </summary>
        RESERVED3 = 26,
        /// <summary>
        /// Reserved for future use
        /// </summary>
        RESERVED4 = 27,
        /// <summary>
        /// Minimum width of a window, in pixels.
        /// </summary>
        CXMIN = 28,
        /// <summary>
        /// Minimum height of a window, in pixels.
        /// </summary>
        CYMIN = 29,
        /// <summary>
        /// Width of a button in a window's caption or title bar, in pixels.
        /// </summary>
        CXSIZE = 30,
        /// <summary>
        /// Height of a button in a window's caption or title bar, in pixels.
        /// </summary>
        CYSIZE = 31,
        /// <summary>
        /// Thickness of the sizing border around the perimeter of a window that can be resized, in pixels. CXSIZEFRAME is the width of the horizontal border, and CYSIZEFRAME is the height of the vertical border. 
        /// </summary>
        CXFRAME = 32,
        /// <summary>
        /// Thickness of the sizing border around the perimeter of a window that can be resized, in pixels. CXSIZEFRAME is the width of the horizontal border, and CYSIZEFRAME is the height of the vertical border. 
        /// </summary>
        CYFRAME = 33,
        /// <summary>
        /// Minimum tracking width of a window, in pixels. The user cannot drag the window frame to a size smaller than these dimensions. A window can override this value by processing the WM_GETMINMAXINFO message.
        /// </summary>
        CXMINTRACK = 34,
        /// <summary>
        /// Minimum tracking height of a window, in pixels. The user cannot drag the window frame to a size smaller than these dimensions. A window can override this value by processing the WM_GETMINMAXINFO message
        /// </summary>
        CYMINTRACK = 35,
        /// <summary>
        /// Width of the rectangle around the location of a first click in a double-click sequence, in pixels. The second click must occur within the rectangle defined by CXDOUBLECLK and CYDOUBLECLK for the system to consider the two clicks a double-click
        /// </summary>
        CXDOUBLECLK = 36,
        /// <summary>
        /// Height of the rectangle around the location of a first click in a double-click sequence, in pixels. The second click must occur within the rectangle defined by CXDOUBLECLK and CYDOUBLECLK for the system to consider the two clicks a double-click. (The two clicks must also occur within a specified time.) 
        /// </summary>
        CYDOUBLECLK = 37,
        /// <summary>
        /// Width of a grid cell for items in large icon view, in pixels. Each item fits into a rectangle of size CXICONSPACING by CYICONSPACING when arranged. This value is always greater than or equal to CXICON
        /// </summary>
        CXICONSPACING = 38,
        /// <summary>
        /// Height of a grid cell for items in large icon view, in pixels. Each item fits into a rectangle of size CXICONSPACING by CYICONSPACING when arranged. This value is always greater than or equal to CYICON.
        /// </summary>
        CYICONSPACING = 39,
        /// <summary>
        /// Nonzero if drop-down menus are right-aligned with the corresponding menu-bar item; zero if the menus are left-aligned.
        /// </summary>
        MENUDROPALIGNMENT = 40,
        /// <summary>
        /// Nonzero if the Microsoft Windows for Pen computing extensions are installed; zero otherwise.
        /// </summary>
        PENWINDOWS = 41,
        /// <summary>
        /// Nonzero if User32.dll supports DBCS; zero otherwise. (WinMe/95/98): Unicode
        /// </summary>
        DBCSENABLED = 42,
        /// <summary>
        /// Number of buttons on mouse, or zero if no mouse is installed.
        /// </summary>
        CMOUSEBUTTONS = 43,
        /// <summary>
        /// Identical Values Changed After Windows NT 4.0  
        /// </summary>
        CXFIXEDFRAME = CXDLGFRAME,
        /// <summary>
        /// Identical Values Changed After Windows NT 4.0
        /// </summary>
        CYFIXEDFRAME = CYDLGFRAME,
        /// <summary>
        /// Identical Values Changed After Windows NT 4.0
        /// </summary>
        CXSIZEFRAME = CXFRAME,
        /// <summary>
        /// Identical Values Changed After Windows NT 4.0
        /// </summary>
        CYSIZEFRAME = CYFRAME,
        /// <summary>
        /// Nonzero if security is present; zero otherwise.
        /// </summary>
        SECURE = 44,
        /// <summary>
        /// Width of a 3-D border, in pixels. This is the 3-D counterpart of CXBORDER
        /// </summary>
        CXEDGE = 45,
        /// <summary>
        /// Height of a 3-D border, in pixels. This is the 3-D counterpart of CYBORDER
        /// </summary>
        CYEDGE = 46,
        /// <summary>
        /// Width of a grid cell for a minimized window, in pixels. Each minimized window fits into a rectangle this size when arranged. This value is always greater than or equal to CXMINIMIZED.
        /// </summary>
        CXMINSPACING = 47,
        /// <summary>
        /// Height of a grid cell for a minimized window, in pixels. Each minimized window fits into a rectangle this size when arranged. This value is always greater than or equal to CYMINIMIZED.
        /// </summary>
        CYMINSPACING = 48,
        /// <summary>
        /// Recommended width of a small icon, in pixels. Small icons typically appear in window captions and in small icon view
        /// </summary>
        CXSMICON = 49,
        /// <summary>
        /// Recommended height of a small icon, in pixels. Small icons typically appear in window captions and in small icon view.
        /// </summary>
        CYSMICON = 50,
        /// <summary>
        /// Height of a small caption, in pixels
        /// </summary>
        CYSMCAPTION = 51,
        /// <summary>
        /// Width of small caption buttons, in pixels.
        /// </summary>
        CXSMSIZE = 52,
        /// <summary>
        /// Height of small caption buttons, in pixels.
        /// </summary>
        CYSMSIZE = 53,
        /// <summary>
        /// Width of menu bar buttons, such as the child window close button used in the multiple document interface, in pixels.
        /// </summary>
        CXMENUSIZE = 54,
        /// <summary>
        /// Height of menu bar buttons, such as the child window close button used in the multiple document interface, in pixels.
        /// </summary>
        CYMENUSIZE = 55,
        /// <summary>
        /// Flags specifying how the system arranged minimized windows
        /// </summary>
        ARRANGE = 56,
        /// <summary>
        /// Width of a minimized window, in pixels.
        /// </summary>
        CXMINIMIZED = 57,
        /// <summary>
        /// Height of a minimized window, in pixels.
        /// </summary>
        CYMINIMIZED = 58,
        /// <summary>
        /// Default maximum width of a window that has a caption and sizing borders, in pixels. This metric refers to the entire desktop. The user cannot drag the window frame to a size larger than these dimensions. A window can override this value by processing the WM_GETMINMAXINFO message.
        /// </summary>
        CXMAXTRACK = 59,
        /// <summary>
        /// Default maximum height of a window that has a caption and sizing borders, in pixels. This metric refers to the entire desktop. The user cannot drag the window frame to a size larger than these dimensions. A window can override this value by processing the WM_GETMINMAXINFO message.
        /// </summary>
        CYMAXTRACK = 60,
        /// <summary>
        /// Default width, in pixels, of a maximized top-level window on the primary display monitor.
        /// </summary>
        CXMAXIMIZED = 61,
        /// <summary>
        /// Default height, in pixels, of a maximized top-level window on the primary display monitor.
        /// </summary>
        CYMAXIMIZED = 62,
        /// <summary>
        /// Least significant bit is set if a network is present; otherwise, it is cleared. The other bits are reserved for future use
        /// </summary>
        NETWORK = 63,
        /// <summary>
        /// Value that specifies how the system was started: 0-normal, 1-failsafe, 2-failsafe /w net
        /// </summary>
        CLEANBOOT = 67,
        /// <summary>
        /// Width of a rectangle centered on a drag point to allow for limited movement of the mouse pointer before a drag operation begins, in pixels. 
        /// </summary>
        CXDRAG = 68,
        /// <summary>
        /// Height of a rectangle centered on a drag point to allow for limited movement of the mouse pointer before a drag operation begins. This value is in pixels. It allows the user to click and release the mouse button easily without unintentionally starting a drag operation.
        /// </summary>
        CYDRAG = 69,
        /// <summary>
        /// Nonzero if the user requires an application to present information visually in situations where it would otherwise present the information only in audible form; zero otherwise. 
        /// </summary>
        SHOWSOUNDS = 70,
        /// <summary>
        /// Width of the default menu check-mark bitmap, in pixels.
        /// </summary>
        CXMENUCHECK = 71,
        /// <summary>
        /// Height of the default menu check-mark bitmap, in pixels.
        /// </summary>
        CYMENUCHECK = 72,
        /// <summary>
        /// Nonzero if the computer has a low-end (slow) processor; zero otherwise
        /// </summary>
        SLOWMACHINE = 73,
        /// <summary>
        /// Nonzero if the system is enabled for Hebrew and Arabic languages, zero if not.
        /// </summary>
        MIDEASTENABLED = 74,
        /// <summary>
        /// Nonzero if a mouse is installed; zero otherwise. This value is rarely zero, because of support for virtual mice and because some systems detect the presence of the port instead of the presence of a mouse.
        /// </summary>
        MOUSEPRESENT = 19,
        /// <summary>
        /// Windows 2000 (v5.0+) Coordinate of the top of the virtual screen
        /// </summary>
        XVIRTUALSCREEN = 76,
        /// <summary>
        /// Windows 2000 (v5.0+) Coordinate of the left of the virtual screen
        /// </summary>
        YVIRTUALSCREEN = 77,
        /// <summary>
        /// Windows 2000 (v5.0+) Width of the virtual screen
        /// </summary>
        CXVIRTUALSCREEN = 78,
        /// <summary>
        /// Windows 2000 (v5.0+) Height of the virtual screen
        /// </summary>
        CYVIRTUALSCREEN = 79,
        /// <summary>
        /// Number of display monitors on the desktop
        /// </summary>
        CMONITORS = 80,
        /// <summary>
        /// Windows XP (v5.1+) Nonzero if all the display monitors have the same color format, zero otherwise. Note that two displays can have the same bit depth, but different color formats. For example, the red, green, and blue pixels can be encoded with different numbers of bits, or those bits can be located in different places in a pixel's color value. 
        /// </summary>
        SAMEDISPLAYFORMAT = 81,
        /// <summary>
        /// Windows XP (v5.1+) Nonzero if Input Method Manager/Input Method Editor features are enabled; zero otherwise
        /// </summary>
        IMMENABLED = 82,
        /// <summary>
        /// Windows XP (v5.1+) Width of the left and right edges of the focus rectangle drawn by DrawFocusRect. This value is in pixels. 
        /// </summary>
        CXFOCUSBORDER = 83,
        /// <summary>
        /// Windows XP (v5.1+) Height of the top and bottom edges of the focus rectangle drawn by DrawFocusRect. This value is in pixels. 
        /// </summary>
        CYFOCUSBORDER = 84,
        /// <summary>
        /// Nonzero if the current operating system is the Windows XP Tablet PC edition, zero if not.
        /// </summary>
        TABLETPC = 86,
        /// <summary>
        /// Nonzero if the current operating system is the Windows XP, Media Center Edition, zero if not.
        /// </summary>
        MEDIACENTER = 87,
        /// <summary>
        /// Metrics Other
        /// </summary>
        CMETRICS_OTHER = 76,
        /// <summary>
        /// Metrics Windows 2000
        /// </summary>
        CMETRICS_2000 = 83,
        /// <summary>
        /// Metrics Windows NT
        /// </summary>
        CMETRICS_NT = 88,
        /// <summary>
        /// Windows XP (v5.1+) This system metric is used in a Terminal Services environment. If the calling process is associated with a Terminal Services client session, the return value is nonzero. If the calling process is associated with the Terminal Server console session, the return value is zero. The console session is not necessarily the physical console - see WTSGetActiveConsoleSessionId for more information. 
        /// </summary>
        REMOTESESSION = 0x1000,
        /// <summary>
        /// Windows XP (v5.1+) Nonzero if the current session is shutting down; zero otherwise
        /// </summary>
        SHUTTINGDOWN = 0x2000,
        /// <summary>
        /// Windows XP (v5.1+) This system metric is used in a Terminal Services environment. Its value is nonzero if the current session is remotely controlled; zero otherwise
        /// </summary>
        REMOTECONTROL = 0x2001,
    }
}
