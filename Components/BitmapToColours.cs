using Grasshopper.Kernel.Data;
using Grasshopper.Kernel.Types;
using Grasshopper.Kernel;
using Grasshopper;
using System;
using System.Drawing;

namespace Kniteretta
{
    public class BitmapToColours : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the MyComponent1 class.
        /// </summary>
        public BitmapToColours()
          : base("Bitmap to colours", "LoadImg",
              "Converts a bitmap image into a bidimensional colour array",
              "Kniteretta", "Load")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Bitmap", "bmp", "Bitmap image to convert", GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddColourParameter("Colours", "colours", "Bidimensional array (tree) of the colours read", GH_ParamAccess.tree);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            Bitmap bmp = null;
            if (!DA.GetData(0, ref bmp)) return;


            GH_Colour[,] arr = Util.ColoursFromBmp(bmp);

            DataTree<GH_Colour> tree = new DataTree<GH_Colour>();
            for (int i = 0; i < arr.GetLength(0); i++)
            {
                GH_Path p = new GH_Path(i);
                for (int j = 0; j < arr.GetLength(1); j++)
                {
                    tree.Add(arr[i, j], p);
                }
            }

            DA.SetDataTree(0, tree);
        }

        /// <summary>
        /// Provides an Icon for the component.
        /// </summary>
        protected override System.Drawing.Bitmap Icon => Properties.Resources.Image;

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("E666364D-9C06-4FF2-95FE-2E92AFC10610"); }
        }
    }
}
