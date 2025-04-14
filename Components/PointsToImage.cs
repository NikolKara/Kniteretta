using System;
using System.Linq;
using System.Collections.Generic;
using GH_IO.Types;
using Grasshopper;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Data;
using Grasshopper.Kernel.Types;
using Rhino.Geometry;
using System.Drawing;

namespace Kniteretta.Components
{
    public class PointsToImage : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public PointsToImage()
          : base("Points to image", "PntsToImg",
              "Converts collections of points to a bitmap image. " +
                "Points must be passed as groups, where each group represent a colour. " +
                "Points must also be scaled to a 1x1 m grid, where 1 point = 1 pixel in the final image",
              "Kniteretta", "Load")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddPointParameter("Points", "points", "Collection of points, grouped by colour", GH_ParamAccess.tree);
            pManager.AddColourParameter("Colours", "colours", "Collection of colours, ordered the same as the points", GH_ParamAccess.list);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Bitmap", "bmp", "Resulting bitmap from points", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            GH_Structure<GH_Point> points = new GH_Structure<GH_Point>();
            List<GH_Colour> colours = new List<GH_Colour>();
            if (!DA.GetDataTree(0, out points)) return;
            if (!DA.GetDataList(1, colours)) return;

            if (points.Branches.Count != colours.Count)
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Number of colours does not match number of point groups");
                return;
            }

            var flatPoints = points.AllData(true).Select(x => x as GH_Point).ToArray();
            
            int minX = flatPoints.Min(x => (int)x.Value.X);
            int maxX = flatPoints.Max(x => (int)x.Value.X);
            
            int minY = flatPoints.Min(x => (int)x.Value.Y);
            int maxY = flatPoints.Max(x => (int)x.Value.Y);

            int width = maxX - minX + 1;
            int height = maxY - minY + 1;

            Bitmap bitmap = new Bitmap(width, height);
            for (int i = 0; i < bitmap.Width; i++)
            {
                for (int j = 0; j < bitmap.Height; j++)
                {
                    bitmap.SetPixel(i, j, Color.Black);
                }
            }

            for (int i = 0; i < points.Branches.Count; i++)
            {
                IList<GH_Point> colourBlock = points.Branches[i];
                GH_Colour colour = colours[i];

                foreach (GH_Point ghPnt in colourBlock)
                {
                    Point3d pnt = ghPnt.Value;
                    int x = (int) Math.Round(pnt.X);
                    int y = (int)Math.Round(pnt.Y);
                    bitmap.SetPixel(x - minX, maxY - (y - minY), colour.Value);
                }
            }

            DA.SetData(0, bitmap);
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
            get { return new Guid("0BE4F066-11CD-4A63-BF51-DE771AEBA921"); }
        }
    }
}

