using Grasshopper.Kernel;
using Grasshopper.Kernel.Data;
using Grasshopper.Kernel.Types;
using System;

namespace Kniteretta
{
    public class GenerateKCode : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the ImagesToText class.
        /// </summary>
        public GenerateKCode()
          : base("Generate K-code", "GenKCode",
              "Generates K-code data from stitch and yarn data",
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

            // parameters
            int parameters = pManager.AddGenericParameter("Custom parameters", "params", "[Optional] Custom parameters that define the properties of the project", GH_ParamAccess.item);
            pManager[parameters].Optional = true;
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("Output", "out", "The resulting command file", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            // get the stitch colour pattern
            if (!DA.GetDataTree<GH_Colour>(0, out GH_Structure<GH_Colour> stitches)) return;

            // get the yarn colour pattern
            if (!DA.GetDataTree<GH_Colour>(1, out GH_Structure<GH_Colour> yarns)) return;

            // optionally get the stitch palette
            KStitchPalette sPalette = new KStitchPalette();
            if (!DA.GetData(2, ref sPalette)) sPalette = new KStitchPalette();

            // optionally get the yarn palette
            KYarnPalette yPalette = new KYarnPalette();
            if (!DA.GetData(3, ref yPalette)) yPalette = new KYarnPalette();

            // optionally get the custom parameters
            KParameters kParams = new KParameters();
            if (!DA.GetData(4, ref kParams)) kParams = new KParameters();

            KGenerator generator = new KGenerator(sPalette, Util.TreeTo2D(stitches), yPalette, Util.TreeTo2D(yarns), kParams);
            generator.Generate();

            string result = generator.Writer.Output;

            DA.SetData(0, result);
        }

        /// <summary>
        /// Provides an Icon for the component.
        /// </summary>
        protected override System.Drawing.Bitmap Icon => Properties.Resources.GenerateKCode;

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("8F59AFE4-35B8-4481-8571-CA8A45CD76F1"); }
        }
    }
}