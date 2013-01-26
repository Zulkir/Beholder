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
using System.Windows.Forms;
using Beholder.Input;
using Beholder.Libraries.Windows.Forms.Input;
using Beholder.Platform;
using Beholder.Utility;

namespace Beholder.Libraries.Windows.Forms
{
    public class ControlWindow : ICWindow, IWindowHandle
    {
        readonly Control control;
        readonly Form parentForm;

        readonly InputHandler input;
        public IInputHandler Input { get { return input; } }

        public Control Control { get { return control; } }
        public ISwapChain SwapChain { get; private set; }
        public int SwapChainWidth { get; private set; }
        public int SwapChainHeight { get; private set; }
        public FullscreenState FullscreenState { get; private set; }
        public bool IsVisible { get { return !Closed && !minimizedWhileFullscreen && control.ClientSize.Width * control.ClientSize.Height > 0; } }
        public bool Closed { get; private set; }
        public bool ContainsFocus { get { return parentForm.ContainsFocus; } }

        readonly WindowStateBuffer windowedState;

        Action onLostFocusInFullscreen;
        Action onAltTabbingBackToFullscreen;
        FullscreenState11 fullscreenState11;

        bool minimizedWhileFullscreen;
        int numOnModeChangeMethodsCurrentlyInside;
        
        public IntPtr Handle { get { return control.Handle; } }

        public ControlWindow(Control control)
        {
            this.control = control;
            input = new InputHandler(control);

            windowedState = new WindowStateBuffer();

            parentForm = control.FindForm();

            if (parentForm == null) throw new ShouldNeverHappenException("A given control somehow has no parent form. (Poor orphan control T_T)");

            parentForm.Resize += OnResize;
            parentForm.GotFocus += OnGotFocus;
            parentForm.LostFocus += OnLostFocus;
            parentForm.FormClosed += OnFormClosed;
            parentForm.Deactivate += OnDeactivate;

            SwapChainWidth = control.ClientSize.Width;
            SwapChainHeight = control.ClientSize.Height;
        }

        public void SetSwapChain(ISwapChain swapChain)
        {
            SwapChain = swapChain;
        }

        public event FormClosingEventHandler FormClosing
        {
            add { parentForm.FormClosing += value; }
            remove { parentForm.FormClosing -= value; }
        }

        public void OnNewFrame()
        {
            input.OnNewFrame();
        }

        void OnFullscreenStateChangeBegin()
        {
            numOnModeChangeMethodsCurrentlyInside++;
        }

        void OnFullscreenStateChangeEnd(FullscreenState state)
        {
            FullscreenState = state;
            fullscreenState11 = FullscreenState == FullscreenState.Fullscreen
                                    ? FullscreenState11.Fullscreen
                                    : FullscreenState11.Windowed;
            minimizedWhileFullscreen = false;
            numOnModeChangeMethodsCurrentlyInside--;
        }

        public void FixOutOfFullscreenBug11()
        {
            windowedState.Apply(parentForm);
        }

        public void OnWindowed()
        {
            if (FullscreenState == FullscreenState.Windowed) return;

            OnFullscreenStateChangeBegin();
            {
                windowedState.Apply(parentForm);

                SwapChainWidth = control.ClientSize.Width;
                SwapChainHeight = control.ClientSize.Height;
            }
            OnFullscreenStateChangeEnd(FullscreenState.Windowed);
        }

        public void OnPseudoFullscreen()
        {
            if (FullscreenState == FullscreenState.PseudoFullscreen) return;

            OnFullscreenStateChangeBegin();
            {
                OnWindowed();

                windowedState.Read(parentForm);

                FullscreenState = FullscreenState.PseudoFullscreen;

                parentForm.FormBorderStyle = FormBorderStyle.None;
                parentForm.TopMost = true;
                parentForm.WindowState = FormWindowState.Normal;
                parentForm.WindowState = FormWindowState.Maximized;
                parentForm.TopMost = false;

                SwapChainWidth = control.ClientSize.Width;
                SwapChainHeight = control.ClientSize.Height;
            }
            OnFullscreenStateChangeEnd(FullscreenState.PseudoFullscreen);
        }

        public void OnFullscreen(int width, int height, Action onLostFocusInFullscreen = null, Action onAltTabbingBackToFullscreen = null)
        {
            if (FullscreenState == FullscreenState.Fullscreen && SwapChainWidth == width && SwapChainHeight == height) return;

            this.onLostFocusInFullscreen = onLostFocusInFullscreen;
            this.onAltTabbingBackToFullscreen = onAltTabbingBackToFullscreen;

            OnFullscreenStateChangeBegin();
            {
                OnWindowed();

                windowedState.Read(parentForm);

                SwapChainWidth = width;
                SwapChainHeight = height;

                parentForm.WindowState = FormWindowState.Normal;
                parentForm.FormBorderStyle = FormBorderStyle.None;
            }
            OnFullscreenStateChangeEnd(FullscreenState.Fullscreen);
        }

        void OnResize(object sender, EventArgs e)
        {
            if (numOnModeChangeMethodsCurrentlyInside > 0) return;

            if (FullscreenState == FullscreenState.Windowed)
            {
                if (control.ClientSize.Width * control.ClientSize.Height > 0)
                {
                    SwapChainWidth = control.ClientSize.Width;
                    SwapChainHeight = control.ClientSize.Height;
                }
            }
        }

        void OnGotFocus(object sender, EventArgs e)
        {
            if (fullscreenState11 == FullscreenState11.BrokenFullscreen)
            {
                if (onAltTabbingBackToFullscreen != null) onAltTabbingBackToFullscreen();
                fullscreenState11 = FullscreenState11.Fullscreen;
            }
        }

        void OnLostFocus(object sender, EventArgs e)
        {
            if (fullscreenState11 == FullscreenState11.Fullscreen)
            {
                if (onLostFocusInFullscreen != null) onLostFocusInFullscreen();
            }
        }

        void OnDeactivate(object sender, EventArgs e)
        {
        }

        public void OnFullscreenBroken11()
        {
            if (fullscreenState11 == FullscreenState11.Fullscreen)
            {
                fullscreenState11 = FullscreenState11.BrokenFullscreen;
                parentForm.WindowState = FormWindowState.Minimized;
            }
        }

        void OnFormClosed(object sender, FormClosedEventArgs e)
        {
            Closed = true;
        }
    }
}
