using Grasshopper;
using Grasshopper.Kernel;
using System;
using System.Drawing;

namespace Kniteretta
{
    public class KniterettaInfo : GH_AssemblyInfo
    {
        public override string Name => "Kniteretta";

        //Return a 24x24 pixel bitmap to represent this GHA library.
        public override Bitmap Icon => null;

        //Return a short string describing the purpose of this GHA library.
        public override string Description => "";

        public override Guid Id => new Guid("66c6ecdb-d337-4d80-b47a-4bd9d7c37c04");

        //Return a string identifying you or your company.
        public override string AuthorName => "";

        //Return a string representing your preferred contact details.
        public override string AuthorContact => "";

        public override string Version => "0.1.0";
    }
}