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

// This is a purely experimental class.
// Code from the OpenTK project is used to write this class.

using System;
using System.Runtime.InteropServices;
using Beholder.Input;
using Win32;


namespace WindowsPlatform
{
    public class GameWindow //: ICWindow
    {
        #region 'Constants'
        static readonly IntPtr NULL = IntPtr.Zero;
        #endregion

        #region Fields
        //const ExtendedWindowStyle ParentStyleEx = ExtendedWindowStyle.WindowEdge | ExtendedWindowStyle.ApplicationWindow;
        //const ExtendedWindowStyle ChildStyleEx = 0;

        readonly WndProc wndProc;
        readonly IntPtr instance = Marshal.GetHINSTANCE(typeof(GameWindow).Module);
        readonly string className = Guid.NewGuid().ToString(); //Marshal.StringToHGlobalAuto(Guid.NewGuid().ToString());
        //readonly WindowProcedure windowProcedureDelegate;
        readonly IntPtr hwnd;

        const uint ModalLoopTimerPeriod = 1;
        readonly Action onTimer;
        IntPtr timerHandle;
        
        //readonly TimerProc modalLoopCallback;

        //bool classRegistered;
        //bool disposed;
        //bool exists;

        //WinWindowInfo window, child_window;
        //WindowBorder windowBorder = WindowBorder.Resizable;
        //Nullable<WindowBorder> previous_window_border; // Set when changing to fullscreen state.
        //Nullable<WindowBorder> deferred_window_border; // Set to avoid changing borders during fullscreen state.
        //WindowState windowState = WindowState.Normal;
        //bool borderless_maximized_window_state = false; // Hack to get maximized mode with hidden border (not normally possible).
        bool focused;
        //bool mouseOutsideWindow = true;
        //bool invisible_since_creation; // Set by WindowsMessage.CREATE and consumed by Visible = true (calls BringWindowToFront).
        //int suppressResize; // Used in WindowBorder and WindowState in order to avoid rapid, consecutive resize events.

        //Rectangle bounds;
        bool resizingOrMoving;
        RECT clientRectangle;
        //Rectangle previousBounds; // Used to restore previous size when leaving fullscreen mode.

        //Icon icon;

        //const ClassStyle DefaultClassStyle = ClassStyle.OwnDC;

        //readonly IntPtr defaultWindowProcedure =
        //    Marshal.GetFunctionPointerForDelegate(new WindowProcedure(Functions.DefWindowProc));

        // Used for IInputDriver implementation
        //WinMMJoystick joystick_driver = new WinMMJoystick();
        //KeyboardDevice keyboard = new KeyboardDevice();
        //MouseDevice mouse = new MouseDevice();
        //IList<KeyboardDevice> keyboards = new List<KeyboardDevice>(1);
        //IList<MouseDevice> mice = new List<MouseDevice>(1);
        //internal static readonly WinKeyMap KeyMap = new WinKeyMap();
        //const long ExtendedBit = 1 << 24;           // Used to distinguish left and right control, alt and enter keys.
        //static readonly uint ShiftRightScanCode = Functions.MapVirtualKey(VirtualKeys.RSHIFT, 0);         // Used to distinguish left and right shift keys.

        //KeyPressEventArgs key_press = new KeyPressEventArgs((char)0);

        bool minimized;

        

        readonly Input input;
        #endregion

        public IInputHandler Input { get { return input; } }
        public IntPtr Handle { get { return hwnd; } }
        public bool ContainsFocus { get { return focused; } }
        public bool Minimized { get { return minimized; } }
        public bool ResizingOrMoving { get { return resizingOrMoving; } }
        public int Width { get { return clientRectangle.Width; } }
        public int Height { get { return clientRectangle.Height; } }
        public float AspectRatio
        {
            get
            {
                if (clientRectangle.Width == 0 || clientRectangle.Height == 0)
                {
                    return 1.0f;
                }

                return (float)clientRectangle.Width / clientRectangle.Height;
            }
        }
        internal void GetClientRect(out RECT rect)
        {
            rect = clientRectangle;
        }


        void StartTimer(IntPtr handle)
        {
            if (timerHandle == IntPtr.Zero)
            {
                timerHandle = Functions.SetTimer(handle, new IntPtr(1), ModalLoopTimerPeriod, null);
            }
        }

        void StopTimer(IntPtr handle)
        {
            if (timerHandle != IntPtr.Zero)
            {
                Functions.KillTimer(handle, timerHandle);
                timerHandle = IntPtr.Zero;
            }
        }

        void OnMouseEvent(MouseEventType eventType, IntPtr wParam, IntPtr lParam)
        {
            POINT point = new POINT(
                            (short)((uint)lParam.ToInt32() & 0x0000FFFF),
                            (short)(((uint)lParam.ToInt32() & 0xFFFF0000) >> 16));

            MK mk = (MK)(wParam.ToInt64() & 0xFF);

            MouseButtons buttons = MouseButtons.None;
            if ((mk & MK.LBUTTON) != 0) buttons |= MouseButtons.Left;
            if ((mk & MK.RBUTTON) != 0) buttons |= MouseButtons.Right;
            if ((mk & MK.MBUTTON) != 0) buttons |= MouseButtons.Middle;
            if ((mk & MK.XBUTTON1) != 0) buttons |= MouseButtons.XButton1;
            if ((mk & MK.XBUTTON2) != 0) buttons |= MouseButtons.XButton2;

            input.OnMouseEvent(new MouseEventInfo(
                 eventType,
                 buttons,
                 point.X, point.Y,
                 (int)((long)wParam << 32 >> 48)));
        }



        internal IntPtr ProcessMessages(IntPtr handle, WM message, IntPtr wParam, IntPtr lParam)
        {
            switch (message)
            {
                #region Size / Move / Style events

                case WM.ACTIVATE:
                    // See http://msdn.microsoft.com/en-us/library/ms646274(VS.85).aspx (WM_ACTIVATE notification):
                    // wParam: The low-order word specifies whether the window is being activated or deactivated.
                    //bool newFocusedState = focused;
                    if (IntPtr.Size == 4)
                        focused = (wParam.ToInt32() & 0xFFFF) != 0;
                    else
                        focused = (wParam.ToInt64() & 0xFFFF) != 0;

                    //if (newFocusedState != focused && FocusedChanged != null)
                    //    FocusedChanged(this, EventArgs.Empty);

                    if (focused)
                    {
                        input.OnGotFocus();
                    }
                    else
                    {
                        input.OnLostFocus();
                    }

                    break;

                
                case WM.ENTERMENULOOP:
                    //StartTimer(handle);
                    break;
                case WM.ENTERSIZEMOVE:
                    // Entering the modal size/move loop: we don't want rendering to
                    // stop during this time, so we register a timer callback to continue
                    // processing from time to time.
                    //StartTimer(handle);
                    resizingOrMoving = true;
                    break;

                case WM.EXITMENULOOP:
                    //StopTimer(handle);
                    break;
                case WM.EXITSIZEMOVE:
                    // ExitingmModal size/move loop: the timer callback is no longer
                    // necessary.
                    //StopTimer(handle);
                    resizingOrMoving = false;
                    break;
                    /*
                case WM.TIMER:
                    if (wParam == timerHandle)
                    {
                        onTimer();
                    }
                    break;*/

                case WM.ERASEBKGND:
                    {
                        return new IntPtr(1);
                    }
                case WM.SYSCOMMAND:
                    {
                        if (wParam.ToInt32() == 61696)
                        {
                            return NULL;
                        }
                    }
                    break;
                    /*
                case WindowMessage.WINDOWPOSCHANGED:
                    unsafe
                    {
                        WindowPosition* pos = (WindowPosition*)lParam;
                        if (window != null && pos->hwnd == window.WindowHandle)
                        {
                            Point new_location = new Point(pos->x, pos->y);
                            if (Location != new_location)
                            {
                                bounds.Location = new_location;
                                if (Move != null)
                                    Move(this, EventArgs.Empty);
                            }

                            Size new_size = new Size(pos->cx, pos->cy);
                            if (Size != new_size)
                            {
                                bounds.SwapChainWidth = pos->cx;
                                bounds.SwapChainHeight = pos->cy;

                                Win32Rectangle rect;
                                Functions.GetClientRect(handle, out rect);
                                client_rectangle = rect.ToRectangle();

                                Functions.SetWindowPos(child_window.WindowHandle, IntPtr.Zero, 0, 0, ClientRectangle.SwapChainWidth, ClientRectangle.SwapChainHeight,
                                    SetWindowPosFlags.NOZORDER | SetWindowPosFlags.NOOWNERZORDER |
                                    SetWindowPosFlags.NOACTIVATE | SetWindowPosFlags.NOSENDCHANGING);

                                if (suppress_resize <= 0 && Resize != null)
                                    Resize(this, EventArgs.Empty);
                            }
                        }
                    }
                    break;*/
                case WM.WINDOWPOSCHANGED:
                    {
                        Functions.GetClientRect(hwnd, out clientRectangle);
                    }
                    break;

                    /*
                case WindowMessage.STYLECHANGED:
                    unsafe
                    {
                        if (wParam.ToInt64() == (long)GWL.STYLE)
                        {
                            WindowStyle style = ((StyleStruct*)lParam)->New;
                            if ((style & WindowStyle.Popup) != 0)
                                windowBorder = WindowBorder.Hidden;
                            else if ((style & WindowStyle.ThickFrame) != 0)
                                windowBorder = WindowBorder.Resizable;
                            else if ((style & ~(WindowStyle.ThickFrame | WindowStyle.MaximizeBox)) != 0)
                                windowBorder = WindowBorder.Fixed;
                        }
                    }

                    break;*/

                    /*
                case WindowMessage.SIZE:
                    SizeMessage state = (SizeMessage)wParam.ToInt64();
                    WindowState new_state = windowState;
                    switch (state)
                    {
                        case SizeMessage.RESTORED: new_state = borderless_maximized_window_state ?
                            WindowState.Maximized : WindowState.Normal; break;
                        case SizeMessage.MINIMIZED: new_state = WindowState.Minimized; break;
                        case SizeMessage.MAXIMIZED: new_state = WindowBorder == WindowBorder.Hidden ?
                            WindowState.Fullscreen : WindowState.Maximized;
                            break;
                    }

                    if (new_state != windowState)
                    {
                        windowState = new_state;
                        if (WindowStateChanged != null)
                            WindowStateChanged(this, EventArgs.Empty);
                    }

                    break;*/

                case WM.SIZE:
                    {
                        SIZE state = (SIZE)wParam.ToInt64();
                        minimized = state == SIZE.MINIMIZED;
                        Functions.GetClientRect(hwnd, out clientRectangle);
                    }
                    break;
                #endregion

                #region Input
                    /*
                case WM.CHAR:
                    */
                case WM.MOUSEMOVE: 
                    OnMouseEvent(MouseEventType.Move, wParam, lParam); 
                    break;
                case WM.LBUTTONDOWN:
                case WM.RBUTTONDOWN:
                case WM.MBUTTONDOWN: 
                case WM.XBUTTONDOWN:
                    OnMouseEvent(MouseEventType.Down, wParam, lParam); 
                    break;
                case WM.LBUTTONUP:
                case WM.RBUTTONUP:
                case WM.MBUTTONUP:
                case WM.XBUTTONUP:
                    OnMouseEvent(MouseEventType.Up, wParam, lParam);
                    break;
                case WM.LBUTTONDBLCLK:
                case WM.RBUTTONDBLCLK:
                case WM.MBUTTONDBLCLK:
                case WM.XBUTTONDBLCLK:
                    OnMouseEvent(MouseEventType.DoubleClick, wParam, lParam);
                    break;
                case WM.MOUSEWHEEL: 
                    OnMouseEvent(MouseEventType.Wheel, wParam, lParam); 
                    break;
                #endregion

                case WM.DESTROY:
                    Functions.UnregisterClass(className, instance);
                    Functions.PostQuitMessage(0);
                    break;
            }

            return Functions.DefWindowProc(handle, message, wParam, lParam);
        }

        internal GameWindow(int x, int y, int width, int height, string title, IntPtr icon, IntPtr smallIcon, Action onTimer)
        {
            this.onTimer = onTimer;

            wndProc = ProcessMessages;

            WNDCLASSEX wc = new WNDCLASSEX
            {
                cbSize = (uint)WNDCLASSEX.SizeInBytes,
                //style = ClassStyles.HorizontalRedraw | ClassStyles.VerticalRedraw,
                style = ClassStyles.OwnDC | ClassStyles.HorizontalRedraw | ClassStyles.VerticalRedraw,
                hInstance = instance,
                lpfnWndProc = wndProc,
                lpszClassName = className,
                hIcon = icon,
                hIconSm = smallIcon,
                hCursor = Functions.LoadCursor(NULL, IDC.ARROW),

                hbrBackground = NULL,
                cbClsExtra = 0,
                cbWndExtra = 0,
                lpszMenuName = null
            };

            short atom = Functions.RegisterClassEx(ref wc);

            if (atom == 0)
                throw new Exception(String.Format("Failed to register window class. Error: {0}", Marshal.GetLastWin32Error()));


            const WS style = //(WS)0x16cf0000;
                //WS.CAPTION | WS.VISIBLE | WS.CLIPSIBLINGS | WS.CLIPCHILDREN |
                //WS.SYSMENU | WS.SIZEFRAME | WS.OVERLAPPEDWINDOW | WS.MINIMIZEBOX | WS.MAXIMIZEBOX; 
            (WS)114229248;

            const WS_EX exStyle = //(WS_EX)0x00040100;
                //WS_EX.LEFT | WS_EX.LTRREADING | WS_EX.RIGHTSCROLLBAR |
                //WS_EX.WINDOWEDGE | WS_EX.APPWINDOW; 
            (WS_EX)262401 ^ WS_EX.DLGMODALFRAME;

            //IntPtr windowName = Marshal.StringToHGlobalAuto(title);

            // Find out the final window rectangle, after the WM has added its chrome (titlebar, sidebars etc).
            RECT rect = new RECT { left = x, top = y, right = x + width, bottom = y + height };
            Functions.AdjustWindowRectEx(ref rect, style, false, exStyle);

            hwnd = Functions.CreateWindowEx(
                exStyle,
                className, 
                title, 
                style,
                rect.left, rect.top, rect.Width, rect.Height,
                IntPtr.Zero, 
                IntPtr.Zero, 
                instance, 
                IntPtr.Zero);

            if (hwnd == IntPtr.Zero)
                throw new Exception(String.Format("Failed to create window. Error: {0}", Marshal.GetLastWin32Error()));

            Functions.GetClientRect(hwnd, out clientRectangle);

            input = new Input(GetClientRect);
        }

        internal void OnNewFrame()
        {
            input.OnNewFrame();
        }
    }
}
