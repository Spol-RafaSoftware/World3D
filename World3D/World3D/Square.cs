using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace World3D
{
    public class Square : MovableModel
    {

        Vector3[] vertices;
        int[] indices;

        public override Vector3[] Vertices { get { return vertices; } protected set { vertices = value; } }

        public override int[] Indices { get { return indices; } protected set { indices = value; } }

        public Square()
        {
            BeginMode mode;
            CreateSquare(out this.vertices, out this.indices, out mode);
            this.DrawMode = mode;
        }

        public static void CreateSquare(out Vector3[] vertices, out int[] indices, out BeginMode drawMode)
        {
            CreateSquare(new Vector2(-0.5f, -0.5f), new Vector2(0.5f,0.5f), out vertices, out indices, out drawMode);
        }
        public static void CreateSquare(Vector2 vMin, Vector2 vMax, out Vector3[] vertices, out int[]indices, out BeginMode mode)
        {
            mode = BeginMode.Triangles;
            vertices = new Vector3[] {
                new Vector3(vMin.X, 0, vMin.Y),
                new Vector3(vMin.X, 0,vMax.Y),
                new Vector3(vMax.X, 0,vMax.Y),
                new Vector3(vMax.X, 0,vMin.Y)
                };
            indices = new int[] { 0, 1, 3, 1, 2, 3 };
        }
    }

    public class TexturedSquare : Square, ITexturedModel
    {
        public int TextureID { get; set; }
        protected Vector2[] texCoords;

        public Vector2[] TextureCoords { get { return texCoords; } }

        public TexturedSquare()
        {
            texCoords = new Vector2[]
            {
                new Vector2(0,1),
                new Vector2(0,0),
                new Vector2(1,0),
                new Vector2(1,1)
            };
        }
    }
}
