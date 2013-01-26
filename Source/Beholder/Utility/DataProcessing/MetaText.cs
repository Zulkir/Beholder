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
using System.Globalization;
using System.Text.RegularExpressions;

namespace Beholder.Utility.DataProcessing
{
    public class MetaText
    {
        public string Doctype { get; set; }
        public double Version { get; set; }

        public Dictionary<string, string> Meta { get; private set; }
        public Dictionary<string, string[]> Regions { get; private set; }

        public MetaText(string doctype = "metatext", double version = 1.0)
        {
            Doctype = doctype;
            Version = version;

            Meta = new Dictionary<string, string>();
            Regions = new Dictionary<string, string[]>();
        }
        
        static readonly Regex FirstLineEx = new Regex(@"^%doctype\s+([_a-zA-Z]\w+)\s+(\d+\.\d+)$");
        static readonly Regex MetaEx = new Regex(@"^([_a-zA-Z]\w*)\s*=\s*(\S.*)$");
        static readonly Regex RegionEx = new Regex(@"^%region\s+([_a-zA-Z]\w*)$");

        public static MetaText FromLines(string[] allLines)
        {
            if (allLines == null) throw new ArgumentNullException("allLines");
            if (allLines.Length == 0) throw new TextParsingException("Metatext file must contain at least one line");

            Match match = FirstLineEx.Match(allLines[0].Trim());
            string doctype = match.Groups[1].Value;
            double version = double.Parse(match.Groups[2].Value, CultureInfo.InvariantCulture);

            if (!match.Success) throw new TextParsingException("Metatext file must begin with a '%doctype <doctype> <version>' line.");
            if (doctype != "metatext") throw new TextParsingException(string.Format("Doctype '{0}' is not supported.", doctype));
            if (version != 1.0) throw new TextParsingException(string.Format("Version '{0}' is not supported.", version.ToString(CultureInfo.InvariantCulture)));
            
            var metatext = new MetaText(doctype, version);

            const int readingMeta = 0;
            const int readingRegions = 1;

            int state = readingMeta;
            string currentRegion = "";

            var regionLines = new List<string>();

            for (int i = 1; i < allLines.Length; i++)
            {
                string line = allLines[i].Trim();
                if (line == string.Empty) continue;

                switch (state)
                {
                    case readingMeta:
                        {
                            if (line.StartsWith("%region"))
                            {
                                state = readingRegions;
                                match = RegionEx.Match(line);
                                if (!match.Success) 
                                    throw new TextParsingException("Region declaration must have a '%region <name>' format.", i);
                                currentRegion = match.Groups[1].Value;
                            }
                            else
                            {
                                match = MetaEx.Match(line);
                                if (!match.Success) 
                                    throw new TextParsingException("Meta declaration must have a '<name> = <value>' format.", i);
                                metatext.Meta.Add(match.Groups[1].Value, match.Groups[2].Value);
                            }
                        }
                        break;
                    case readingRegions:
                        {
                            if (line.StartsWith("%region"))
                            {
                                metatext.Regions.Add(currentRegion, regionLines.ToArray());
                                regionLines.Clear();

                                match = RegionEx.Match(line);
                                if (!match.Success) 
                                    throw new TextParsingException("Region declaration must have a '%region <name>' format.", i);
                                currentRegion = match.Groups[1].Value;
                            }
                            else
                            {
                                regionLines.Add(line);
                            }
                        }
                        break;
                }
            }

            metatext.Regions.Add(currentRegion, regionLines.ToArray());
            regionLines.Clear();

            return metatext;
        }
    }
}
