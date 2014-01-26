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

using System.Collections.Generic;
using System.Windows.Forms;
using Beholder.Input;
using Beholder.Math;
using BMouseButtons = Beholder.Input.MouseButtons;

namespace Beholder.Libraries.Windows.Forms.Input
{
    public class Mouse : IMouseHandler
    {
        readonly Control control;
        readonly Queue<MouseEventInfo> events;

        bool visible = true;

        public Mouse(Control control)
        {
            this.control = control;

            events = new Queue<MouseEventInfo>();

            control.MouseClick += OnMouseClick;
            control.MouseDoubleClick += OnMouseDoubleClick;
            control.MouseDown += OnMouseDown;
            control.MouseMove += OnMouseMove;
            control.MouseUp += OnMouseUp;
            control.MouseWheel += OnMouseWheel;
        }

        public bool Visible
        {
            get { return visible; }
            set
            {
                if (visible == value) return;

                visible = value;
                if (visible)
                {
                    Cursor.Show();
                }
                else
                {
                    Cursor.Hide();
                }
            }
        }

        public void GetWindowPosition(out IntVector2 pos)
        {
            var point = control.PointToClient(Cursor.Position);

            pos.X = point.X;
            pos.Y = point.Y;
        }

        public void SetWindowPosition(int x, int y)
        {
            Cursor.Position = new System.Drawing.Point { X = x, Y = y };
        }

        public BMouseButtons ButtonsDown
        {
            get
            {
                return (BMouseButtons)((int)Control.MouseButtons >> 20);
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

        void OnMouseClick(object sender, MouseEventArgs e)
        {
            if (!control.ContainsFocus) return;

            events.Enqueue(new MouseEventInfo
            {
                EventType = MouseEventType.Click,
                Buttons = (BMouseButtons)e.Button,
                X = (ushort)e.X,
                Y = (ushort)e.Y,
                Delta = (short)e.Delta
            });
        }

        void OnMouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (!control.ContainsFocus) return;

            events.Enqueue(new MouseEventInfo
            {
                EventType = MouseEventType.DoubleClick,
                Buttons = (BMouseButtons)e.Button,
                X = (ushort)e.X,
                Y = (ushort)e.Y,
                Delta = (short)e.Delta
            });
        }

        void OnMouseDown(object sender, MouseEventArgs e)
        {
            if (!control.ContainsFocus) return;

            events.Enqueue(new MouseEventInfo
            {
                EventType = MouseEventType.Down,
                Buttons = (BMouseButtons)e.Button,
                X = (ushort)e.X,
                Y = (ushort)e.Y,
                Delta = (short)e.Delta
            });
        }

        void OnMouseMove(object sender, MouseEventArgs e)
        {
            if (!control.ContainsFocus) return;

            events.Enqueue(new MouseEventInfo
            {
                EventType = MouseEventType.Move,
                Buttons = (BMouseButtons)e.Button,
                X = (ushort)e.X,
                Y = (ushort)e.Y,
                Delta = (short)e.Delta
            });
        }

        void OnMouseUp(object sender, MouseEventArgs e)
        {
            if (!control.ContainsFocus) return;

            events.Enqueue(new MouseEventInfo
            {
                EventType = MouseEventType.Up,
                Buttons = (BMouseButtons)e.Button,
                X = (ushort)e.X,
                Y = (ushort)e.Y,
                Delta = (short)e.Delta
            });
        }

        void OnMouseWheel(object sender, MouseEventArgs e)
        {
            if (!control.ContainsFocus) return;

            events.Enqueue(new MouseEventInfo
            {
                EventType = MouseEventType.Wheel,
                Buttons = (BMouseButtons)e.Button,
                X = (ushort)e.X,
                Y = (ushort)e.Y,
                Delta = (short)e.Delta
            });
        }
    }
}
