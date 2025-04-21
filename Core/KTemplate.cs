using System.Collections.Generic;

namespace Kniteretta
{
    public static class KTemplate
    {
        public static Dictionary<string, string> GetDictForTemplate()
        {
            return new Dictionary<string, string>
            {
                {"shape_filename", null },
                {"piece_description", null },
                {"main_stitches" , null },
                {"main_rows", null },
                {"rib_stitches", null },
                {"rib_rows", null },
                {"rib_stitch_density", null },
                {"rib_row_density", null },
                {"stitch_density", null },
                {"row_density", null },
                {"yarn", null },
                {"yarn_palette", null },
                {"stitch_symbols", null },
                {"knit_racking", null },
                {"knit_speed", null },
                {"knit_roller", null },
                {"knit_front_size", null },
                {"knit_rear_size", null },
                {"knit_direction", null },
                {"transfer_racking", null },
                {"transfer_speed", null },
                {"transfer_roller", null },
                {"transfer_front_size", null },
                {"transfer_rear_size", null },
                {"transfer_direction", null },
            };
        }

        public static string GetTemplate()
        {
            return _template;
        }

        private static string _template = @"FILE FORMAT : DAK
FILE FORMAT VERSION: 0.1
GARMENT PIECE
Shape filename : {shape_filename}
Piece : {piece_description}
Integrated with : 
Stitches : {main_stitches}
Rows : {main_rows}
RIB DIMENSIONS
Stitches : {rib_stitches}
Rows : {rib_rows}
RIB TENSIONS
  Stitches per 10 cm =  {rib_stitch_density}
  Rows per 10 cm     =  {rib_row_density}
MAIN TENSIONS
  Stitches per 10 cm =  {stitch_density}
  Rows per 10 cm     =  {row_density}
YARNS
{yarn}
YARN PALETTE
{yarn_palette}
STITCH SYMBOLS
{stitch_symbols}
STITCH PATTERN NOTES
SHAPE FILE NOTES
KNIT_ROWS_DEFAULTS: {knit_racking} / {knit_speed} / {knit_roller} / {knit_front_size} / {knit_rear_size} / {knit_direction}
TRANSFER_ROWS_DEFAULTS: {transfer_racking} / {transfer_speed} / {transfer_roller} / {transfer_front_size} / {transfer_rear_size} / {transfer_direction}
END
";
    }
}
