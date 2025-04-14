using Grasshopper.Kernel.Data;
using Grasshopper.Kernel.Types;
using System;
using System.Drawing;
using System.IO;

namespace Kniteretta
{
    public static class Util
    {
        public static string ColorToString(int[] col)
        {
            return string.Format("{0}-{1}-{2}", col[0], col[1], col[2]);
        }

        /// <summary>
        /// Load an image into a <see cref="GH_Colour"/> bi-dimensional array
        /// </summary>
        /// <param name="filename">the path of the file to be loaded</param>
        /// <returns></returns>
        /// <exception cref="Exception">If the file does not exist</exception>
        public static GH_Colour[,] LoadImage(string filename)
        {
            Bitmap bitmap;

            if (File.Exists(filename))
            {
                bitmap = new Bitmap(filename);
            }
            else
            {
                throw new Exception("File does not exist!");
            }

            GH_Colour[,] colourArray = new GH_Colour[bitmap.Width, bitmap.Height];
            for (int i = 0; i < bitmap.Width; i++)
            {
                for (int j = 0; j < bitmap.Height; j++)
                {
                    colourArray[i,j] = new GH_Colour(bitmap.GetPixel(i, j));
                }
            }

            return colourArray;
        }

        public static Bitmap GetImage(GH_Colour[,] arr)
        {
            int width = arr.GetLength(0);
            int height = arr.GetLength(1);
            Bitmap bitmap = new Bitmap(width, height);
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    bitmap.SetPixel(x, y, arr[x, y].Value);
                }
            }

            return bitmap;
        }

        public static GH_Colour[,] TreeTo2D(GH_Structure<GH_Colour> tree)
        {
            GH_Colour[,] result = new GH_Colour[tree.PathCount, tree.Branches[0].Count];
            for (int i = 0; i < tree.PathCount; i++)
            {
                var branch = tree.Branches[i];
                for (int j = 0;j < branch.Count;  j++)
                {
                    result[i,j] = branch[j];
                }
            }
            return result;
        }

    }
}
