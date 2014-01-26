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
using Beholder.Input;

namespace Beholder.Validation.Input
{
    class KeyboardHandler : Wrapper<IKeyboardHandler>, IKeyboardHandler
    {
        bool textMode;

        public KeyboardHandler(IKeyboardHandler real) : base(real) { }

        public bool KeyDown(Keys key) { return Real.KeyDown(key); }
        public bool KeyToggled(Keys key) { return Real.KeyToggled(key); }
        public bool KeyPressed(Keys key) { return Real.KeyPressed(key); }
        public bool KeyReleased(Keys key) { return Real.KeyReleased(key); }
        public void ResetText() { Real.ResetText(); }
        public string Text { get { return Real.Text; } }

        public void BeginTextMode() 
        {
            if (textMode) 
                throw new InvalidOperationException("Calling IKeyboard.BeginTextMode() while already in Text Mode.");
            textMode = true;
            Real.BeginTextMode();
        }

        public void EndTextMode() 
        {
            if (!textMode) 
                throw new InvalidOperationException("Calling IKeyboard.EndTextMode() without calling IKeyboard.BeginTextMode() first.");
            textMode = false;
            Real.EndTextMode(); 
        }

        public void MaskKeys(params Keys[] keys) 
        {
            if (keys == null) 
                throw new ArgumentNullException("keys");
            Real.MaskKeys(keys); 
        }
    }
}
