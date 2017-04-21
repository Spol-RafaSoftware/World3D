using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace World3D
{
    public class SquareGrid : MovableModel
    {
        protected Vector3[] vertices;
        protected int[] indices;

        protected int xColumns;
        protected int zRows;
        protected float xPitch;
        protected float zPitch;

        public override Vector3[] Vertices { get { return vertices; } protected set { vertices = value; } }

        public override int[] Indices { get { return indices; } protected set { indices = value; } }

        public SquareGrid() : this(1, 1, 1, 1) { }
        public SquareGrid(int xColumns, int zRows, float xPitch, float zPitch)
        {
            RecreateGrid(xColumns, zRows, xPitch, zPitch);
        }

        protected void RecreateGrid(int xColumns, int zRows, float xPitch, float zPitch)
        {
            this.xColumns = xColumns;
            this.zRows = zRows;
            this.xPitch = xPitch;
            this.zPitch = zPitch;
            BeginMode drawMode;
            CreateSquareGrid(xColumns, zRows, xPitch, zPitch, out vertices, out indices, out drawMode);
            DrawMode = drawMode;
        }

        public static void CreateSquareGrid(int xColumns, int zRows, float xPitch, float zPitch, out Vector3[] vertices, out int[] indices, out BeginMode drawMode)
        {
            drawMode = BeginMode.TriangleStrip;
            int nX = xColumns + 1;
            int nZ = zRows + 1;
            vertices = new Vector3[nX * nZ];
            indices = new int[4];

            int i = 0;
            for (int z = 0; z < nZ; z++)
            {
                for (int x = 0; x < nX; x++)
                {
                    Vector3 v = new Vector3(x * xPitch, 0, z * zPitch);
                    vertices[i++] = v;
                }
            }

            nZ--;
            List<int> inds = new List<int>();


            for (int row = 0; row < nZ; row++)
            {
                if ((row & 1) == 0)
                { // even rows
                    for (int col = 0; col < nX; col++)
                    {
                        inds.Add(col + row * nX);
                        inds.Add(col + (row + 1) * nX);
                    }
                }
                else
                { // odd rows
                    for (int col = nX - 1; col > 0; col--)
                    {
                        inds.Add(col + (row + 1) * nX);
                        inds.Add(col - 1 + +row * nX);
                    }
                }
            }
            // TODO: Figure out when to logically add this last index.
            if(((nZ & 1) == 0) && nZ > 2)
            {
                inds.Add(nZ * nX);
            }
            indices = inds.ToArray();
        }


    }
}
