using Grasshopper.Kernel;
using System;
using System.Collections.Generic;

namespace Kniteretta.Components
{
    public class SetPieceParameters : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the MyComponent1 class.
        /// </summary>
        public SetPieceParameters()
          : base("Set Piece Parameters", "SetPieceParams",
              "Defines the piece parameters - optional properties that help identify the project properties",
              "Kniteretta", "Parameters")
        {
        }

        // Parameter indexes
        private int _shapeFilename;
        private int _pieceDescription;
        private int _mainStitches;
        private int _mainRows;
        private int _ribStitches;
        private int _ribRows;
        private int _ribStitchDensity;
        private int _ribRowDensity;
        private int _stitchDensity;
        private int _rowDensity;

        private int _knitRacking;
        private int _knitSpeed;
        private int _knitRoller;
        private int _knitFrontSize;
        private int _knitRearSize;
        private int _knitDirection;

        private int _transferRacking;
        private int _transferSpeed;
        private int _transferRoller;
        private int _transferFrontSize;
        private int _transferRearSize;
        private int _transferDirection;

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            _shapeFilename = pManager.AddTextParameter("Shape filename", "filename", "Sets the project's file name", GH_ParamAccess.item);
            _pieceDescription = pManager.AddTextParameter("Piece description", "description", "Sets the project's description", GH_ParamAccess.item);
            _mainStitches = pManager.AddTextParameter("Main stitches", "mainStitches", "Number of Stitches (Width)", GH_ParamAccess.item);
            _mainRows = pManager.AddTextParameter("Main rows", "mainRows", "Number of Rows (Height)", GH_ParamAccess.item);
            _ribStitches = pManager.AddTextParameter("Rib stitches", "ribStitches", "Number of Stitches in Rib (Width)", GH_ParamAccess.item);
            _ribRows = pManager.AddTextParameter("Rib rows", "ribRows", "Number of Rows in Rib (Height)", GH_ParamAccess.item);
            _ribStitchDensity = pManager.AddTextParameter("Rib stitch density", "ribStitchDensity", "Number of stitches in 10 cm for the Rib", GH_ParamAccess.item);
            _ribRowDensity = pManager.AddTextParameter("Rib row density", "ribRowDensity", "Number of rows in 10 cm for the Rib", GH_ParamAccess.item);
            _stitchDensity = pManager.AddTextParameter("Stitch density", "stitchDensity", "Number of stitches in 10 cm", GH_ParamAccess.item);
            _rowDensity = pManager.AddTextParameter("Row density", "rowDensity", "Number of stitches in 10 cm", GH_ParamAccess.item);
            
            _knitRacking = pManager.AddTextParameter("Knit racking", "knitRacking", "Stitch option: knit racking", GH_ParamAccess.item);
            _knitSpeed = pManager.AddTextParameter("Knit speed", "knitSpeed", "Stitch option: knit speed", GH_ParamAccess.item);
            _knitRoller = pManager.AddTextParameter("Knit roller", "knitRoller", "Stitch option: knit roller", GH_ParamAccess.item);
            _knitFrontSize = pManager.AddTextParameter("Knit front size", "knitFrontSize", "Stitch option: knit front stitch size", GH_ParamAccess.item);
            _knitRearSize = pManager.AddTextParameter("Knit rear size", "knitRearSize", "Stitch option: knit rear stitch size", GH_ParamAccess.item);
            _knitDirection = pManager.AddTextParameter("Knit direction", "knitDirection", "Stitch option: knit direction", GH_ParamAccess.item);

            _transferRacking = pManager.AddTextParameter("Transfer racking", "transferRacking", "Stitch option: transfer racking", GH_ParamAccess.item);
            _transferSpeed = pManager.AddTextParameter("Transfer speed", "transferSpeed", "Stitch option: transfer speed", GH_ParamAccess.item);
            _transferRoller = pManager.AddTextParameter("Transfer roller", "transferRoller", "Stitch option: transfer roller", GH_ParamAccess.item);
            _transferFrontSize = pManager.AddTextParameter("Transfer front size", "transferFrontSize", "Stitch option: transfer front stitch size", GH_ParamAccess.item);
            _transferRearSize = pManager.AddTextParameter("Transfer rear size", "transferRearSize", "Stitch option: transfer rear stitch size", GH_ParamAccess.item);
            _transferDirection = pManager.AddTextParameter("Transfer direction", "transferDirection", "Stitch option: transfer direction", GH_ParamAccess.item);

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
            pManager.AddGenericParameter("Out parameters", "params", "Defined parameters", GH_ParamAccess.item);
            pManager.AddTextParameter("AsList", "L", "List of defined parameters", GH_ParamAccess.list);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            KParameters p = new KParameters();

            string filename = "";
            if (DA.GetData(_shapeFilename, ref filename)) p.ShapeFilename = filename;

            string description = "";
            if (DA.GetData(_pieceDescription, ref description)) p.PieceDescription = description;

            string mainStitches = "";
            if (DA.GetData(_mainStitches, ref mainStitches)) p.MainStitches = mainStitches;

            string mainRows = "";
            if (DA.GetData(_mainRows, ref mainRows)) p.MainRows = mainRows;

            string ribStitches = "";
            if (DA.GetData(_ribStitches, ref ribStitches)) p.RibStitches = ribStitches;

            string ribRows = "";
            if (DA.GetData(_ribRows, ref ribRows)) p.RibRows = ribRows;

            string ribStitchDensity = "";
            if (DA.GetData(_ribStitchDensity, ref ribStitchDensity)) p.RibStitchDensity = ribStitchDensity;

            string ribRowDensity = "";
            if (DA.GetData(_ribRowDensity, ref ribRowDensity)) p.RibRowDensity = ribRowDensity;

            string stitchDensity = "";
            if (DA.GetData(_stitchDensity, ref stitchDensity)) p.StitchDensity = stitchDensity;

            string rowDensity = "";
            if (DA.GetData(_rowDensity, ref rowDensity)) p.RowDensity = rowDensity;

            string knitRacking = "";
            if (DA.GetData(_knitRacking, ref knitRacking)) p.KnitRacking = knitRacking;

            string knitSpeed = "";
            if (DA.GetData(_knitSpeed, ref knitSpeed)) p.KnitSpeed= knitSpeed;

            string knitRoller = "";
            if (DA.GetData(_knitRoller, ref knitRoller)) p.KnitRoller = knitRoller;

            string knitFrontSize = "";
            if (DA.GetData(_knitFrontSize, ref knitFrontSize)) p.KnitFrontSize = knitFrontSize;
            
            string knitRearSize = "";
            if (DA.GetData(_knitRearSize, ref knitRearSize)) p.KnitRearSize = knitRearSize;

            string knitDirection = "";
            if (DA.GetData(_knitDirection, ref knitDirection)) p.KnitDirection = knitDirection;


            string transferRacking = "";
            if (DA.GetData(_transferRacking, ref transferRacking)) p.TransferRacking = transferRacking;

            string transferSpeed = "";
            if (DA.GetData(_transferSpeed, ref transferSpeed)) p.TransferSpeed = transferSpeed;

            string transferRoller = "";
            if (DA.GetData(_transferRoller, ref transferRoller)) p.TransferRoller = transferRoller;

            string transferFrontSize = "";
            if (DA.GetData(_transferFrontSize, ref transferFrontSize)) p.TransferFrontSize = transferFrontSize;

            string transferRearSize = "";
            if (DA.GetData(_transferRearSize, ref transferRearSize)) p.TransferRearSize = transferRearSize;

            string transferDirection = "";
            if (DA.GetData(_transferDirection, ref transferDirection)) p.TransferDirection = transferDirection;


            DA.SetData(0, p);

            List<string> list = new List<string>()
            {
                p.ShapeFilename,
                p.PieceDescription,
                p.MainStitches,
                p.MainRows, 
                p.RibStitches,
                p.RibRows,
                p.RibStitchDensity,
                p.RibRowDensity,
                p.StitchDensity,
                p.RowDensity,
                p.KnitRacking, p.KnitSpeed, p.KnitRoller, p.KnitFrontSize, p.KnitRearSize, p.KnitDirection,
                p.TransferRacking, p.TransferSpeed, p.TransferRoller, p.TransferFrontSize, p.TransferRearSize, p.TransferDirection
            };

            DA.SetDataList(1, list);
        }

        /// <summary>
        /// Provides an Icon for the component.
        /// </summary>
        protected override System.Drawing.Bitmap Icon => Properties.Resources.Parameters;

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("2CF45277-D70F-410D-BE35-7D5AE195F024"); }
        }
    }
}