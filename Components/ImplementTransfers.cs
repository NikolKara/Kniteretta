using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Grasshopper;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Data;
using Grasshopper.Kernel.Types;
using Rhino.Geometry;

namespace Kniteretta.Components
{
    public class ImplementTransfers : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the MyComponent1 class.
        /// </summary>
        public ImplementTransfers()
          : base("Implement Transfers", "Transfers",
              "Detects and implements transfers on the input images",
              "Kniteretta", "Generate")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            // input images
            pManager.AddColourParameter("Stitches", "S", "2D array of stitches, assigned as colours", GH_ParamAccess.tree);
            pManager.AddColourParameter("Yarn", "Y", "2D array of stitches, assigned as colours", GH_ParamAccess.tree);

            // palettes
            int sPalette = pManager.AddGenericParameter("Stitch palette", "sPalette", "[Optional] Palette of stitch colours, mapping colours to stitch types", GH_ParamAccess.item);
            int yPalette = pManager.AddGenericParameter("Yarn palette", "yPalette", "[Optional] Palette of yarn colours, mapping colours to yarns", GH_ParamAccess.item);

            pManager[sPalette].Optional = true;
            pManager[yPalette].Optional = true;
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            //pManager.AddGenericParameter("Output", "out", "The resulting command file", GH_ParamAccess.item);
            pManager.AddColourParameter("Stitch colours", "S", "Bidimensional array (tree) of the stitch colours read", GH_ParamAccess.tree);
            pManager.AddColourParameter("Yarn colours", "Y", "Bidimensional array (tree) of the yarn colours read", GH_ParamAccess.tree);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var time = DateTime.Now.Millisecond;
            // get the stitch colour pattern
            if (!DA.GetDataTree<GH_Colour>(0, out GH_Structure<GH_Colour> stitches)) return;
            if (!DA.GetDataTree<GH_Colour>(1, out GH_Structure<GH_Colour> yarns)) return;

            // optionally get the stitch palette
            KStitchPalette sPalette = new KStitchPalette();
            if (!DA.GetData(2, ref sPalette)) sPalette = new KStitchPalette();
            
            // optionally get the yarn palette
            KYarnPalette yPalette = new KYarnPalette();
            if (!DA.GetData(3, ref yPalette)) yPalette = new KYarnPalette();

            // Convert trees to 2D arrays
            GH_Colour[,] stitchesArray = Util.TreeTo2D(stitches);
            GH_Colour[,] yarnsArray = Util.TreeTo2D(yarns);

            // Create generator and implement transfers
            KGenerator generator = new KGenerator(sPalette, stitchesArray, yPalette, yarnsArray, new KParameters());
            (GH_Colour[,] newStitches, GH_Colour[,] newYarns) = generator.ImplementTransfers();

            // Convert 2D arrays back to trees
            DataTree<GH_Colour> stitchesTree = new DataTree<GH_Colour>();
            DataTree<GH_Colour> yarnsTree = new DataTree<GH_Colour>();

            for (int i = 0; i < newStitches.GetLength(0); i++)
            {
                GH_Path sPath = new GH_Path(i);
                GH_Path yPath = new GH_Path(i);
                for (int j = 0; j < newStitches.GetLength(1); j++)
                {
                    stitchesTree.Add(newStitches[i, j], sPath);
                    yarnsTree.Add(newYarns[i, j], yPath);
                }
            };

            DA.SetDataTree(0, stitchesTree);
            DA.SetDataTree(1, yarnsTree);
            double elapsed = DateTime.Now.Millisecond - time;
        }

        /// <summary>
        /// Provides an Icon for the component.
        /// </summary>
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                //You can add image files to your project resources and access them like this:
                // return Resources.IconForThisComponent;
                return null;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("7EE6870F-E16E-4381-B083-9F1EEC69574E"); }
        }
    }
}