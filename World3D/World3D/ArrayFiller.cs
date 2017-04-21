using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using OpenTK;

namespace World3D
{
	public class ArrayFiller
	{


        public static void Fill2DArray<T>(T[,] dest, T[,] src, int yDestOffset, int xDestOffset)
        {
            Fill2DArray(dest, src, yDestOffset, xDestOffset, 0, 0);
        }


        public static void Fill2DArray<T>(T[,] dest, T[,] src, int yDestOffset, int xDestOffset, int ySrcOffset, int xSrcOffset)
        {
            Fill2DArray(dest, src, yDestOffset, xDestOffset, 0, 0, src.GetLength(0), src.GetLength(1));
        }

        public static void Fill2DArray<T>(T[,] dest, T[,] src, int yDestOffset, int xDestOffset, int ySrcOffset, int xSrcOffset, int yCount, int xCount)
        {
            int snx = src.GetLength(1);
            int dnx = dest.GetLength(1);
            T loc00 = dest[0, 0];
            T locYX = dest[dest.GetLength(0) - 1, dest.GetLength(1) - 1];
            for (int h = 0; h < yCount; h++)
            {
                int sourceIndex = (ySrcOffset + h) * snx + xSrcOffset;
                int destIndex = (yDestOffset + h) * dnx + xDestOffset;
                Array.Copy(src, sourceIndex, dest, destIndex, xCount);
            }
            loc00 = dest[0, 0];
            locYX = dest[dest.GetLength(0) - 1, dest.GetLength(1) - 1];
        }
    }
}
