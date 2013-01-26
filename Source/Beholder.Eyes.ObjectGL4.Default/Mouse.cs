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
using System.Drawing;
using System.Windows.Forms;
using Beholder.Input;
using Beholder.Math;
using OpenTK;
using OpenTK.Input;
using MouseButtons = Beholder.Input.MouseButtons;

namespace Beholder.Eyes.ObjectGL4.Default
{
    class Mouse : IMouseHandler
    {
        const int NumButtons = 5;

        readonly GameWindow gameWindow;
        readonly Queue<MouseEventInfo> events;

        int lastX, lastY;
        int lastWheel;
        bool[] downPrev;
        bool[] downCurr;
        readonly float[] timeSincePressed;
        readonly float[] timeSinceReleased;
        const float MaxClickSeconds = 0.4f;
        const float MaxDoubleClickSeconds = 1.0f;

        bool visible = true;

        public Mouse(GameWindow gameWindow)
        {
            this.gameWindow = gameWindow;

            events = new Queue<MouseEventInfo>();

            if (gameWindow.Mouse != null)
            {
                lastX = gameWindow.Mouse.X;
                lastY = gameWindow.Mouse.Y;
                lastWheel = gameWindow.Mouse.Wheel;
            }

            downPrev = new bool[NumButtons];
            downCurr = new bool[NumButtons];
            timeSincePressed = new float[NumButtons];
            timeSinceReleased = new float[NumButtons];
        }

        public bool Visible
        {
            get { return visible; }
            set
            {
                if (visible == value) return;
                visible = value;
                gameWindow.CursorVisible = visible;
            }
        }

        public void GetWindowPosition(out IntVector2 pos)
        {
            if (gameWindow.Mouse != null)
            {
                pos.X = gameWindow.Mouse.X;
                pos.Y = gameWindow.Mouse.Y;
            }
            else
            {
                pos = new IntVector2();
            }
        }

        public void SetWindowPosition(int x, int y)
        {
            Cursor.Position = new Point { X = x, Y = y };
        }

        public MouseButtons ButtonsDown
        {
            get
            {
                MouseButtons result = 0;
                if (downCurr[(int)MouseButton.Left]) result |= MouseButtons.Left;
                if (downCurr[(int)MouseButton.Right]) result |= MouseButtons.Right;
                if (downCurr[(int)MouseButton.Middle]) result |= MouseButtons.Middle;
                if (downCurr[(int)MouseButton.Button1]) result |= MouseButtons.XButton1;
                if (downCurr[(int)MouseButton.Button2]) result |= MouseButtons.XButton2;

                return result;
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

        public void OnNewFrame(float elapsedSeconds)
        {
            var t = downPrev;
            downPrev = downCurr;
            downCurr = t;

            if (gameWindow.Mouse != null)
            {
                if (gameWindow.Mouse.X != lastX || gameWindow.Mouse.Y != lastY)
                    EnqueueEvent(MouseButtons.None, MouseEventType.Move);

                lastX = gameWindow.Mouse.X;
                lastY = gameWindow.Mouse.Y;

                if (gameWindow.Mouse.Wheel != lastWheel)
                    EnqueueEvent(MouseButtons.None, MouseEventType.Wheel, gameWindow.Mouse.Wheel - lastWheel);

                lastWheel = gameWindow.Mouse.Wheel;

                for (int index = 0; index < NumButtons; index++)
                {
                    var button = ButtonFromIndex(index);

                    timeSincePressed[index] += elapsedSeconds;
                    timeSinceReleased[index] += elapsedSeconds;

                    bool isButtonDown = gameWindow.Focused && gameWindow.Mouse[ToMouseButton(button)];
                    downCurr[index] = isButtonDown;
                    bool isButtonPressed = isButtonDown && !downPrev[index];
                    bool isButtonReleased = !isButtonDown && downPrev[index];

                    if (isButtonReleased && timeSincePressed[index] < MaxClickSeconds)
                    {
                        EnqueueEvent(button, MouseEventType.Click);

                        if (timeSinceReleased[index] < MaxDoubleClickSeconds)
                            EnqueueEvent(button, MouseEventType.DoubleClick);
                    }

                    if (isButtonPressed)
                    {
                        timeSincePressed[index] = 0.0f;
                        EnqueueEvent(button, MouseEventType.Down);
                    }

                    if (isButtonReleased)
                    {
                        timeSinceReleased[index] = 0.0f;
                        EnqueueEvent(button, MouseEventType.Up);
                    }
                }
            }
        }

        void EnqueueEvent(MouseButtons button, MouseEventType mouseEventType, int delta = 0)
        {
            events.Enqueue(new MouseEventInfo
            {
                EventType = mouseEventType,
                Buttons = button,
                Delta = delta,
                X = gameWindow.Mouse.X,
                Y = gameWindow.Mouse.Y
            });
        }

        static MouseButton ToMouseButton(MouseButtons bButton)
        {
            switch (bButton)
            {
                case MouseButtons.None: return 0;
                case MouseButtons.Left: return MouseButton.Left;
                case MouseButtons.Right: return MouseButton.Right;
                case MouseButtons.Middle: return MouseButton.Middle;
                case MouseButtons.XButton1: return MouseButton.Button1;
                case MouseButtons.XButton2: return MouseButton.Button2;
                default: throw new ArgumentOutOfRangeException("bButton");
            }
        }

        static MouseButtons ButtonFromIndex(int index)
        {
            switch (index)
            {
                case 0: return MouseButtons.Left;
                case 1: return MouseButtons.Right;
                case 2: return MouseButtons.Middle;
                case 3: return MouseButtons.XButton1;
                case 4: return MouseButtons.XButton2;
                default: throw new ArgumentOutOfRangeException("index");
            }
        }
    }
}
