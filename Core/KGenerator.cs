using Grasshopper.Kernel.Types;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Drawing;
using Rhino.Geometry;

namespace Kniteretta
{
    /// <summary>
    /// Object that is responsible for generating the stitch and yarn patterns
    /// </summary>
    public class KGenerator
    {
        /// <summary>
        /// Writer instance that is used by this generator
        /// </summary>
        public KWriter Writer;
        private GH_Colour[,] _yarnPatterns;
        private GH_Colour[,] _stitchPatterns;
        private Dictionary<int, string> _stitchDict;
        private Dictionary<int, int> _yarnDict;
        private KStitchPalette _stitchPalette;
        private KYarnPalette _yarnPalette;

        /// <summary>
        /// Constructs a generator object
        /// </summary>
        /// <param name="sPalette">stitch palette</param>
        /// <param name="stitches">stitch patterns</param>
        /// <param name="yPalette">yarn palette</param>
        /// <param name="yarns">yarn patterns</param>
        /// <param name="kParams">complementary parameters</param>
        /// <exception cref="Exception"></exception>
        public KGenerator(KStitchPalette sPalette, GH_Colour[,] stitches, KYarnPalette yPalette, GH_Colour[,] yarns, KParameters kParams)
        {
            if (stitches.GetLength(0) != yarns.GetLength(0))
            {
                throw new Exception("Stitches and Yarns must have the same width");
            }

            if (stitches.GetLength(1) != yarns.GetLength(1))
            {
                throw new Exception("Stitches and Yarns must have the same height");
            }

            _stitchPatterns = stitches;
            _yarnPatterns = yarns;
            _stitchPalette = sPalette;
            _yarnPalette = yPalette;

            // Create the stitch dictionary
            _stitchDict = new Dictionary<int, string>()
            {
                { sPalette.Front.Value.ToArgb(), "-" },
                { sPalette.Rear.Value.ToArgb(), "." },
                { sPalette.Both.Value.ToArgb(), "X" },
                { sPalette.TransferToRear.Value.ToArgb(), "↑" },
                { sPalette.TransferToFront.Value.ToArgb(), "↓" },
                { sPalette.Miss.Value.ToArgb(), " " },
                { sPalette.LeftFrontToRear1.Value.ToArgb(), "↖" },
                { sPalette.LeftFrontToRear2.Value.ToArgb(), "↖" },
                { sPalette.LeftFrontToRear3.Value.ToArgb(), "↖" },
                { sPalette.RightFrontToRear1.Value.ToArgb(), "↗" },
                { sPalette.RightFrontToRear2.Value.ToArgb(), "↗" },
                { sPalette.RightFrontToRear3.Value.ToArgb(), "↗" },
                { sPalette.LeftRearToFront1.Value.ToArgb(), "↙" },
                { sPalette.LeftRearToFront2.Value.ToArgb(), "↙" },
                { sPalette.LeftRearToFront3.Value.ToArgb(), "↙" },
                { sPalette.RightRearToFront1.Value.ToArgb(), "↘" },
                { sPalette.RightRearToFront2.Value.ToArgb(), "↘" },
                { sPalette.RightRearToFront3.Value.ToArgb(), "↘" },
                { sPalette.TuckOnFront.Value.ToArgb(), "V" },
                { sPalette.TuckOnRear.Value.ToArgb(), "^" },
                { sPalette.KnitFrontTuckRear.Value.ToArgb(), "F" },
                { sPalette.TuckFrontKnitRear.Value.ToArgb(), "R" },
                { sPalette.Slip.Value.ToArgb(), "M" },
                { sPalette.SplitOnFront.Value.ToArgb(), "↔" },
                { sPalette.SplitOnRear.Value.ToArgb(), "=" }
            };

            // Create the yarn dictionary
            _yarnDict = new Dictionary<int, int>()
            {
                { yPalette.Yarn0.Value.ToArgb(), 0 },
                { yPalette.Yarn1.Value.ToArgb(), 1 },
                { yPalette.Yarn2.Value.ToArgb(), 2 },
                { yPalette.Yarn3.Value.ToArgb(), 3 },
                { yPalette.Yarn4.Value.ToArgb(), 4 },
                { yPalette.Yarn5.Value.ToArgb(), 5 },
                { yPalette.Yarn6.Value.ToArgb(), 6 },
            };

            Writer = new KWriter();
            GenerateYarnPalette();
            Writer.SetCustomParameters(kParams);
        }

        /// <summary>
        /// Generate this generator's output
        /// </summary>
        public void Generate()
        {
            var time = DateTime.Now;
            GenerateStitches();
            GenerateYarn();
            Writer.Generate();
            var elapsed = DateTime.Now.Millisecond - time.Millisecond;
        }

        /// <summary>
        /// Generates the stitch patterns/symbols by comparing the stitches against its palette
        /// </summary>
        /// <exception cref="Exception"></exception>
        private void GenerateStitches()
        {
            if (_stitchDict.Count > 0)
            {
                var stitchSymbols = new System.Text.StringBuilder();
                int width = _stitchPatterns.GetLength(0);
                int height = _stitchPatterns.GetLength(1);

                for (int h = 0; h < height; h++)
                {
                    for (int w = 0; w < width; w++)
                    {
                        GH_Colour pixel = _stitchPatterns[w, h];
                        int key = pixel.Value.ToArgb();
                        if (_stitchDict.ContainsKey(key)) stitchSymbols.Append(_stitchDict[key]);
                        else stitchSymbols.Append(" ");
                    }
                    stitchSymbols.Append('\n');
                }
                Writer.SetParameter("stitch_symbols", stitchSymbols.ToString());
            }
            else
            {
                throw new Exception("Stitch dictionary is empty");
            }
        }

        /// <summary>
        /// Generates the yarn patterns/symbols by comparing the stitches against its palette
        /// </summary>
        /// <exception cref="Exception"></exception>
        private void GenerateYarn()
        {
            if (_yarnDict.Count > 0)
            {
                var yarnSymbols = new System.Text.StringBuilder();
                int width = _yarnPatterns.GetLength(0);
                int height = _yarnPatterns.GetLength(1);

                for (int h = 0; h < height; h++)
                {
                    for (int w = 0; w < width; w++)
                    {
                        GH_Colour pixel = _yarnPatterns[w, h];
                        int key = pixel.Value.ToArgb();
                        if (_yarnDict.ContainsKey(key)) yarnSymbols.Append(_yarnDict[key]);
                        else yarnSymbols.Append(" ");
                    }
                    yarnSymbols.Append('\n');
                }
                Writer.SetParameter("yarn", yarnSymbols.ToString());
            }
            else
            {
                throw new Exception("Yarn dictionary is empty");
            }
        }

        /// <summary>
        /// Generates the writer's yarn palette according to <see cref="_yarnDict"/>
        /// </summary>
        /// <exception cref="Exception"></exception>
        private void GenerateYarnPalette()
        {
            if (_yarnDict.Count > 0)
            {
                string palette = "";
                foreach (var item in _yarnDict)
                {
                    Color clr = Color.FromArgb(item.Key);
                    palette += KWriter.CreateYarnForPalette(item.Value, clr.R, clr.G, clr.B, item.Value.ToString());
                    palette += '\n';
                }
                Writer.SetParameter("yarn_palette", palette);
            }
            else
            {
                throw new Exception("Yarn dictionary is empty");
            }
        }

        #region Handle pattern changes

        public (GH_Colour[,], GH_Colour[,]) ImplementTransfers()
        {
            int width = _stitchPatterns.GetLength(0);
            int height = _stitchPatterns.GetLength(1);

            // create lists to store the results
            List<GH_Colour[]> stitchesWithTransfers = new List<GH_Colour[]>();
            List<GH_Colour[]> yarnsWithTransfers = new List<GH_Colour[]>();

            // walk through the stitches, bottom up
            for (int y = height - 1; y >= 0; y--)
            {
                GH_Colour[] currentStitchLine = GetLine(_stitchPatterns, y);
                GH_Colour[] previousStitchLine = y + 1 < height ? GetLine(_stitchPatterns, y + 1) : null;
                GH_Colour[] nextStitchLine = y - 1 >= 0 ? GetLine(_stitchPatterns, y - 1) : null;

                GH_Colour[] currentYarnLine = GetLine(_yarnPatterns, y);

                // if there is no previous line or it is equal to the current, skip and add the current line
                if (previousStitchLine == null)
                {
                    if (AreLinesEqual(currentStitchLine, nextStitchLine))
                    {
                        stitchesWithTransfers.Add(currentStitchLine);
                        yarnsWithTransfers.Add(currentYarnLine);
                        continue;
                    }
                }
                else
                {
                    if (AreLinesEqual(currentStitchLine, previousStitchLine))
                    {
                        if (nextStitchLine == null || AreLinesEqual(currentStitchLine, nextStitchLine))
                        {
                            stitchesWithTransfers.Add(currentStitchLine);
                            yarnsWithTransfers.Add(currentYarnLine);
                            continue;
                        }
                    }
                }

                // line to group transfers to back
                GH_Colour[] previousToBackLine = new GH_Colour[width];
                GH_Colour[] nextToBackLine = new GH_Colour[width];
                Dictionary<GH_Colour, List<int>> previousTransferIndexDict = new Dictionary<GH_Colour, List<int>>();
                Dictionary<GH_Colour, List<int>> nextTransferIndexDict = new Dictionary<GH_Colour, List<int>>();

                // yarn transfer line, which has the same width as the 
                GH_Colour[] yarnTransferLine = Enumerable.Repeat(_yarnPalette.Yarn0, width).ToArray();
                bool changedPrevious = false;
                bool changedNext = false;

                for (int x = 0; x < width; x++)
                {
                    // compare two cells at a time
                    int current = currentStitchLine[x].Value.ToArgb();
                    int previous = previousStitchLine != null ? previousStitchLine[x].Value.ToArgb() : 0;

                    // detect pattern cases
                    // going from front to miss
                    if (current != previous && previousStitchLine != null && previous == _stitchPalette.Front.Value.ToArgb() && current == _stitchPalette.Miss.Value.ToArgb())
                    {
                        // need to transfer the current front stitch to back and then to the next available front
                        // find the closest point to transfer to
                        int nextOnRight = Array.IndexOf(currentStitchLine.Where((_, i) => i > x && i < x + 4).Select(c => c.Value.ToArgb()).ToArray(), _stitchPalette.Front.Value.ToArgb()) + 1;
                        int nextOnLeft = Array.IndexOf(currentStitchLine.Where((_, i) => i < x && i > x - 4).Reverse().Select(c => c.Value.ToArgb()).ToArray(), _stitchPalette.Front.Value.ToArgb()) + 1;

                        if (nextOnRight == 0 && nextOnLeft == 0) continue;
                        changedPrevious = true;

                        int targetIndex = nextOnRight <= nextOnLeft ? x + nextOnRight : x - nextOnLeft;
                        int diff = Math.Min(nextOnRight, nextOnLeft);

                        previousToBackLine[x] = _stitchPalette.TransferToRear;

                        GH_Colour transferType = targetIndex > x ?
                            diff == 1 ? _stitchPalette.RightRearToFront1 :
                            diff == 2 ? _stitchPalette.RightRearToFront2 : _stitchPalette.RightRearToFront3 :
                            diff == 1 ? _stitchPalette.LeftRearToFront1 :
                            diff == 2 ? _stitchPalette.LeftRearToFront2 : _stitchPalette.LeftRearToFront3;

                        // add the index to the appropriate transfer type in the dictionary
                        if (previousTransferIndexDict.ContainsKey(transferType)) previousTransferIndexDict[transferType].Add(x);
                        else previousTransferIndexDict[transferType] = new List<int> { x };
                    }
                    // going from miss to front
                    else if (current == _stitchPalette.Miss.Value.ToArgb() && nextStitchLine != null && nextStitchLine[x].Value.ToArgb() == _stitchPalette.Front.Value.ToArgb())
                    {
                        // need to transfer the closest front stitch to back and then to the current position
                        // find the closest point to transfer from
                        int currentOnRight = Array.IndexOf(currentStitchLine
                            .Where((_, i) => i > x && i < x + 4)
                            .Select(c => c.Value.ToArgb())
                            .ToArray(), _stitchPalette.Front.Value.ToArgb()) + 1;

                        int currentOnLeft = Array.IndexOf(currentStitchLine
                            .Where((_, i) => i < x && i > x - 4)
                            .Reverse()
                            .Select(c => c.Value.ToArgb())
                            .ToArray(), _stitchPalette.Front.Value.ToArgb()) + 1;

                        if (currentOnRight == 0 && currentOnLeft == 0) continue;
                        changedNext = true;

                        // define which index will be used as a source
                        int sourceIndex = currentOnRight <= currentOnLeft ? x + currentOnRight : x - currentOnLeft;
                        int diff = Math.Min(currentOnRight, currentOnLeft);

                        nextToBackLine[sourceIndex] = _stitchPalette.TransferToRear;

                        GH_Colour transferType = sourceIndex > x ?
                            diff == 1 ? _stitchPalette.LeftRearToFront1 :
                            diff == 2 ? _stitchPalette.LeftRearToFront2 : _stitchPalette.LeftRearToFront3 :
                            diff == 1 ? _stitchPalette.RightRearToFront1 :
                            diff == 2 ? _stitchPalette.RightRearToFront2 : _stitchPalette.RightRearToFront3;

                        // add the index to the appropriate transfer type in the dictionary
                        if (nextTransferIndexDict.ContainsKey(transferType)) nextTransferIndexDict[transferType].Add(sourceIndex);
                        else nextTransferIndexDict[transferType] = new List<int> { sourceIndex };
                    }
                }

                if ((previousToBackLine.Length == 0 && previousTransferIndexDict.Count == 0) ||
                    (nextToBackLine.Length == 0 && nextTransferIndexDict.Count == 0))
                {
                    // if there were no transfers calculates (in case of unsupported transfer type)
                    stitchesWithTransfers.Add(currentStitchLine);
                    yarnsWithTransfers.Add(currentYarnLine);
                    continue;
                }
                else
                {
                    if (changedPrevious)
                    {
                        // detect if there are neighboring cells that are moving to back on the previous lines
                        // create an array to store the moved transfers
                        GH_Colour[] toPrevSecondBackLine = new GH_Colour[width];
                        bool movedToSecond = false;
                        for (int i = 1; i < previousToBackLine.Length; i++)
                        {
                            GH_Colour current = previousToBackLine[i];
                            GH_Colour previous = previousToBackLine[i - 1];

                            // check if the neighbors are the same
                            if (current != null && previous != null && current == previous)
                            {
                                // move transfer to second array
                                toPrevSecondBackLine[i] = current;
                                // remove the transfer from main array
                                previousToBackLine[i] = null;
                                movedToSecond = true;
                            }
                        }

                        // add the transfers to the back lines
                        stitchesWithTransfers.Add(previousToBackLine);
                        if (movedToSecond) stitchesWithTransfers.Add(toPrevSecondBackLine);

                        // create the transfer lines, per type
                        List<GH_Colour[]> newPrevTransferLines = new List<GH_Colour[]>();
                        foreach (var item in previousTransferIndexDict)
                        {
                            var transferType = item.Key;
                            var indices = item.Value;

                            GH_Colour[] transferLine = new GH_Colour[width];
                            foreach (int index in indices) transferLine[index] = transferType;
                            newPrevTransferLines.Add(transferLine);
                        }

                        // add the lateral transfers 
                        stitchesWithTransfers.AddRange(newPrevTransferLines);

                        // add yarn transfer lines for each new line that is being added to stitches
                        int toBackLineCount = movedToSecond ? 2 : 1;
                        for (int i = 0; i < toBackLineCount + newPrevTransferLines.Count; i++)
                        {
                            yarnsWithTransfers.Add(yarnTransferLine);
                        }
                    }

                    // add current line when transfers for it have been implemented
                    stitchesWithTransfers.Add(currentStitchLine);

                    // add the current yarn line
                    yarnsWithTransfers.Add(currentYarnLine);

                    if (changedNext)
                    {
                        GH_Colour[] toNextSecondBackLine = new GH_Colour[width];
                        bool movedToSecond = false;
                        for (int i = 0; i < nextToBackLine.Length - 1; i++)
                        {
                            GH_Colour current = nextToBackLine[i];
                            GH_Colour next = nextToBackLine[i + 1];

                            // check if the neighbors are the same
                            if (current != null && next != null && current == next)
                            {
                                // move transfer to second array
                                toNextSecondBackLine[i] = current;
                                // remove the transfer from main array
                                nextToBackLine[i] = null;
                                movedToSecond = true;
                            }
                        }

                        // add the transfers to the back lines
                        stitchesWithTransfers.Add(nextToBackLine);
                        if (movedToSecond) stitchesWithTransfers.Add(toNextSecondBackLine);

                        // create the transfer lines, per type
                        List<GH_Colour[]> newNextTransferLines = new List<GH_Colour[]>();
                        foreach (var item in nextTransferIndexDict)
                        {
                            var transferType = item.Key;
                            var indices = item.Value;

                            GH_Colour[] transferLine = new GH_Colour[width];
                            foreach (int index in indices) transferLine[index] = transferType;
                            newNextTransferLines.Add(transferLine);
                        }

                        // add the lateral transfers 
                        stitchesWithTransfers.AddRange(newNextTransferLines);

                        // add yarn transfer lines for each new line that is being added to stitches
                        int toBackLineCount = movedToSecond ? 2 : 1;
                        for (int i = 0; i < toBackLineCount + newNextTransferLines.Count; i++)
                        {
                            yarnsWithTransfers.Add(yarnTransferLine);
                        }
                    }
                }
            }

            // organize the result
            // stitches are created in reverse order, so they need to be reversed here
            stitchesWithTransfers.Reverse();
            yarnsWithTransfers.Reverse();

            // create the resulting 2D arrays
            GH_Colour[,] resultStitches = new GH_Colour[width, stitchesWithTransfers.Count];
            GH_Colour[,] resultYarns = new GH_Colour[width, yarnsWithTransfers.Count];

            // add the elements to the resulting arrays
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < stitchesWithTransfers.Count; y++)
                {
                    resultStitches[x, y] = stitchesWithTransfers[y][x] == null ? _stitchPalette.Miss : stitchesWithTransfers[y][x];
                }

                for (int y = 0; y < yarnsWithTransfers.Count; y++)
                {
                    resultYarns[x, y] = yarnsWithTransfers[y][x];
                }
            }

            return (resultStitches, resultYarns);
        }

        private GH_Colour[] GetLine(GH_Colour[,] patterns, int y)
        {
            int width = patterns.GetLength(0);
            GH_Colour[] result = new GH_Colour[width];
            for (int i = 0; i < width; i++)
            {
                result[i] = patterns[i, y];
            }
            return result;
        }


        private bool AreLinesEqual(GH_Colour[] a, GH_Colour[] b)
        {
            if (a == null || b == null) return false;
            if (a.Length != b.Length) return false;

            int width = a.Length;

            for (int i = 0; i < width; i++)
            {
                if (a[i].Value.ToArgb() != b[i].Value.ToArgb()) return false;
            }
            return true;
        }

        private GH_Colour[] GetStichLine(int y)
        {
            int height = _stitchPatterns.GetLength(1);
            if (y > height - 1 || y < 0) return null;
            int width = _stitchPatterns.GetLength(0);
            GH_Colour[] result = new GH_Colour[width];

            for (int i = 0; i < width; i++)
            {
                result[i] = _stitchPatterns[i, y];
            }

            return result;
        }

        private GH_Colour[] GetYarnLine(int y)
        {
            int height = _yarnPatterns.GetLength(1);
            if (y > height - 1) return null;
            int width = _yarnPatterns.GetLength(0);
            GH_Colour[] result = new GH_Colour[width];

            for (int i = 0; i < width; i++)
            {
                result[i] = _yarnPatterns[i, y];
            }

            return result;
        }

        # endregion Handle pattern changes
    }
}
