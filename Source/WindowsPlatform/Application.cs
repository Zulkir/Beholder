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
using Beholder.Platform;
using Win32;

namespace WindowsPlatform
{
    public class Application
    {
        readonly Timer timer;
        GameWindow gameWindow;

        public event Action<IRealTime> NewFrame;

        public Application()
        {
            timer = new Timer();
        }

        public GameWindow CreateGameWindow(int width, int height, string title, IntPtr iconHandle, IntPtr smallIconHandle)
        {
            return CreateGameWindow(int.MinValue, int.MinValue, width, height, title, iconHandle, smallIconHandle);
        }

        public GameWindow CreateGameWindow(int x, int y, int width, int height, string title, IntPtr iconHandle, IntPtr smallIconHandle)
        {
            gameWindow = new GameWindow(x, y, width, height, title, iconHandle, smallIconHandle, () => 
                {
                    gameWindow.OnNewFrame();
                    timer.UpdateTime();

                    if (NewFrame != null)
                    {
                        NewFrame(timer.RealTime);
                    }
                });
            return gameWindow;
        }

        public int Run()
        {
            Functions.ShowWindow(gameWindow.Handle, SW.SHOW);
            Functions.UpdateWindow(gameWindow.Handle);

            timer.Start();

            MSG msg = new MSG();

            while (msg.Message != WM.QUIT)
            {
                if (Functions.PeekMessage(out msg, IntPtr.Zero, 0, 0, PM.REMOVE))
                {
                    //messages++;
                    //Functions.SetWindowText(gameWindow.Handle, string.Format("Message #{0}: {1}", messages.ToString(), msg.Message.ToString()));

                    Functions.TranslateMessage(ref msg);
                    Functions.DispatchMessage(ref msg);
                }
                else 
                {
                    gameWindow.OnNewFrame();
                    timer.UpdateTime();

                    if (NewFrame != null)
                    {
                        NewFrame(timer.RealTime);
                    }
                }
                
            }

            return msg.WParam.ToInt32();
        }

        public void Exit()
        {
            Functions.PostQuitMessage(0);
        }
    }
}
