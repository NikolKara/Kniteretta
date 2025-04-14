using Grasshopper.Kernel.Types;
using System.Collections.Generic;
using System.Drawing;

namespace Kniteretta
{
    /// <summary>
    /// Object that contains the colour definitions for the yarn template palette
    /// </summary>
    public class KYarnPalette
    {
        public GH_Colour Yarn0;
        public GH_Colour Yarn1;
        public GH_Colour Yarn2;
        public GH_Colour Yarn3;
        public GH_Colour Yarn4;
        public GH_Colour Yarn5;
        public GH_Colour Yarn6;

        /// <summary>
        /// Creates the initial colour palette, utilising colours from https://sashamaps.net/docs/resources/20-colors/
        /// as a base. Specific types can be updated by modifying its specific properties
        /// </summary>
        public KYarnPalette()
        {
            Yarn0 = new GH_Colour(Color.FromArgb(170, 255, 195)); // mint
            Yarn1 = new GH_Colour(Color.FromArgb(128,0,0)); // maroon
            Yarn2 = new GH_Colour(Color.FromArgb(170, 110, 40)); // brown
            Yarn3 = new GH_Colour(Color.FromArgb(128, 128, 0)); // olive
            Yarn4 = new GH_Colour(Color.FromArgb(0,128,128)); // teal
            Yarn5 = new GH_Colour(Color.FromArgb(0,0,128)); // navy
            Yarn6 = new GH_Colour(Color.FromArgb(220, 190, 255)); // lavender
        }

        /// <summary>
        /// Checks if this palette is valid by ensuring that all values are unique
        /// </summary>
        /// <returns></returns>
        public bool IsValid()
        {
            List<GH_Colour> list = new List<GH_Colour>()
            {
                Yarn0, Yarn1, Yarn2, Yarn3, Yarn4, Yarn5, Yarn6
            };

            HashSet<GH_Colour> set = new HashSet<GH_Colour>(list);

            return list.Count == set.Count;
        }
    }
}
