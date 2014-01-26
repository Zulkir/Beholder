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
using System.Diagnostics;
using System.Windows.Forms;
using Beholder.Platform;
using Win32;

namespace Beholder.Libraries.Windows.Forms
{
    public class GameLoop
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="mainForm">Can be null</param>
        /// <param name="onNewFrame"></param>
        public GameLoop(Form mainForm, Action<RealTime> onNewFrame)
        {
            this.mainForm = mainForm;
            this.onNewFrame = onNewFrame;

            realTime = new RealTime();

            startTicks = Stopwatch.GetTimestamp();

            Application.Idle += OnIdle;
        }

        #region Loop and Time
        readonly RealTime realTime;
        readonly long startTicks;
        readonly Action<RealTime> onNewFrame;

        long totalTicks;
        long frame;
        long ticksSinceLastFpsUpdate;
        long lastFpsUpdateFrame;

        static float TicksToSeconds(long ticks)
        {
            return (float)((double)ticks / Stopwatch.Frequency);
        }

        void UpdateTime()
        {
            long ticks = Stopwatch.GetTimestamp() - startTicks;
            long elapsedTics = ticks - totalTicks;
            totalTicks = ticks;

            realTime.TotalRealTime = TicksToSeconds(totalTicks);
            realTime.ElapsedRealTime = TicksToSeconds(elapsedTics);

            ticksSinceLastFpsUpdate += elapsedTics;

            if (ticksSinceLastFpsUpdate >= Stopwatch.Frequency / 4)
            {
                realTime.FramesPerSecond = (frame - lastFpsUpdateFrame) / TicksToSeconds(ticksSinceLastFpsUpdate);

                lastFpsUpdateFrame = frame;
                ticksSinceLastFpsUpdate = 0;
            }

            frame++;
        }

        static bool AppStillIdle
        {
            get
            {
                MSG msg;
                return !Functions.PeekMessage(out msg, IntPtr.Zero, 0, 0, (PM)0);
            }
        }

        void OnIdle(object sender, EventArgs e)
        {
            while (AppStillIdle)
            {
                UpdateTime();

                onNewFrame(realTime);
            }
        }
        #endregion

        #region Birth and Death

        readonly Form mainForm;

        public void Run()
        {
            if (mainForm == null)
                Application.Run();
            else
                Application.Run(mainForm);
        }
        
        public void Exit()
        {
            Application.Exit();
        }

        #endregion
    }
}
