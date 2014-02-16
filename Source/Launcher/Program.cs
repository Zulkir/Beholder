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
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
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

        static IEnumerable<Exception> UnrollException(Exception ex)
        {
            while (ex != null)
            {
                yield return ex;
                ex = ex.InnerException;
            }
        }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            try
            {
                Run(args);
            }
            catch (Exception ex)
            {
                var builder = new StringBuilder();
                foreach (var innerException in UnrollException(ex))
                {
                    builder.AppendLine("--- Exception ---");
                    builder.AppendLine(innerException.Message);
                    builder.AppendLine();
                    builder.AppendLine(innerException.StackTrace);
                    builder.AppendLine();
                }

                File.WriteAllText("error.txt", builder.ToString());
                throw;
            }
        }

        static void Run(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            IEye eye;
            if (args.Length > 0)
            {
                switch (args[0])
                {
                    case "dx9": eye = EyeCreator.FromAssembly("Beholder.Eyes.SharpDX9.Winforms.dll", "Beholder.Eyes.SharpDX9.Winforms.WinformsEye"); break;
                    case "dx11": eye = EyeCreator.FromAssembly("Beholder.Eyes.SharpDX11.Winforms.dll", "Beholder.Eyes.SharpDX11.Winforms.WinformsEye"); break;
                    case "ogl": eye = EyeCreator.FromAssembly("Beholder.Eyes.ObjectGL4.Default.dll", "Beholder.Eyes.ObjectGL4.Default.DefaultEye"); break;
                    default: eye = EyeCreator.FromAssembly("Beholder.Eyes.ObjectGL4.Default.dll", "Beholder.Eyes.ObjectGL4.Default.DefaultEye"); break;
                }
            }
            else
            {
                //eye = EyeCreator.FromAssembly("Beholder.Eyes.SharpDX9.Winforms.dll", "Beholder.Eyes.SharpDX9.Winforms.WinformsEye");
                //eye = EyeCreator.FromAssembly("Beholder.Eyes.SharpDX11.Winforms.dll", "Beholder.Eyes.SharpDX11.Winforms.WinformsEye");
                eye = EyeCreator.FromAssembly("Beholder.Eyes.ObjectGL4.Default.dll", "Beholder.Eyes.ObjectGL4.Default.DefaultEye");
            }

            eye = new Beholder.Validation.Eye(eye);
            using (eye)
            {
                var flags = DeviceInitializationFlags.None;
                // flags |= DeviceInitializationFlags.Debug;

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

                var windowHandle = eye.CreateNewWindow(400, 400, "Beholder Demo", true);
                eye.Initialize(adapter, windowHandle, new SwapChainDescription(2, desctopDisplayMode.FormatID, true, depthStencilFormat.ID, new Sampling(4, 0), true), flags);

                var metaScene = new MetaScene(eye, desctopDisplayMode);
                metaScene.Run();
            }
        }
    }
}
