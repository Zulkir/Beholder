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
using System.Collections.Concurrent;
using System.Linq;
using Beholder.Core;
using Beholder.Input;
using Beholder.Platform;
using Beholder.Resources;
using Beholder.Shaders;
using Beholder.Validation.Core;
using Beholder.Validation.Input;
using Beholder.Validation.Platform;
using Beholder.Validation.Resources;
using Beholder.Validation.Shaders;
using Buffer = Beholder.Validation.Resources.Buffer;

namespace Beholder.Validation
{
    static class Wrappers
    {
        const int MaxTypeCollisions = 32;
        static readonly ConcurrentDictionary<object, object>[] Collection = Enumerable.Range(0, MaxTypeCollisions).Select(x => new ConcurrentDictionary<object, object>()).ToArray();

        public static void InitializeEye(Eye eye)
        {
            Collection[0].AddOrUpdate(eye.Real, eye, (k, c) => eye);
        }

        public static void Remove<T>(IWrapper<T> obj) where T : class
        {
            object o;
            if (!Collection[0].TryRemove(obj.Real, out o) || obj != o)
                throw new InvalidOperationException("Removing a wrapper failed");
            for (int i = 1; i < MaxTypeCollisions; i++)
                Collection[i].TryRemove(obj.Real, out o);
        }

        static TWrapper GetGeneric<TWrapper, TInterface>(TInterface obj, Func<TInterface, TWrapper> constructor) 
            where TWrapper : class, TInterface
            where TInterface : class
        {
            if (obj == null)
                return null;
            if (obj is TWrapper)
                return (TWrapper)obj;
            foreach (var dictionary in Collection)
            {
                var wrapper = dictionary.GetOrAdd(obj, o => constructor((TInterface)o));
                if (wrapper is TWrapper)
                    return (TWrapper)wrapper;
            }
            throw new OverflowException("Maximum type collision number exceeded");
        }

        public static Eye Get(IEye obj) { return GetGeneric(obj, o => new Eye(o)); }
        public static Adapter Get(IAdapter obj) { return GetGeneric(obj, o => new Adapter(o)); }
        public static Output Get(IOutput obj) { return GetGeneric(obj, o => new Output(o)); }
        public static WindowHandle Get(IWindowHandle obj) { return GetGeneric(obj, o => new WindowHandle(o)); }
        public static Window Get(IWindow obj) { return GetGeneric(obj, o => new Window(o)); }
        public static InputHandler Get(IInputHandler obj) { return GetGeneric(obj, o => new InputHandler(o)); }
        public static KeyboardHandler Get(IKeyboardHandler obj) { return GetGeneric(obj, o => new KeyboardHandler(o)); }
        public static MouseHandler Get(IMouseHandler obj) { return GetGeneric(obj, o => new MouseHandler(o)); }
        public static Device Get(IDevice obj) { return GetGeneric(obj, o => new Device(o)); }
        public static ISwapChain Get(ISwapChain obj)
        {
            if (obj is IPrimarySwapChain) return Get((IPrimarySwapChain)obj);
            if (obj is IAdditionalSwapChain) return Get((IAdditionalSwapChain)obj);
            throw new InvalidOperationException(string.Format("obj is of unexpected type '{0}'", obj.GetType()));
        }
        public static PrimarySwapChain Get(IPrimarySwapChain obj) { return GetGeneric(obj, o => new PrimarySwapChain(o)); }
        public static AdditionalSwapChain Get(IAdditionalSwapChain obj) { return GetGeneric(obj, o => new AdditionalSwapChain(o)); }
        public static DeviceChildCreator Get(IDeviceChildCreator obj) { return GetGeneric(obj, o => new DeviceChildCreator(o)); }
        public static DeviceChildLoader Get(IDeviceChildLoader obj) { return GetGeneric(obj, o => new DeviceChildLoader(o)); }
        public static DeviceContext Get(IDeviceContext obj) { return GetGeneric(obj, o => new DeviceContext(o)); }
        public static BlendState Get(IBlendState obj) { return GetGeneric(obj, o => new BlendState(o)); }
        public static DepthStencilState Get(IDepthStencilState obj) { return GetGeneric(obj, o => new DepthStencilState(o)); }
        public static RasterizerState Get(IRasterizerState obj) { return GetGeneric(obj, o => new RasterizerState(o)); }
        public static SamplerState Get(ISamplerState obj) { return GetGeneric(obj, o => new SamplerState(o)); }
        public static Resource Get(IResource obj)
        {
            if (obj is IBuffer) return Get((IBuffer)obj);
            if (obj is ITexture1D) return Get((ITexture1D)obj);
            if (obj is ITexture2D) return Get((ITexture2D)obj);
            if (obj is ITexture3D) return Get((ITexture3D)obj);
            throw new InvalidOperationException(string.Format("obj is of unexpected type '{0}'", obj.GetType()));
        }
        public static Buffer Get(IBuffer obj) { return GetGeneric(obj, o => new Buffer(o));}
        public static Texture1D Get(ITexture1D obj) { return GetGeneric(obj, o => new Texture1D(o)); }
        public static Texture2D Get(ITexture2D obj) { return GetGeneric(obj, o => new Texture2D(o)); }
        public static Texture3D Get(ITexture3D obj) { return GetGeneric(obj, o => new Texture3D(o)); }
        public static VertexLayout Get(IVertexLayout obj) { return GetGeneric(obj, o => new VertexLayout(o)); }
        public static RenderTargetView Get(IRenderTargetView obj) { return GetGeneric(obj, o => new RenderTargetView(o)); }
        public static DepthStencilView Get(IDepthStencilView obj) { return GetGeneric(obj, o => new DepthStencilView(o)); }
        public static ShaderResourceView Get(IShaderResourceView obj) { return GetGeneric(obj, o => new ShaderResourceView(o)); }
        public static UnorderedAccessView Get(IUnorderedAccessView obj) { return GetGeneric(obj, o => new UnorderedAccessView(o)); }
        public static VertexShader Get(IVertexShader obj) { return GetGeneric(obj, o => new VertexShader(o)); }
        public static HullShader Get(IHullShader obj) { return GetGeneric(obj, o => new HullShader(o)); }
        public static DomainShader Get(IDomainShader obj) { return GetGeneric(obj, o => new DomainShader(o)); }
        public static GeometryShader Get(IGeometryShader obj) { return GetGeneric(obj, o => new GeometryShader(o)); }
        public static PixelShader Get(IPixelShader obj) { return GetGeneric(obj, o => new PixelShader(o)); }
        public static ComputeShader Get(IComputeShader obj) { return GetGeneric(obj, o => new ComputeShader(o)); }
        public static ShaderCombination Get(IShaderCombination obj) { return GetGeneric(obj, o => new ShaderCombination(o)); }
    }
}
