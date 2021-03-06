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

using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Beholder;
using Beholder.Platform;
using DemoRunner.Scenes.EarthlitNight;

namespace DemoRunner
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            AppDomain.CurrentDomain.UnhandledException += CurrentDomainOnUnhandledException;
            Run(args);
        }

        static void CurrentDomainOnUnhandledException(object sender, UnhandledExceptionEventArgs unhandledExceptionEventArgs)
        {
            MessageBox.Show(unhandledExceptionEventArgs.ExceptionObject.ToString(), "Unhandled Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        static void Run(string[] args)
        {
            if (args.Length != 2)
                throw new InvalidDataException("There must be exactly two arguments for DemoRunner");

            var eyeName = UnescapeSpaces(args[0]);
            var sceneName = UnescapeSpaces(args[1]);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            using (var eye = CreateEye(eyeName))
            {
                var adapter = eye.Adapters[0];

                var desctopDisplayMode = adapter.Outputs[0].GetSupportedDisplayModes()
                    .OrderByDescending(m => m.Width * m.Height)
                    .ThenByDescending(m => m.RefreshRate == 60 ? int.MaxValue : m.RefreshRate.Round())
                    .ThenBy(m => { var fi = adapter.GetFormatInfo(m.FormatID); return fi.ColorBits == 24 && fi.TotalBits == 32 ? 0 : 1; })
                    .ThenBy(m => FormatTypePriority(adapter.GetFormatInfo(m.FormatID).ColorFormatType))
                    .ThenByDescending(m => adapter.GetFormatInfo(m.FormatID).ColorBits)
                    .First();

                var depthStencilFormat = adapter.GetSupportedFormats(FormatSupport.DepthStencil)
                    .OrderByDescending(fi => fi.ColorBits)
                    .ThenBy(fi => FormatTypePriority(fi.ColorFormatType))
                    .First();

                var windowHandle = eye.CreateNewWindow(400, 400, string.Format("Beholder {0} - {1}", eyeName, sceneName), true);
                eye.Initialize(adapter, windowHandle, new SwapChainDescription(2, desctopDisplayMode.FormatID, true, depthStencilFormat.ID, new Sampling(4, 0), true));

                var scene = CreateScene(sceneName, eye);

                eye.NewFrame += scene.OnNewFrame;
                eye.RunLoop(eye.Device.PrimarySwapChain.Window);
            }
        }

        static IEye CreateEye(string eyeName)
        {
            switch (eyeName)
            {
                case "Direct3D 9": return EyeCreator.FromAssembly("Beholder.Eyes.SharpDX9.Winforms.dll", "Beholder.Eyes.SharpDX9.Winforms.WinformsEye");
                case "Direct3D 11": return EyeCreator.FromAssembly("Beholder.Eyes.SharpDX11.Winforms.dll", "Beholder.Eyes.SharpDX11.Winforms.WinformsEye");
                case "OpenGL 4.x": return EyeCreator.FromAssembly("Beholder.Eyes.ObjectGL4.Default.dll", "Beholder.Eyes.ObjectGL4.Default.DefaultEye");
                default: throw new ArgumentOutOfRangeException("eyeName");
            }
        }

        static IScene CreateScene(string sceneName, IEye eye)
        {
            switch (sceneName)
            {
                case "Earthlit Night": return new EarthlitNightScene(eye);
                default: throw new ArgumentOutOfRangeException("sceneName");
            }
        }

        static string UnescapeSpaces(string s)
        {
            return s.Replace('-', ' ');
        }

        static int FormatTypePriority(FormatElementType formatType)
        {
            switch (formatType)
            {
                case FormatElementType.UNORM: return 0;
                case FormatElementType.FLOAT: return 1;
                default: return 2;
            }
        }
    }
}
