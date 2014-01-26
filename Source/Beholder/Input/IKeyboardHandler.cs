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

namespace Beholder.Input
{
    public interface IKeyboardHandler
    {
        bool KeyDown(Keys key);
        bool KeyToggled(Keys key);
        bool KeyPressed(Keys key);
        bool KeyReleased(Keys key);

        void BeginTextMode();
        void EndTextMode();
        void ResetText();
        string Text { get; }

        /// <summary>
        /// Masks a set of keys for the duration of one frame. Masked keys are not marked as down, pressed or released.
        /// </summary>
        /// <param name="keys">Keys to mask. This field can not be null.</param>
        void MaskKeys(params Keys[] keys);
    }
}
