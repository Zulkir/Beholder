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
using System.Text;
using Beholder.Input;
using OpenTK;
using OpenTK.Input;

namespace Beholder.Eyes.ObjectGL4.Default
{
    class Keyboard : IKeyboardHandler
    {
        readonly GameWindow gameWindow;

        bool[] downPrev = new bool[256];
        bool[] downCurr = new bool[256];
        readonly bool[] toggled = new bool[256];
        readonly bool[] masked = new bool[256];

        readonly Keys[] allKeys = (Keys[])Enum.GetValues(typeof(Keys));

        readonly StringBuilder stringBuilder = new StringBuilder();
        bool textInput;

        public string Text { get { return stringBuilder.ToString(); } }

        public Keyboard(GameWindow gameWindow)
        {
            this.gameWindow = gameWindow;
            gameWindow.KeyPress += OnKeyPress;
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

            if (gameWindow.Keyboard != null)
            {
                foreach (Keys key in allKeys)
                {
                    bool nativeState = gameWindow.Focused && IsKeyDown(key);

                    downCurr[(int)key] = nativeState;
                    toggled[(int)key] = false;
                }
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
                    stringBuilder.Remove(stringBuilder.Length - 1, 1);
            }
            else
            {
                if (ch == '\r')
                    ch = '\n';
                stringBuilder.Append(ch);
            }
        }

        public string CompositionString
        {
            get { throw new NotImplementedException(); }
        }

        bool IsKeyDown(Keys bKey)
        {
            switch (bKey)
            {
                case Keys.None: return false;
                case Keys.Cancel: return false;
                case Keys.Back: return gameWindow.Keyboard[Key.Back];
                case Keys.Tab: return gameWindow.Keyboard[Key.Tab];
                case Keys.Clear: return gameWindow.Keyboard[Key.Clear];
                case Keys.Enter: return gameWindow.Keyboard[Key.Enter];
                case Keys.ShiftKey: return gameWindow.Keyboard[Key.ShiftLeft] || gameWindow.Keyboard[Key.ShiftRight];
                case Keys.ControlKey: return gameWindow.Keyboard[Key.ControlLeft] || gameWindow.Keyboard[Key.ControlRight];
                case Keys.Menu: return gameWindow.Keyboard[Key.AltLeft] || gameWindow.Keyboard[Key.AltRight];
                case Keys.Pause: return gameWindow.Keyboard[Key.Pause];
                case Keys.CapsLock: return gameWindow.Keyboard[Key.CapsLock];
                case Keys.KanaMode:
                case Keys.JunjaMode:
                case Keys.FinalMode:
                case Keys.KanjiMode: return false;
                case Keys.Escape: return gameWindow.Keyboard[Key.Escape];
                case Keys.IMEConvert:
                case Keys.IMENonconvert:
                case Keys.IMEAccept:
                case Keys.IMEModeChange: return false;
                case Keys.Space: return gameWindow.Keyboard[Key.Space];
                case Keys.PageUp: return gameWindow.Keyboard[Key.PageUp];
                case Keys.PageDown: return gameWindow.Keyboard[Key.PageDown];
                case Keys.End: return gameWindow.Keyboard[Key.End];
                case Keys.Home: return gameWindow.Keyboard[Key.Home];
                case Keys.Left: return gameWindow.Keyboard[Key.Left];
                case Keys.Up: return gameWindow.Keyboard[Key.Up];
                case Keys.Right: return gameWindow.Keyboard[Key.Right];
                case Keys.Down: return gameWindow.Keyboard[Key.Down];
                case Keys.Select: return false;
                case Keys.Print: return false;
                case Keys.Execute: return false;
                case Keys.PrintScreen: return gameWindow.Keyboard[Key.PrintScreen];
                case Keys.Insert: return gameWindow.Keyboard[Key.Insert];
                case Keys.Delete: return gameWindow.Keyboard[Key.Delete];
                case Keys.Help: return false;
                case Keys.LWin: return gameWindow.Keyboard[Key.LWin];
                case Keys.RWin: return gameWindow.Keyboard[Key.RWin];
                case Keys.Apps: return gameWindow.Keyboard[Key.Menu];
                case Keys.Sleep: return gameWindow.Keyboard[Key.Sleep];
                case Keys.Multiply: return gameWindow.Keyboard[Key.KeypadMultiply];
                case Keys.Add: return gameWindow.Keyboard[Key.KeypadAdd];
                case Keys.Separator: return false;
                case Keys.Subtract: return gameWindow.Keyboard[Key.KeypadSubtract];
                case Keys.Decimal: return gameWindow.Keyboard[Key.KeypadDecimal];
                case Keys.Divide: return gameWindow.Keyboard[Key.KeypadDivide];
                case Keys.NumLock: return gameWindow.Keyboard[Key.NumLock];
                case Keys.Scroll: return gameWindow.Keyboard[Key.ScrollLock];
                case Keys.OemFjJisho:
                case Keys.OemFjMasshou:
                case Keys.OemFjTouroku:
                case Keys.OemFjLOya:
                case Keys.OemFjROya: return false;
                case Keys.LShiftKey: return gameWindow.Keyboard[Key.LShift];
                case Keys.RShiftKey: return gameWindow.Keyboard[Key.RShift];
                case Keys.LControlKey: return gameWindow.Keyboard[Key.LControl];
                case Keys.RControlKey: return gameWindow.Keyboard[Key.RControl];
                case Keys.LMenu: return gameWindow.Keyboard[Key.LAlt];
                case Keys.RMenu: return gameWindow.Keyboard[Key.RAlt];
                case Keys.BrowserBack:
                case Keys.BrowserForward:
                case Keys.BrowserRefresh:
                case Keys.BrowserStop:
                case Keys.BrowserSearch:
                case Keys.BrowserFavorites:
                case Keys.BrowserHome:
                case Keys.VolumeMute:
                case Keys.VolumeDown:
                case Keys.VolumeUp:
                case Keys.MediaNextTrack:
                case Keys.MediaPreviousTrack:
                case Keys.MediaStop:
                case Keys.MediaPlayPause:
                case Keys.LaunchMail:
                case Keys.SelectMedia:
                case Keys.LaunchApplication1:
                case Keys.LaunchApplication2: return false;
                case Keys.OemSemicolon: return gameWindow.Keyboard[Key.Semicolon];
                case Keys.Oemplus: return gameWindow.Keyboard[Key.Plus];
                case Keys.OemComma: return gameWindow.Keyboard[Key.Comma];
                case Keys.OemMinus: return gameWindow.Keyboard[Key.Minus];
                case Keys.OemPeriod: return gameWindow.Keyboard[Key.Period];
                case Keys.OemQuestion: return gameWindow.Keyboard[Key.Slash];
                case Keys.OemTilde: return gameWindow.Keyboard[Key.Tilde];
                case Keys.OemOpenBrackets: return gameWindow.Keyboard[Key.LBracket];
                case Keys.OemPipe: return gameWindow.Keyboard[Key.BackSlash];
                case Keys.OemCloseBrackets: return gameWindow.Keyboard[Key.RBracket];
                case Keys.OemQuotes: return gameWindow.Keyboard[Key.Quote];
                case Keys.Oem8: return false;
                case Keys.OemBackslash: return gameWindow.Keyboard[Key.BackSlash];
                case Keys.ProcessKey:
                case Keys.Packet:
                case Keys.Attn:
                case Keys.Crsel:
                case Keys.Exsel:
                case Keys.EraseEof:
                case Keys.Play:
                case Keys.Zoom:
                case Keys.NoName:
                case Keys.Pa1: return false;
                case Keys.OemClear: return gameWindow.Keyboard[Key.Clear];
            }

            if (bKey >= Keys.D0 && bKey <= Keys.D9)
                return gameWindow.Keyboard[Key.Number0 - (int)Keys.D0 + (int)bKey];

            if (bKey >= Keys.A && bKey <= Keys.Z)
                return gameWindow.Keyboard[Key.A - (int)Keys.A + (int)bKey];

            if (bKey >= Keys.NumPad0 && bKey <= Keys.NumPad9)
                return gameWindow.Keyboard[Key.Keypad0 - (int)Keys.NumPad0 + (int)bKey];

            if (bKey >= Keys.F1 && bKey <= Keys.F24)
                return gameWindow.Keyboard[Key.F1 - (int)Keys.F1 + (int)bKey];

            throw new ArgumentOutOfRangeException("bKey");
        }
    }
}
