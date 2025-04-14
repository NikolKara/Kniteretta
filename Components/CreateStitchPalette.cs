using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using System;
using System.Collections.Generic;

namespace Kniteretta
{
    public class CreateStitchPalette : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the CreateColourPalette class.
        /// </summary>
        public CreateStitchPalette()
          : base("Create stitch palette", "StitchPalette",
              "Creates a colour palette that matches colours with stitch patterns",
              "Kniteretta", "Parameters")
        {
        }

        // parameter indexes
        private int _front;
        private int _rear;
        private int _both;

        private int _tuckOnFront;
        private int _tuckOnRear;
        private int _knitFrontTuckRear;
        private int _tuckFrontKnitRear;

        private int _slip;

        private int _splitOnFront;
        private int _splitOnRear;

        private int _transferToRear;
        private int _transferToFront;
        private int _miss;

        private int _leftFrontToRear1;
        private int _leftFrontToRear2;
        private int _leftFrontToRear3;

        private int _rightFrontToRear1;
        private int _rightFrontToRear2;
        private int _rightFrontToRear3;

        private int _leftRearToFront1;
        private int _leftRearToFront2;
        private int _leftRearToFront3;

        private int _rightRearToFront1;
        private int _rightRearToFront2;
        private int _rightRearToFront3;

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            _front = pManager.AddColourParameter("Front", "front", "Knit on front bed colour", GH_ParamAccess.item);
            _rear = pManager.AddColourParameter("Rear", "rear", "Knit on rear bed colour", GH_ParamAccess.item);
            _both = pManager.AddColourParameter("Both", "both", "Knit on both beds", GH_ParamAccess.item);

            _tuckOnFront = pManager.AddColourParameter("TuckOnFront", "TucKF", "Tuck on front bed", GH_ParamAccess.item);
            _tuckOnRear = pManager.AddColourParameter("TuckOnRear", "TucKR", "Tuck on rear bed", GH_ParamAccess.item);
            _knitFrontTuckRear = pManager.AddColourParameter("KnitFrontTuckRear", "KFTR", "Knit on front, tuck rear bed", GH_ParamAccess.item);
            _tuckFrontKnitRear = pManager.AddColourParameter("TuckFrontKnitRear", "TFKR", "Tuck on front, knit rear bed", GH_ParamAccess.item);

            _slip = pManager.AddColourParameter("Slip", "slip", "Slip", GH_ParamAccess.item);

            _splitOnFront = pManager.AddColourParameter("SplitOnFront", "SFront", "Split on front bed", GH_ParamAccess.item);
            _splitOnRear = pManager.AddColourParameter("SplitOnRear", "SRear", "Split on rear bed", GH_ParamAccess.item);

            _transferToRear = pManager.AddColourParameter("Transfer to rear", "TtoR", "Transfer front to rear", GH_ParamAccess.item);
            _transferToFront = pManager.AddColourParameter("Transfer to front", "TtoF", "Transfer rear to front", GH_ParamAccess.item);
            _miss = pManager.AddColourParameter("Miss", "Miss", "No stitch", GH_ParamAccess.item);

            _leftFrontToRear1 = pManager.AddColourParameter("Left to rear 1", "LtoR1", "Left Xfer front to rear by 1 stitch", GH_ParamAccess.item);
            _leftFrontToRear2 = pManager.AddColourParameter("Left to rear 2", "LtoR2", "Left Xfer front to rear by 2 stitches", GH_ParamAccess.item);
            _leftFrontToRear3 = pManager.AddColourParameter("Left to rear 3", "LtoR3", "Left Xfer front to rear by 3 stitches", GH_ParamAccess.item);

            _rightFrontToRear1 = pManager.AddColourParameter("Right to rear 1", "RtoR1", "Right Xfer front to rear by 1 stitch", GH_ParamAccess.item);
            _rightFrontToRear2 = pManager.AddColourParameter("Right to rear 2", "RtoR2", "Right Xfer front to rear by 2 stitches", GH_ParamAccess.item);
            _rightFrontToRear3 = pManager.AddColourParameter("Right to rear 3", "RtoR3", "Right Xfer front to rear by 3 stitches", GH_ParamAccess.item);

            _leftRearToFront1 = pManager.AddColourParameter("Left to front 1", "LtoF1", "Left Xfer rear to front by 1 stitch", GH_ParamAccess.item);
            _leftRearToFront2 = pManager.AddColourParameter("Left to front 2", "LtoF2", "Left Xfer rear to front by 2 stitches", GH_ParamAccess.item);
            _leftRearToFront3 = pManager.AddColourParameter("Left to front 3", "LtoF3", "Left Xfer rear to front by 3 stitches", GH_ParamAccess.item);

            _rightRearToFront1 = pManager.AddColourParameter("Right to front 1", "RtoF1", "Right Xfer rear to front by 1 stitch", GH_ParamAccess.item);
            _rightRearToFront2 = pManager.AddColourParameter("Right to front 2", "RtoF2", "Right Xfer rear to front by 2 stitches", GH_ParamAccess.item);
            _rightRearToFront3 = pManager.AddColourParameter("Right to front 3", "RtoF3", "Right Xfer rear to front by 3 stitches", GH_ParamAccess.item);


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
            pManager.AddGenericParameter("Palette", "P", "Stitch colour palette", GH_ParamAccess.item);
            pManager.AddTextParameter("AsList", "L", "List of colours and commands", GH_ParamAccess.list);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            KStitchPalette palette = new KStitchPalette();

            // Basic moviments
            GH_Colour front = new GH_Colour();
            if (DA.GetData(_front, ref front)) palette.Front = front;

            GH_Colour rear = new GH_Colour();
            if (DA.GetData(_rear, ref rear)) palette.Rear = rear;

            GH_Colour both = new GH_Colour();
            if (DA.GetData(_both, ref both)) palette.Both = both;

            GH_Colour tuckOnFront = new GH_Colour();
            if (DA.GetData(_tuckOnFront, ref tuckOnFront)) palette.TuckOnFront = tuckOnFront;

            GH_Colour tuckOnRear = new GH_Colour();
            if (DA.GetData(_tuckOnRear, ref tuckOnRear)) palette.TuckOnRear = tuckOnRear;

            GH_Colour knitFrontTuckRear = new GH_Colour();
            if (DA.GetData(_knitFrontTuckRear, ref knitFrontTuckRear)) palette.KnitFrontTuckRear = knitFrontTuckRear;

            GH_Colour tuckFrontKnitRear = new GH_Colour();
            if (DA.GetData(_tuckFrontKnitRear, ref tuckFrontKnitRear)) palette.TuckFrontKnitRear = tuckFrontKnitRear;

            GH_Colour slip = new GH_Colour();
            if (DA.GetData(_slip, ref slip)) palette.Slip = slip;

            GH_Colour splitOnFront = new GH_Colour();
            if (DA.GetData(_splitOnFront, ref splitOnFront)) palette.SplitOnFront = splitOnFront;

            GH_Colour splitOnRear = new GH_Colour();
            if (DA.GetData(_splitOnRear, ref splitOnRear)) palette.SplitOnRear = splitOnRear;

            GH_Colour tToR = new GH_Colour();
            if (DA.GetData(_transferToRear, ref tToR)) palette.TransferToRear = tToR;

            GH_Colour tToF = new GH_Colour();
            if (DA.GetData(_transferToFront, ref tToF)) palette.TransferToFront = tToF;

            GH_Colour miss = new GH_Colour();
            if (DA.GetData(_miss, ref miss)) palette.Miss = miss;

            // Cross transfers - left front to rear

            GH_Colour lToR1 = new GH_Colour();
            if (DA.GetData(_leftFrontToRear1, ref lToR1)) palette.LeftFrontToRear1 = lToR1;

            GH_Colour lToR2 = new GH_Colour();
            if (DA.GetData(_leftFrontToRear2, ref lToR2)) palette.LeftFrontToRear2 = lToR2;

            GH_Colour lToR3 = new GH_Colour();
            if (DA.GetData(_leftFrontToRear3, ref lToR3)) palette.LeftFrontToRear3 = lToR3;

            // Cross transfers - Right front to rear

            GH_Colour rToR1 = new GH_Colour();
            if (DA.GetData(_rightFrontToRear1, ref rToR1)) palette.RightFrontToRear1 = rToR1;

            GH_Colour rToR2 = new GH_Colour();
            if (DA.GetData(_rightFrontToRear2, ref rToR2)) palette.RightFrontToRear2 = rToR2;

            GH_Colour rToR3 = new GH_Colour();
            if (DA.GetData(_rightFrontToRear3, ref rToR3)) palette.RightFrontToRear3 = rToR3;

            // Cross transfers - Left rear to front

            GH_Colour lToF1 = new GH_Colour();
            if (DA.GetData(_leftRearToFront1, ref lToF1)) palette.LeftRearToFront1 = lToF1;

            GH_Colour lToF2 = new GH_Colour();
            if (DA.GetData(_leftRearToFront2, ref lToF2)) palette.LeftRearToFront2 = lToF2;

            GH_Colour lToF3 = new GH_Colour();
            if (DA.GetData(_leftRearToFront3, ref lToF3)) palette.LeftRearToFront3 = lToF3;

            // Cross transfers - Right rear to front 

            GH_Colour rToF1 = new GH_Colour();
            if (DA.GetData(_rightRearToFront1, ref rToF1)) palette.RightRearToFront1 = rToF1;

            GH_Colour rToF2 = new GH_Colour();
            if (DA.GetData(_rightRearToFront2, ref rToF2)) palette.RightRearToFront2 = rToF2;

            GH_Colour rToF3 = new GH_Colour();
            if (DA.GetData(_rightRearToFront3, ref rToF3)) palette.RightRearToFront3 = rToF3;

            List<string> list = new List<string>()
            {
                "front: " + palette.Front.ToString(),
                "rear: " + palette.Rear.ToString(),
                "both: " + palette.Both.ToString(),
                "tuck on front: " + palette.TuckOnFront.ToString(),
                "tuck on rear:" + palette.TuckOnRear.ToString(),
                "knit front, tuck rear: " + palette.KnitFrontTuckRear.ToString(),
                "tuck front, knit rear: " + palette.TuckFrontKnitRear.ToString(),
                "slip: " + palette.Slip.ToString(),
                "split on front: " + palette.SplitOnFront.ToString(),
                "split on rear: " + palette.SplitOnRear.ToString(),
                "transfer to rear: " + palette.TransferToRear.ToString(),
                "transfer to front: " + palette.TransferToFront.ToString(),
                "miss: " + palette.Miss.ToString(),
                "Left front to rear by 1: " + palette.LeftFrontToRear1.ToString(),
                "Left front to rear by 2: " + palette.LeftFrontToRear2.ToString(),
                "Left front to rear by 3: " + palette.LeftFrontToRear3.ToString(),
                "Right front to rear by 1: " + palette.RightFrontToRear1.ToString(),
                "Right front to rear by 2: " + palette.RightFrontToRear2.ToString(),
                "Right front to rear by 3: " + palette.RightFrontToRear3.ToString(),
                "Left rear to front by 1: " + palette.LeftRearToFront1.ToString(),
                "Left rear to front by 2: " + palette.LeftRearToFront2.ToString(),
                "Left rear to front by 3: " + palette.LeftRearToFront3.ToString(),
                "Right rear to front by 1: " + palette.RightRearToFront1.ToString(),
                "Right rear to front by 2: " + palette.RightRearToFront2.ToString(),
                "Right rear to front by 3: " + palette.RightRearToFront3.ToString()
            };

            if (!palette.IsValid())
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "All stitch colours must be unique");
                return;
            }

            DA.SetData(0, palette);
            DA.SetDataList(1, list);
        }

        /// <summary>
        /// Provides an Icon for the component.
        /// </summary>
        protected override System.Drawing.Bitmap Icon => Properties.Resources.SPalette;

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("233C1BFC-9405-401A-9D09-F32791F63EDE"); }
        }
    }
}