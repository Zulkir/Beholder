﻿/*
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

using System.Drawing;
using System.Windows.Forms;

namespace Beholder.Libraries.Windows.Forms
{
    class WindowStateBuffer
    {
        FormWindowState windowState;
        Size clientSize;
        int left;
        int top;
        bool topMost;
        FormBorderStyle borderStyle;

        public void Read(Form form)
        {
            windowState = form.WindowState;

            form.WindowState = FormWindowState.Normal;

            clientSize = form.ClientSize;
            left = form.Left;
            top = form.Top;
            topMost = form.TopMost;
            borderStyle = form.FormBorderStyle;

            form.WindowState = windowState;
        }

        public void Apply(Form form)
        {
            form.WindowState = FormWindowState.Normal;

            form.ClientSize = clientSize;
            form.Left = left;
            form.Top = top;
            form.TopMost = topMost;
            form.FormBorderStyle = borderStyle;

            form.WindowState = windowState;
        }
    }
}
