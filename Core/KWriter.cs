using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kniteretta
{
    public class KWriter
    {
        private Dictionary<string, string> _parameters;
        public string Output { private set; get; }

        public KWriter()
        {
            _parameters = KTemplate.GetDictForTemplate();
            Output = "";
        }

        public List<string> GetUnfilledParams()
        {
            List<string> unfilled = new List<string>();

            foreach (string key in _parameters.Keys)
            {
                var item = _parameters[key];
                if (item == null || item == "") unfilled.Add(item);
            }

            return unfilled;
        }

        public string Generate()
        {
            List<string> unfilled = GetUnfilledParams();
            var unfilledRequired = new HashSet<string>(unfilled.Intersect(GetRequiredParams()));
            if (unfilledRequired.Count > 0)
            {
                throw new Exception(
                  string.Format(
                    "The knit piece has not been fully defined, missing {0} out of {1} parameters.\nMissing Parameters:\n{2}",
                    unfilled.Count,
                    KTemplate.GetDictForTemplate().Count,
                    unfilled)
                  );
            }
            else
            {
                StringBuilder output = new StringBuilder(KTemplate.GetTemplate());
                foreach (var parameter in _parameters)
                {
                    output.Replace("{" + parameter.Key + "}", parameter.Value);
                }
                Output = output.ToString();
                return output.ToString();
            }
        }

        public void SetCustomParameters(KParameters kParams)
        {
            SetParameter("shape_filename", kParams.ShapeFilename);
            SetParameter("piece_description", kParams.PieceDescription);
            SetParameter("main_stitches", kParams.MainStitches);
            SetParameter("main_rows", kParams.MainRows);
            SetParameter("rib_stitches", kParams.RibStitches);
            SetParameter("rib_rows", kParams.RibRows);
            SetParameter("rib_stitch_density", kParams.RibStitchDensity);
            SetParameter("rib_row_density", kParams.RibRowDensity);
            SetParameter("stitch_density", kParams.StitchDensity);
            SetParameter("row_density", kParams.RowDensity);

            SetParameter("knit_racking", kParams.KnitRacking);
            SetParameter("knit_speed", kParams.KnitSpeed);
            SetParameter("knit_roller", kParams.KnitRoller);
            SetParameter("knit_front_size", kParams.KnitFrontSize);
            SetParameter("knit_rear_size", kParams.KnitRearSize);
            SetParameter("knit_direction", kParams.KnitDirection);
            
            SetParameter("transfer_racking", kParams.TransferRacking);
            SetParameter("transfer_speed", kParams.TransferSpeed);
            SetParameter("transfer_roller", kParams.TransferRoller);
            SetParameter("transfer_front_size", kParams.TransferFrontSize);
            SetParameter("transfer_rear_size", kParams.TransferRearSize);
            SetParameter("transfer_direction", kParams.TransferDirection);
        }

        public string GetParameter(string key)
        {
            if (_parameters.ContainsKey(key))
            {
                return _parameters[key];
            }
            else return null;
        }


        public void SetParameter(string key, string val) { _parameters[key] = val; }
        public void DelParameter(string key) { _parameters.Remove(key); }

        private string[] GetRequiredParams()
        {
            return new string[] { "yarn", "yarn_palette", "stitch_symbols" };
        }

        public static string CreateYarnForPalette(int num, int red, int green, int blue, string colour)
        {
            return string.Format("YARN {0} : {1},{2},{3} {4}", num, red, green, blue, colour);
        }
    }
}
