using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace World3D
{
    public class UserInterfaceModel : MovableModel, ITexturedModel
    {
        protected int width, height;
        Bitmap bmp;
        public Bitmap Bitmap { get { return bmp; } }
        Vector3[] vertices;
        int[] indices;

        public override Vector3[] Vertices { get { return vertices; } protected set { vertices = value; } }

        public override int[] Indices { get { return indices; } protected set { indices = value; } }

        public int TextureID { get; set; } = -1;
        protected Vector2[] texCoords;

        public Vector2[] TextureCoords { get { return texCoords; } }

        public UserInterfaceModel() : this(800, 600) { }
        public UserInterfaceModel(int width, int height)
        {
            this.width = width;
            this.height = height;
            BeginMode mode;
            Square.CreateSquare(new Vector2(-1,-1),new Vector2(1,1), out this.vertices, out this.indices, out mode);
            this.DrawMode = mode;
            texCoords = new Vector2[]
             {
                new Vector2(0,1),
                new Vector2(1,1),
                new Vector2(0,0),
                new Vector2(1,0)
             };
            for (int i = 0; i < vertices.Length; i++)
                vertices[i] = new Vector3(vertices[i].X, -vertices[i].Z, -1.0f);
            bmp = new Bitmap(width, height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
        }

        public override void UpdateFrame()
        {
            if(TextureID == -1)
                TextureID = TextureLoader.LoadImage(Bitmap);
            else
                TextureLoader.UpdateTexture(Bitmap, TextureID);
            WorldMatrix = Matrix4.Identity;
        }
        

        public override Matrix4 ViewProjectionMatrix { get { return WorldMatrix = Matrix4.Identity; } set { } }
        public override Matrix4 WorldViewProjectionMatrix { get { return WorldMatrix = Matrix4.Identity; } set { } }
    }

    public class UserInterfaceSimpleTextbox : UserInterfaceModel
    {
        /// <summary>
        /// Position of textbox as a proportion of bitmap size.
        /// Eg, (0,0,0.5,0.25) will start in top-left, extend 50% width, 25% height.
        /// </summary>
        public RectangleF BoxPosition { get; set; } = new RectangleF(0, 0, 0.3f, 0.2f);
        public PointF TextPosition { get; set; } = new PointF(0.02f, 0.02f);
        public Color BoxBackground { get; set; } = Color.FromArgb(120, 255, 200, 0);
        public string Text { get; set; }
        public int FontSize { get; set; } = 12;
        public override void UpdateFrame()
        {
            using (Graphics g = Graphics.FromImage(Bitmap))
            {
                g.Clear(Color.FromArgb(0, 0, 0, 0));
                g.FillRectangle(new SolidBrush(BoxBackground), 
                    BoxPosition.Left*width, BoxPosition.Top*height, BoxPosition.Width*width, BoxPosition.Height*height);
                g.DrawString(Text, new Font("Arial", FontSize), Brushes.Black, TextPosition.X*width, TextPosition.Y*height);

            }
            base.UpdateFrame();
        }
    }
}
