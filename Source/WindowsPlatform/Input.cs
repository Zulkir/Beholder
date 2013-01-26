/*
Copyright (c) 2010-2013 Beholder Project - Daniil Rodin

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

using System;
using System.Collections.Generic;
using System.Text;
using Beholder.Input;
using Beholder.Math;
using Beholder.Utility.Delegates;
using Win32;


namespace WindowsPlatform
{
    public class Input : IInputHandler, IKeyboardHandler, IMouseHandler
    {
        readonly FuncOut<RECT> getClientRect;
        readonly Queue<MouseEventInfo> events;

        bool[] downPrev = new bool[256];
        bool[] downCurr = new bool[256];
        readonly bool[] toggled = new bool[256];
        readonly bool[] masked = new bool[256];

        readonly VK[] allKeys = (VK[])Enum.GetValues(typeof(VK));

        bool focused;

        bool textMode;
        readonly StringBuilder stringBuilder = new StringBuilder();
        
        bool cursorVisible = true;

        CURSORINFO tempCursorInfo;
        RECT tempRect;

        public bool HasFocus { get { return focused; } }

        internal Input(FuncOut<RECT> getClientRect)
        {
            this.getClientRect = getClientRect;

            events = new Queue<MouseEventInfo>();
        }

        public IKeyboardHandler Keyboard { get { return this; } }
        public IMouseHandler Mouse { get { return this; } }

        public string Text { get { return stringBuilder.ToString(); } }

        public bool KeyDown(Keys key)
        {
            return downCurr[(int)key] && !masked[(int)key];
        }

        public bool KeyPressed(Keys key)
        {
            return downCurr[(int)key] && !downPrev[(int)key] && !masked[(int)key];
        }

        public bool KeyReleased(Keys key)
        {
            return !downCurr[(int)key] && downPrev[(int)key] && !masked[(int)key];
        }

        public bool KeyToggled(Keys key)
        {
            return toggled[(int)key];
        }

        public void MaskKeys(params Keys[] keys)
        {
            foreach (Keys key in keys)
            {
                masked[(int)key] = true;
            }
        }

        public void BeginTextMode()
        {
            if (textMode) throw new InvalidOperationException("Text Input is already on.");

            textMode = true;
        }

        public void ResetText()
        {
            stringBuilder.Clear();
        }

        public void EndTextMode()
        {
            if (!textMode) throw new InvalidOperationException("Trying to end Text Input that is not on.");

            textMode = false;
        }


        bool CursorInside
        {
            get
            {
                getClientRect(out tempRect);
                Functions.GetCursorInfo(out tempCursorInfo);

                return
                    tempRect.left <= tempCursorInfo.ptScreenPos.X && tempCursorInfo.ptScreenPos.X < tempRect.right &&
                    tempRect.top <= tempCursorInfo.ptScreenPos.Y && tempCursorInfo.ptScreenPos.Y < tempRect.bottom;
            }
        }
        public bool TryDequeueEvent(out MouseEventInfo eventInfo)
        {
            if (events.Count > 0)
            {
                eventInfo = events.Dequeue();
                return true;
            }

            eventInfo = new MouseEventInfo();
            return false;
        }
        public bool Visible
        {
            get { return cursorVisible; }
            set
            {
                if (cursorVisible == value) return;

                cursorVisible = value;
                Functions.ShowCursor(cursorVisible);
            }
        }

        public MouseButtons ButtonsDown
        {
            get
            {
                if (!focused || !CursorInside) return MouseButtons.None;

                MouseButtons buttons = MouseButtons.None;

                if (Functions.GetSystemMetrics(SM.SWAPBUTTON) == 0)
                {
                    if (downCurr[(int)VK.LBUTTON]) buttons |= MouseButtons.Left;
                    if (downCurr[(int)VK.RBUTTON]) buttons |= MouseButtons.Right;
                }
                else
                {
                    if (downCurr[(int)VK.RBUTTON]) buttons |= MouseButtons.Left;
                    if (downCurr[(int)VK.LBUTTON]) buttons |= MouseButtons.Right;
                }

                if (downCurr[(int)VK.MBUTTON]) buttons |= MouseButtons.Middle;
                if (downCurr[(int)VK.XBUTTON1]) buttons |= MouseButtons.XButton1;
                if (downCurr[(int)VK.XBUTTON2]) buttons |= MouseButtons.XButton2;

                return buttons;
            }
        }

        public void GetWindowPosition(out IntVector2 pos)
        {
            getClientRect(out tempRect);
            Functions.GetCursorInfo(out tempCursorInfo);

            pos.X = tempCursorInfo.ptScreenPos.X - tempRect.left;
            pos.Y = tempCursorInfo.ptScreenPos.Y - tempRect.top;
        }

        public void SetWindowPosition(int x, int y)
        {
            getClientRect(out tempRect);

            Functions.SetCursorPos(tempRect.left + x, tempRect.top + y);
        }


        internal void OnGotFocus()
        {
            focused = true;
        }

        internal void OnLostFocus()
        {
            focused = false;
        }

        internal void OnNewFrame()
        {
            Array.Clear(masked, 0, masked.Length);

            var t = downPrev;
            downPrev = downCurr;
            downCurr = t;

            if (focused)
            {
                foreach (Keys key in allKeys)
                {
                    short nativeState = Functions.GetAsyncKeyState((VK)key);

                    //If the high-order bit is 1, the key is down
                    //otherwise, it is up.

                    downCurr[(int)key] = ((nativeState & 0x8000) == 0x8000);
                    toggled[(int)key] = ((nativeState & 1) == 1);
                }
            }
            else
            {
                Array.Clear(downCurr, 0, downCurr.Length);
            }
        }

        internal void OnMouseEvent(MouseEventInfo e)
        {
            if (!focused || !CursorInside) return;

            events.Enqueue(e);
        }
    }
}
