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

using System;
using System.Text;
using System.Windows.Forms;
using Beholder.Input;
using Win32;
using Keys = Beholder.Input.Keys;

namespace Beholder.Libraries.Windows.Forms.Input
{
    public class Keyboard : IKeyboardHandler
    {
        readonly Control control;

        bool[] downPrev = new bool[256];
        bool[] downCurr = new bool[256];
        readonly bool[] toggled = new bool[256];
        readonly bool[] masked = new bool[256];

        readonly Keys[] allKeys = (Keys[])Enum.GetValues(typeof(Keys));

        readonly StringBuilder stringBuilder = new StringBuilder();
        bool textInput;
        
        public string Text { get { return stringBuilder.ToString(); } }

        public Keyboard(Control control)
        {
            this.control = control;
            control.KeyPress += OnKeyPress;
        }

        void OnKeyPress(object sender, KeyPressEventArgs e)
        {
            OnChar(e.KeyChar);
        }

        public void OnNewFrame()
        {
            Array.Clear(masked, 0, 256);

            var t = downPrev;
            downPrev = downCurr;
            downCurr = t;

            foreach (Keys key in allKeys)
            {
                short nativeState = control.ContainsFocus ? Functions.GetKeyState((int)key) : (short)0;

                //If the high-order bit is 1, the key is down
                //otherwise, it is up.

                downCurr[(int)key] = ((nativeState & 0x8000) == 0x8000);
                toggled[(int)key] = ((nativeState & 1) == 1);
            }
        }

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
            if (textInput) throw new InvalidOperationException("Text Input is already on.");

            textInput = true;
        }

        public void ResetText()
        {
            stringBuilder.Clear();
        }

        public void EndTextMode()
        {
            if (!textInput) throw new InvalidOperationException("Trying to end Text Input that is not on.");

            textInput = false;
        }

        void OnChar(char ch)
        {
            if (!textInput) return;

            if (ch == '\b')
            {
                if (stringBuilder.Length > 0)
                {
                    stringBuilder.Remove(stringBuilder.Length - 1, 1);
                }
            }
            else
            {
                if (ch == '\r')
                {
                    ch = '\n';
                }

                stringBuilder.Append(ch);
            }
        }

        public string CompositionString
        {
            get { throw new NotImplementedException(); }
        }
    }
}
