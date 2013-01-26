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
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Beholder.Shaders.Reflection;

namespace Beholder.Utility.ForImplementations.Shaders
{
    public class SlotTranslation<T> where T : ShaderSpecialParametersHolder
    {
        readonly T[] holdersByBeholderSlot;
        readonly int beholderStartSlot;
        readonly int beholderRangeLength;
        readonly int apiStartSlot;
        readonly int apiRangeLength;

        public int ApiStartSlot { get { return apiStartSlot; } }
        public int ApiRangeLength { get { return apiRangeLength; } }

        public SlotTranslation(IEnumerable<T> holders, 
            Func<T, int> getBeholderSlot, 
            Func<T, int> getApiSlot,
            string holdersTypeString, string locationString)
        {
            if (holders != null && holders.Any())
            {
                int minBSlot = holders.Min(getBeholderSlot);
                int maxBSlot = holders.Max(getBeholderSlot);

                beholderRangeLength = maxBSlot - minBSlot + 1;
                holdersByBeholderSlot = new T[beholderRangeLength];
                beholderStartSlot = minBSlot;

                foreach (var holder in holders)
                {
                    var index = getBeholderSlot(holder) - beholderStartSlot;
                    if (holdersByBeholderSlot[index] != null)
                        throw new InvalidDataException(string.Format(
                            "{0} '{1}' and '{2}' are occupying the same beholder slot '{3}'",
                            holdersTypeString, holdersByBeholderSlot[index].Name, holder.Name, getBeholderSlot(holder)));
                    holdersByBeholderSlot[index] = holder;
                }

                int minApiSlot = int.MaxValue;
                int maxApiSlot = int.MinValue;

                for (int j = 0; j < holdersByBeholderSlot.Length; j++)
                {
                    if (holdersByBeholderSlot[j] == null)
                        continue;

                    int apiSlotJ = getApiSlot(holdersByBeholderSlot[j]);
                    if (apiSlotJ < minApiSlot)
                        minApiSlot = apiSlotJ;
                    if (apiSlotJ > maxApiSlot)
                        maxApiSlot = apiSlotJ;

                    for (int i = j + 1; i < holdersByBeholderSlot.Length; i++)
                    {
                        if (holdersByBeholderSlot[i] == null)
                            continue;
                        int apiSlotI = getApiSlot(holdersByBeholderSlot[i]);
                        if (apiSlotI == apiSlotJ)
                            throw new InvalidDataException(string.Format(
                            "{0} '{1}' and '{2}' are occupying the same {3} '{4}'",
                            holdersTypeString, holdersByBeholderSlot[i].Name, holdersByBeholderSlot[j].Name, locationString, apiSlotI));
                    }
                }

                apiStartSlot = minApiSlot;
                apiRangeLength = maxApiSlot - minApiSlot + 1;
            }
            else
            {
                holdersByBeholderSlot = EmptyArray;
                beholderStartSlot = beholderRangeLength = apiStartSlot = apiRangeLength = 0;
            }
        }

        public bool TryGetVariable(int bSlot, out T variable)
        {
            var index = bSlot + beholderStartSlot;
            if (index >= holdersByBeholderSlot.Length || holdersByBeholderSlot[index] == null)
            {
                variable = null;
                return false;
            }

            variable = holdersByBeholderSlot[index];
            return true;
        }

        public IEnumerable<T> GetActive()
        {
            return holdersByBeholderSlot.Where(h => h != null);
        }

        static readonly T[] EmptyArray = new T[0];
    }
}
