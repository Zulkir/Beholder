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

using System.Diagnostics;
using Beholder.Platform;

namespace WindowsPlatform
{
    class Timer
    {
        readonly RealTime realTime;
        long startTicks;

        long totalTicks;
        long frame;
        long ticksSinceLastFpsUpdate;
        long lastFpsUpdateFrame;

        public RealTime RealTime { get { return realTime; } }

        public Timer()
        {
            realTime = new RealTime();
        }

        public void Start()
        {
            startTicks = Stopwatch.GetTimestamp();
        }

        static float TicksToSeconds(long ticks)
        {
            return (float)((double)ticks / Stopwatch.Frequency);
        }

        public void UpdateTime()
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
    }
}
