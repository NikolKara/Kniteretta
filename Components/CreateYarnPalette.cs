using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using System;
using System.Collections.Generic;

namespace Kniteretta.Components
{
    public class CreateYarnPalette : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the CreateYarnPalette class.
        /// </summary>
        public CreateYarnPalette()
          : base("CreateYarnPalette", "YarnPalette",
              "Creates a colour palette that matches colours with yarns",
              "Kniteretta", "Parameters")
        {
        }

        // Store the parameter indexes
        private int _yarn0;
        private int _yarn1;
        private int _yarn2;
        private int _yarn3;
        private int _yarn4;
        private int _yarn5;
        private int _yarn6;

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            _yarn0 = pManager.AddColourParameter("Yarn 0", "Y0", "Colour for yarn 0", GH_ParamAccess.item);
            _yarn1 = pManager.AddColourParameter("Yarn 1", "Y1", "Colour for yarn 1", GH_ParamAccess.item);
            _yarn2 = pManager.AddColourParameter("Yarn 2", "Y2", "Colour for yarn 2", GH_ParamAccess.item);
            _yarn3 = pManager.AddColourParameter("Yarn 3", "Y3", "Colour for yarn 3", GH_ParamAccess.item);
            _yarn4 = pManager.AddColourParameter("Yarn 4", "Y4", "Colour for yarn 4", GH_ParamAccess.item);
            _yarn5 = pManager.AddColourParameter("Yarn 5", "Y5", "Colour for yarn 5", GH_ParamAccess.item);
            _yarn6 = pManager.AddColourParameter("Yarn 6", "Y6", "Colour for yarn 6", GH_ParamAccess.item);

            // Make all inputs optional
            for (int i = 0; i < pManager.ParamCount; i++)
            {
                pManager[i].Optional = true;
            }
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Palette", "P", "Yarn colour palette", GH_ParamAccess.item);
            pManager.AddColourParameter("AsList", "L", "List of generated colours", GH_ParamAccess.list);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            // Create the default palette
            KYarnPalette palette = new KYarnPalette();

            // Assign the component values to the palette
            GH_Colour y0 = new GH_Colour();
            if (DA.GetData(_yarn0, ref y0)) palette.Yarn0 = y0;

            GH_Colour y1 = new GH_Colour();
            if (DA.GetData(_yarn1, ref y1)) palette.Yarn1 = y1;

            GH_Colour y2 = new GH_Colour();
            if (DA.GetData(_yarn2, ref y2)) palette.Yarn2 = y2;

            GH_Colour y3 = new GH_Colour();
            if (DA.GetData(_yarn3, ref y3)) palette.Yarn3 = y3;

            GH_Colour y4 = new GH_Colour();
            if (DA.GetData(_yarn4, ref y4)) palette.Yarn4 = y4;

            GH_Colour y5 = new GH_Colour();
            if (DA.GetData(_yarn5, ref y5)) palette.Yarn5 = y5;

            GH_Colour y6 = new GH_Colour();
            if (DA.GetData(_yarn6, ref y6)) palette.Yarn6 = y6;

            // Create a list of the palette's output, for ease of output visualisation
            List<GH_Colour> list = new List<GH_Colour>()
            {
                palette.Yarn0, palette.Yarn1, palette.Yarn2, palette.Yarn3, palette.Yarn4, palette.Yarn5, palette.Yarn6
            };

            // Add error message and return if the palette is invalid
            if (!palette.IsValid())
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "All yarns must have unique colours");
                return;
            }

            // Set the output results
            DA.SetData(0, palette);
            DA.SetDataList(1, list);
        }

        /// <summary>
        /// Provides an Icon for the component.
        /// </summary>
        protected override System.Drawing.Bitmap Icon => Properties.Resources.YPalette;

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("04F5CB82-445A-4193-BF78-90300E7E6797"); }
        }
    }
}