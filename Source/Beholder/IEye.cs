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
using Beholder.Utility.Collections.Readonly;
using Beholder.Utility.Patterns;

namespace Beholder
{
    /// <summary>
    /// Main interface of the Beholder.
    /// It provides the means to enumerate graphical adapters,
    /// create a <see cref="IDevice"/> and run a game loop.
    /// </summary>
    public interface IEye : IDisposableOnce
    {
        /// <summary>
        /// Gets a list of all the graphical adapters available. 
        /// The adapter with the zero index is the most prefered adpater to be used.
        /// </summary>
        IReadonlyList<IAdapter> Adapters { get; }

        /// <summary>
        /// Prepares the existing platform-specific window to hold a swap chain.
        /// </summary>
        /// <param name="platformWindow">Platform-specific window.</param>
        /// <returns>Eye-specific handle of the window.</returns>
        IWindowHandle PrepareExistingWindow(object platformWindow);

        /// <summary>
        /// Creates a new window that is capable of holding a swap chain.
        /// </summary>
        /// <param name="description">Description of a window to create.</param>
        /// <returns>Eye-specific handle of the window.</returns>
        IWindowHandle CreateNewWindow(WindowDescription description);

        /// <summary>
        /// Indicates whether the eye has already been initialized.
        /// </summary>
        bool IsInitialized { get; }

        /// <summary>
        /// Initializes the eye.
        /// </summary>
        /// <param name="adapter">Adapter to be used by the device. Can not be null.</param>
        /// <param name="windowHandle">Handle of the window to hold the primary swap chain.</param>
        /// <param name="primarySwapChainDescription">Description of the primary swap chain.</param>
        /// <param name="initializationFlags">Flags specifying threading and debug mode of the device.</param>
        /// <param name="fileSystem">Unusual file system for loader to use. Specify null to use OS file system.</param>
        void Initialize(IAdapter adapter,
                        IWindowHandle windowHandle,
                        SwapChainDescription primarySwapChainDescription,
                        DeviceInitializationFlags initializationFlags = DeviceInitializationFlags.None,
                        IFileSystem fileSystem = null);

        /// <summary>
        /// <see cref="IDevice"/> of this <see cref="IEye"/>.
        /// </summary>
        /// <remarks>
        /// Device can only be obtained after it is initialized with the <see cref="IEye.Initialize"/> method.
        /// </remarks>
        IDevice Device { get; }

        /// <summary>
        /// Starts the game loop.
        /// </summary>
        /// <param name="mainWindow"><see cref="IWindow"/> that will terminate a loop if closed.</param>
        /// <remarks>
        /// This method will only return when either <see cref="TerminateLoop"/> method is called or the main window is closed.
        /// </remarks>
        void RunLoop(IWindow mainWindow = null);

        /// <summary>
        /// Causes the game loop to terminate.
        /// </summary>
        void TerminateLoop();

        /// <summary>
        /// Raised with the start of an every frame.
        /// </summary>
        event Action<IRealTime> NewFrame;

        /// <summary>
        /// Raised when the game loop is about to terminate.
        /// </summary>
        /// <remarks><see cref="PreventEventArgs"/> object can be used to prevent the game loop termination.</remarks>
        event Action<PreventEventArgs> LoopTerminating;
    }
}