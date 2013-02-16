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
using System.Linq;
using System.Windows.Forms;
using Beholder;
using Beholder.Platform;

namespace Launcher
{
    static class Program
    {
        public static int FormatTypePriority(FormatElementType formatType)
        {
            switch (formatType)
            {
                case FormatElementType.UNORM: return 0;
                case FormatElementType.FLOAT: return 1;
                default: return 2;
            }
        }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            //var eye = EyeCreator.FromAssembly("Beholder.Eyes.SharpDX9.Winforms.dll", "Beholder.Eyes.SharpDX9.Winforms.WinformsEye");
            //var eye = EyeCreator.FromAssembly("Beholder.Eyes.SharpDX11.Winforms.dll", "Beholder.Eyes.SharpDX11.Winforms.WinformsEye");
            var eye = EyeCreator.FromAssembly("Beholder.Eyes.ObjectGL4.Default.dll", "Beholder.Eyes.ObjectGL4.Default.DefaultEye");

            //eye = new Beholder.Validation.Eye(eye);
            using (eye)
            {
                var flags = DeviceInitializationFlags.None;
                // flags |= DeviceInitializationFlags.Debug;

                var adapter = eye.Adapters[0];

                var displayFormat = adapter.GetSupportedWindowedDisplayFormats()
                    .OrderBy(fi => (fi.ColorBits == 24 && fi.TotalBits == 32) ? 0 : 1)
                    .ThenBy(fi => FormatTypePriority(fi.ColorFormatType))
                    .ThenByDescending(fi => fi.ColorBits)
                    .ThenBy(fi => fi.TotalBits)
                    .First();

                var depthStencilFormat = adapter.GetSupportedFormats(FormatSupport.DepthStencil)
                    .OrderByDescending(fi => fi.ColorBits)
                    .ThenBy(fi => FormatTypePriority(fi.ColorFormatType))
                    .First();

                var windowHandle = eye.CreateNewWindow(new WindowDescription(400, 400, "New Window", true));
                eye.Initialize(adapter, windowHandle, new SwapChainDescription(2, displayFormat.ID, true, depthStencilFormat.ID, new Sampling(4, 0), true), flags);

                using (
                    //var scene = new ClearScreenScene(eye, displayFormat.ID, eye.Device.PrimarySwapChain)
                    //var scene = new TriangleScene(eye, displayFormat.ID, eye.Device.PrimarySwapChain)
                    //var scene = new FullscreenQuadScene(eye, displayFormat.ID, eye.Device.PrimarySwapChain)
                    var scene = new CubeScene(eye, displayFormat.ID, eye.Device.PrimarySwapChain)
                    //var scene = new RenderToTextureScene(eye, displayFormat.ID, eye.Device.PrimarySwapChain)
                    //var scene = new ColorfulSpaceScene(eye, displayFormat.ID, eye.Device.PrimarySwapChain)
                    //var scene = new CurveTesselationScene(eye, displayFormat.ID, eye.Device.PrimarySwapChain)
                    )
                {
                    scene.Run();
                }
            }
        }
    }
}
