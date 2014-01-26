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
using Beholder.Utility.Collections;

namespace Beholder.Shaders.Reflection
{
    public abstract class ShaderSpecialParametersHolder
    {
        readonly string name;
        readonly TightConcurrentDictionary<string, string> specialParameters;

        protected ShaderSpecialParametersHolder(string name, IEnumerable<KeyValuePair<string, string>> specialParameters)
        {
            if (name == null)
                throw new ArgumentNullException("name");
            if (specialParameters == null)
                throw new ArgumentNullException("specialParameters");

            this.name = name;
            this.specialParameters = new TightConcurrentDictionary<string, string>(string.Equals, specialParameters.Count());
            foreach (var specialParameter in specialParameters)
                this.specialParameters.GetOrAdd(specialParameter.Key, specialParameter.Value, (k, v) => v);
        }

        public string Name { get { return name; } }

        public IEnumerable<KeyValuePair<string, string>> SpecialParameters
        {
            get { return specialParameters; }
        } 

        public bool TryGetSpecialParameter(string parameterName, out string parameterValue)
        {
            return specialParameters.TryGet(parameterName, out parameterValue);
        }

        public string GetRequiredSpecialParameter(string parameterName)
        {
            string result;
            if (!TryGetSpecialParameter(parameterName, out result))
                throw new InvalidDataException(string.Format("{0} does not have the required parameter '{1}'", name, parameterName));
            return result;
        }

        public int GetReuiredSpecialParameterInt(string parameterName)
        {
            var resultString = GetRequiredSpecialParameter(parameterName);
            int result;
            if (!int.TryParse(resultString, out result))
                throw new InvalidDataException(string.Format("Integer expected, but '{2}' found as {1} of {0}", name, parameterName, resultString));
            return result;
        }

        public int GetSlot()
        {
            return GetReuiredSpecialParameterInt("slot");
        }
    }
}
