using Grasshopper;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Data;
using Grasshopper.Kernel.Types;
using System;

namespace Kniteretta
{
    public class LoadImage : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the MyComponent1 class.
        /// </summary>
        public LoadImage()
          : base("Load image", "LoadImg",
              "Loads an images from a path into a bidimensional colour array",
              "Kniteretta", "Load")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("Path", "path", "Path of the image to load", GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddColourParameter("Colours", "colours", "Bidemensional array (tree) of the colours read", GH_ParamAccess.tree);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            string path = "";
            if (!DA.GetData(0, ref path)) return;

            GH_Colour[,] arr = Util.LoadImage(path);

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
            get { return new Guid("66DC40DA-2C0F-4864-B0F8-38B36D3C3C2A"); }
        }
    }
}