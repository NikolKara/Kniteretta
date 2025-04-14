using Grasshopper.Kernel.Types;
using System.Collections.Generic;
using System.Drawing;

namespace Kniteretta
{
    /// <summary>
    /// Object that contains the colour definitions for the stitch template palette
    /// </summary>
    public class KStitchPalette
    {
        public GH_Colour Front;
        public GH_Colour Rear;
        public GH_Colour Both;

        public GH_Colour TuckOnFront;
        public GH_Colour TuckOnRear;
        public GH_Colour KnitFrontTuckRear;
        public GH_Colour TuckFrontKnitRear;

        public GH_Colour Slip;

        public GH_Colour SplitOnFront;
        public GH_Colour SplitOnRear;

        public GH_Colour TransferToRear;
        public GH_Colour TransferToFront;
        public GH_Colour Miss;

        public GH_Colour LeftFrontToRear1;
        public GH_Colour LeftFrontToRear2;
        public GH_Colour LeftFrontToRear3;

        public GH_Colour RightFrontToRear1;
        public GH_Colour RightFrontToRear2;
        public GH_Colour RightFrontToRear3;
        
        public GH_Colour LeftRearToFront1;
        public GH_Colour LeftRearToFront2;
        public GH_Colour LeftRearToFront3;

        public GH_Colour RightRearToFront1;
        public GH_Colour RightRearToFront2;
        public GH_Colour RightRearToFront3;

        /// <summary>
        /// Creates the initial colour palette, utilising colours from https://sashamaps.net/docs/resources/20-colors/
        /// as a base. Specific types can be updated by modifying its specific properties
        /// </summary>
        public KStitchPalette()
        {
            Front = new GH_Colour(Color.FromArgb(230, 25, 75)); // red
            Rear = new GH_Colour(Color.FromArgb(245, 130, 48)); // orange
            Both = new GH_Colour(Color.FromArgb(255, 255, 25)); // yellow

            TuckOnFront = new GH_Colour(Color.FromArgb(210, 245, 60)); // lime
            TuckOnRear = new GH_Colour(Color.FromArgb(255, 0, 0)); // pure red
            KnitFrontTuckRear = new GH_Colour(Color.FromArgb(0, 255, 0)); // pure green
            TuckFrontKnitRear = new GH_Colour(Color.FromArgb(0, 0, 255)); // pure blue

            Slip = new GH_Colour(Color.FromArgb(128, 128, 128)); // grey

            SplitOnFront = new GH_Colour(Color.FromArgb(50, 50, 50)); // dark grey
            SplitOnRear = new GH_Colour(Color.FromArgb(190, 190, 190)); // light grey

            TransferToRear = new GH_Colour(Color.FromArgb(60, 180, 75)); // green
            TransferToFront = new GH_Colour(Color.FromArgb(70, 240, 240)); // cyan
            Miss = new GH_Colour(Color.FromArgb(0, 0, 0)); // black
            
            LeftFrontToRear1 = new GH_Colour(Color.FromArgb(0, 130, 200)); // blue
            LeftFrontToRear2 = new GH_Colour(Color.FromArgb(0, 0, 127)); // navy
            LeftFrontToRear3 = new GH_Colour(Color.FromArgb(0, 128, 128)); // teal

            RightFrontToRear1 = new GH_Colour(Color.FromArgb(145, 30, 180)); // purple
            RightFrontToRear2 = new GH_Colour(Color.FromArgb(240, 50, 230)); // magenta
            RightFrontToRear3 = new GH_Colour(Color.FromArgb(220, 190, 255)); // lavender

            LeftRearToFront1 = new GH_Colour(Color.FromArgb(128, 0, 0)); // maroon
            LeftRearToFront2 = new GH_Colour(Color.FromArgb(170, 110, 40)); // brown
            LeftRearToFront3 = new GH_Colour(Color.FromArgb(128, 128, 0)); // olive

            RightRearToFront1 = new GH_Colour(Color.FromArgb(250, 190, 212)); // pink
            RightRearToFront2 = new GH_Colour(Color.FromArgb(255, 215, 180)); // apricot
            RightRearToFront3 = new GH_Colour(Color.FromArgb(170, 255, 195)); // mint
        }

        /// <summary>
        /// Checks if this palette is valid by ensuring that all values are unique
        /// </summary>
        /// <returns></returns>
        public bool IsValid()
        {
            List<int> list = new List<int>()
            {
                Front.Value.ToArgb(),
                Rear.Value.ToArgb(),
                Both.Value.ToArgb(),
                TuckOnFront.Value.ToArgb(),
                TuckOnRear.Value.ToArgb(),
                KnitFrontTuckRear.Value.ToArgb(),
                TuckFrontKnitRear.Value.ToArgb(),
                Slip.Value.ToArgb(),
                SplitOnFront.Value.ToArgb(),
                SplitOnRear.Value.ToArgb(),
                TransferToRear.Value.ToArgb(),
                TransferToFront.Value.ToArgb(),
                Miss.Value.ToArgb(),
                LeftFrontToRear1.Value.ToArgb(),
                LeftFrontToRear2.Value.ToArgb(),
                LeftFrontToRear3.Value.ToArgb(),
                RightFrontToRear1.Value.ToArgb(),
                RightFrontToRear2.Value.ToArgb(),
                RightFrontToRear3.Value.ToArgb(),
                LeftRearToFront1.Value.ToArgb(),
                LeftRearToFront2.Value.ToArgb(),
                LeftRearToFront3.Value.ToArgb(),
                RightRearToFront1.Value.ToArgb(),
                RightRearToFront2.Value.ToArgb(),
                RightRearToFront3.Value.ToArgb()
            };

            HashSet<int> set = new HashSet<int>(list);

            return list.Count == set.Count;
        }
    }
}
