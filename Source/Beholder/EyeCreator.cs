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
using System.IO;
using System.Reflection;

namespace Beholder
{
    /// <summary>
    /// Contains helper methods for creating IEye implementations.
    /// </summary>
    public static class EyeCreator
    {
        /// <summary>
        /// Creates an IEye implementation by its type name and containing assembly path.
        /// </summary>
        /// <param name="assemblyPath">Path to an assembly file that stores the IEye implementation.</param>
        /// <param name="fullTypeName">Full type name of the IEye implementation.</param>
        /// <param name="flags">Flags that specify additional options for IEye implementation creation.</param>
        public static IEye FromAssembly(string assemblyPath, string fullTypeName, EyeCreationFlags flags = EyeCreationFlags.None)
        {
            var assemblyFullPath = Path.GetFullPath(assemblyPath);
            if (assemblyFullPath == null)
                throw new FileNotFoundException("Could not resolve assembly full path");
            var assembly = Assembly.LoadFile(assemblyFullPath);
            var type = assembly.GetType(fullTypeName, true);
            var constructor = type.GetConstructor(new Type[0]);
            if (constructor == null)
                throw new ArgumentException("Type '{0}' does not have a parameterless constructor, and thus, cannot be an IEye implementation", fullTypeName);
            var eye = constructor.Invoke(new object[0]) as IEye;
            if (eye == null)
                throw new ArgumentException("Type '{0}' does not implement the IEye interface", fullTypeName);
            if (flags.HasFlag(EyeCreationFlags.ValidationLayer))
                eye = new Validation.Eye(eye);
            return eye;
        }
    }
}