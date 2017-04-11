using System;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Collections.Generic;
using System.Collections;

namespace World3D
{
    /// <summary>
    /// An object made up of vertices
    /// </summary>
    public abstract class Model : IModel
    {
        
        public Matrix4 WorldMatrix { get; set; } = Matrix4.Identity;
        public Matrix4 ViewProjectionMatrix { get; set; } = Matrix4.Identity;
        public Matrix4 WorldViewProjectionMatrix { get; set; } = Matrix4.Identity;

        public abstract Vector3[] Vertices { get; protected set; }
        public abstract int[] Indices { get; protected set; }
        //public virtual Vector3[] ColourData { get { return new Vector3[] { }; }  protected set { } }

        public BeginMode DrawMode { get; protected set; } = BeginMode.Triangles;

        public IEnumerator<Triangle> GetEnumerator()
        {
            return new TriangleEnumerator(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    public abstract class MovableModel : Model, IMovableModel
    {

        public Vector3 Position { get; set; } = Vector3.Zero;
        public Vector3 Rotation { get; set; } = Vector3.Zero;
        public Vector3 Scale { get; set; } = Vector3.One;
        public virtual void CalculateModelMatrix()
        {
            WorldMatrix = Matrix4.CreateScale(Scale)
                * Matrix4.CreateRotationX(Rotation.X) * Matrix4.CreateRotationY(Rotation.Y) * Matrix4.CreateRotationZ(Rotation.Z)
                * Matrix4.CreateTranslation(Position);
        }
    }

    public class Cube : MovableModel
    {
        Vector3[] vertices;
        int[] indices;


        public override Vector3[] Vertices { get { return vertices; }protected set { vertices = value; } }

        public override int[] Indices { get { return indices; } protected set { indices = value; } }
        

        public Cube()
        {
            BeginMode mode;
            CreateCube(out this.vertices, out this.indices, out mode);
            this.DrawMode = mode;
        }


        public static void CreateCube(out Vector3[] vertices, out int[] indices, out BeginMode drawMode)
        {
            CreateCube(new Vector3(-0.5f, -0.5f, -0.5f), new Vector3(0.5f, 0.5f, 0.5f), out vertices, out indices, out drawMode);
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

        public static void CreateTriangleCube(out Vector3[] vertices, out int[] indices, out BeginMode drawMode)
        {
            CreateTriangleCube(new Vector3(-0.5f, -0.5f, -0.5f), new Vector3(0.5f, 0.5f, 0.5f), out vertices, out indices, out drawMode);
        }
        public static void CreateTriangleCube(Vector3 vMin, Vector3 vMax, out Vector3[] vertices, out int[] indices, out BeginMode drawMode)
        {
            drawMode = BeginMode.Triangles;
            vertices = new Vector3[] {

                //left
                new Vector3(vMin.X, vMin.Y,  vMin.Z),
                new Vector3(vMax.X, vMax.Y,  vMin.Z),
                new Vector3(vMax.X, vMin.Y,  vMin.Z),
                new Vector3(vMin.X, vMax.Y,  vMin.Z),

                //back
                new Vector3(vMax.X, vMin.Y,  vMin.Z),
                new Vector3(vMax.X, vMax.Y,  vMin.Z),
                new Vector3(vMax.X, vMax.Y,  vMax.Z),
                new Vector3(vMax.X, vMin.Y,  vMax.Z),

                //right
                new Vector3(vMin.X, vMin.Y,  vMax.Z),
                new Vector3(vMax.X, vMin.Y,  vMax.Z),
                new Vector3(vMax.X, vMax.Y,  vMax.Z),
                new Vector3(vMin.X, vMax.Y,  vMax.Z),

                //top
                new Vector3(vMax.X, vMax.Y,  vMin.Z),
                new Vector3(vMin.X, vMax.Y,  vMin.Z),
                new Vector3(vMax.X, vMax.Y,  vMax.Z),
                new Vector3(vMin.X, vMax.Y,  vMax.Z),

                //front
                new Vector3(vMin.X, vMin.Y,  vMin.Z),
                new Vector3(vMin.X, vMax.Y,  vMax.Z),
                new Vector3(vMin.X, vMax.Y,  vMin.Z),
                new Vector3(vMin.X, vMin.Y,  vMax.Z),

                //bottom
                new Vector3(vMin.X, vMin.Y,  vMin.Z),
                new Vector3(vMax.X, vMin.Y,  vMin.Z),
                new Vector3(vMax.X, vMin.Y,  vMax.Z),
                new Vector3(vMin.X, vMin.Y,  vMax.Z)

            };
            indices = new int[] {
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
        }

    }

    public class ColouredCube : Cube, IColourModel
    {
       protected Vector3[] colourData;

        public ColouredCube()
        {
            Vector3[] vertices;
            int[] indicies;
            BeginMode drawMode;
            Cube.CreateTriangleCube(out vertices, out indicies, out drawMode);
            this.Vertices = vertices;
            this.Indices = indicies;
            this.DrawMode = drawMode;
            
            Vector3 red = new Vector3(1, 0, 0);
            Vector3 green = new Vector3(0, 1, 0);
            Vector3 blue = new Vector3(0, 0, 1);
            Vector3 mag = new Vector3(1, 0, 1);
            Vector3 cyan = new Vector3(0, 1, 1);
            Vector3 yell = new Vector3(1, 1, 0);

            colourData = new Vector3[] {
                red,red,red,red,
                green,green,green,green,
                blue,blue,blue,blue,
                mag,mag,mag,mag,
                cyan,cyan,cyan,cyan,
                yell,yell,yell,yell
            };
        }
        public Vector3[] ColourData
        {
            get { return colourData; }
            protected set { colourData = value; }
        }
    }
    public class TexturedCube : Cube, ITexturedModel
    {
        public int TextureID { get; set; }
        protected Vector2[] texCoords;

        public Vector2[] TextureCoords { get { return texCoords; } }
        public TexturedCube()
        {
            Vector3[] vertices;
            int[] indicies;
            BeginMode drawMode;
            Cube.CreateTriangleCube(out vertices, out indicies, out drawMode);
            this.Vertices = vertices;
            this.Indices = indicies;
            this.DrawMode = drawMode;

            texCoords = new Vector2[] {
                // left
                new Vector2(0.0f, 0.0f),
                new Vector2(-1.0f, 1.0f),
                new Vector2(-1.0f, 0.0f),
                new Vector2(0.0f, 1.0f),
 
                // back
                new Vector2(0.0f, 0.0f),
                new Vector2(0.0f, 1.0f),
                new Vector2(-1.0f, 1.0f),
                new Vector2(-1.0f, 0.0f),
 
                // right
                new Vector2(-1.0f, 0.0f),
                new Vector2(0.0f, 0.0f),
                new Vector2(0.0f, 1.0f),
                new Vector2(-1.0f, 1.0f),
 
                // top
                new Vector2(0.0f, 0.0f),
                new Vector2(0.0f, 1.0f),
                new Vector2(-1.0f, 0.0f),
                new Vector2(-1.0f, 1.0f),
 
                // front
                new Vector2(0.0f, 0.0f),
                new Vector2(1.0f, 1.0f),
                new Vector2(0.0f, 1.0f),
                new Vector2(1.0f, 0.0f),
 
                // bottom
                new Vector2(0.0f, 0.0f),
                new Vector2(0.0f, 1.0f),
                new Vector2(-1.0f, 1.0f),
                new Vector2(-1.0f, 0.0f)
            };
        }
    }
}
