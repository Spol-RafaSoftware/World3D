using System;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace World3D
{
    /// <summary>
    /// An object made up of vertices
    /// </summary>
    public abstract class Model
    {
        public Vector3 Position { get; set; } = Vector3.Zero;
        public Vector3 Rotation { get; set; } = Vector3.Zero;
        public Vector3 Scale { get; set; } = Vector3.One;
        
        public Matrix4 ModelMatrix { get; set; } = Matrix4.Identity;
        public Matrix4 ViewProjectionMatrix { get; set; } = Matrix4.Identity;
        public Matrix4 ModelViewProjectionMatrix { get; set; } = Matrix4.Identity;

        public abstract Vector3[] Vertices { get; protected set; }
        public abstract int[] Indices { get; protected set; }
        public virtual Vector3[] ColorData { get { return new Vector3[] { }; }  protected set { } }

        public BeginMode DrawMode { get; protected set; } = BeginMode.Triangles;
      
        public virtual void CalculateModelMatrix()
        {
            ModelMatrix = Matrix4.CreateScale(Scale)
                * Matrix4.CreateRotationX(Rotation.X) * Matrix4.CreateRotationY(Rotation.Y) * Matrix4.CreateRotationZ(Rotation.Z)
                * Matrix4.CreateTranslation(Position);
        }

        public bool IsTextured { get; set; } = false;
        public int TextureID { get; set; }

        public virtual Vector2[] TextureCoords { get { return new Vector2[] { }; } protected set { } }
    }

    public class Cube : Model
    {
        Vector3[] vertices;
        int[] indices;


        public override Vector3[] Vertices { get { return vertices; }protected set { vertices = value; } }

        public override int[] Indices { get { return indices; } protected set { indices = value; } }

       

        public Cube()
        {
            BeginMode mode;
            CreateCube(new Vector3(-0.5f, -0.5f, -0.5f), new Vector3(0.5f,0.5f,0.5f), out this.vertices, out this.indices, out mode);
            this.DrawMode = mode;
        }


        public static void CreateCube(Vector3 vMin, Vector3 vMax, out Vector3[] vertices, out int[] indices, out BeginMode drawMode)
        {
            vertices = new Vector3[8];
            vertices[0] = new Vector3(vMin.X, vMin.Y, vMax.Z);
            vertices[1] = new Vector3(vMax.X, vMin.Y, vMax.Z);
            vertices[2] = new Vector3(vMin.X, vMax.Y, vMax.Z);
            vertices[3] = new Vector3(vMax.X, vMax.Y, vMax.Z);
            vertices[4] = new Vector3(vMin.X, vMin.Y, vMin.Z);
            vertices[5] = new Vector3(vMax.X, vMin.Y, vMin.Z);
            vertices[6] = new Vector3(vMin.X, vMax.Y, vMin.Z);
            vertices[7] = new Vector3(vMax.X, vMax.Y, vMin.Z);
            drawMode = BeginMode.TriangleStrip;
            indices = new int[] { 0, 1, 2, 3, 7, 1, 5, 4, 7, 6, 2, 4, 0, 1 };
        }

    }

    public class ColouredCube : Cube
    {
       protected Vector3[] colorData;

        public ColouredCube()
        {
            DrawMode = BeginMode.Triangles;

            Vertices = new Vector3[] {
                //left
                new Vector3(-0.5f, -0.5f,  -0.5f),
                new Vector3(0.5f, 0.5f,  -0.5f),
                new Vector3(0.5f, -0.5f,  -0.5f),
                new Vector3(-0.5f, 0.5f,  -0.5f),

                //back
                new Vector3(0.5f, -0.5f,  -0.5f),
                new Vector3(0.5f, 0.5f,  -0.5f),
                new Vector3(0.5f, 0.5f,  0.5f),
                new Vector3(0.5f, -0.5f,  0.5f),

                //right
                new Vector3(-0.5f, -0.5f,  0.5f),
                new Vector3(0.5f, -0.5f,  0.5f),
                new Vector3(0.5f, 0.5f,  0.5f),
                new Vector3(-0.5f, 0.5f,  0.5f),

                //top
                new Vector3(0.5f, 0.5f,  -0.5f),
                new Vector3(-0.5f, 0.5f,  -0.5f),
                new Vector3(0.5f, 0.5f,  0.5f),
                new Vector3(-0.5f, 0.5f,  0.5f),

                //front
                new Vector3(-0.5f, -0.5f,  -0.5f),
                new Vector3(-0.5f, 0.5f,  0.5f),
                new Vector3(-0.5f, 0.5f,  -0.5f),
                new Vector3(-0.5f, -0.5f,  0.5f),

                //bottom
                new Vector3(-0.5f, -0.5f,  -0.5f),
                new Vector3(0.5f, -0.5f,  -0.5f),
                new Vector3(0.5f, -0.5f,  0.5f),
                new Vector3(-0.5f, -0.5f,  0.5f)

            };
            Indices = new int[] {
                //left
                0,1,2,0,3,1,

                //back
                4,5,6,4,6,7,

                //right
                8,9,10,8,10,11,

                //top
                13,14,12,13,15,14,

                //front
                16,17,18,16,19,17,

                //bottom 
                20,21,22,20,22,23
            };
            Vector3 red = new Vector3(1, 0, 0);
            Vector3 green = new Vector3(0, 1, 0);
            Vector3 blue = new Vector3(0, 0, 1);
            Vector3 mag = new Vector3(1, 0, 1);
            Vector3 cyan = new Vector3(0, 1, 1);
            Vector3 yell = new Vector3(1, 1, 0);

            colorData = new Vector3[] {
                red,red,red,red,
                green,green,green,green,
                blue,blue,blue,blue,
                mag,mag,mag,mag,
                cyan,cyan,cyan,cyan,
                yell,yell,yell,yell
            };
        }
        public override Vector3[] ColorData
        {
            get { return colorData; }
            protected set { colorData = value; }
        }
    }
    public class TexturedCube : Cube
    {

    }
}
