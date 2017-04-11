using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace World3D
{

    public class TestGame : WorldWindow
    {
        float time = 0;

        Cube cube = new Cube()
        {
            Scale = new Vector3(100, 100, 100),
            Position = new Vector3(0, 0, 100)
        };

        Cube cCube = new ColouredCube()
        {
            Scale = new Vector3(100, 100, 100),
            Position = new Vector3(-100, 0, 0)
        };
        Cube tCube = new TexturedCube()
        {
            Scale = new Vector3(100, 100, 100),
            Position = new Vector3(100, 0, 0)
        };
        public TestGame()
        {
            ShaderModelRenderer s = new ShaderModelRenderer(cube)
            {
                FragmentShaderFilename = "Shaders/fs.glsl",
                VertexShaderFilaneme = "Shaders/vs.glsl"
            };
            ShaderModelRenderer sCol = new ShaderModelRenderer(cCube)
            {
                FragmentShaderFilename = "Shaders/fs.glsl",
                VertexShaderFilaneme = "Shaders/vs_col.glsl"
            };
            ShaderModelRenderer sTex = new ShaderModelRenderer(tCube)
            {
                FragmentShaderFilename = "Shaders/fs_tex.glsl",
                VertexShaderFilaneme = "Shaders/vs_tex.glsl"
            };
            int texID = TextureLoader.LoadImage("Textures/opentksquare.png");
            (tCube as ITexturedModel).TextureID = texID;
            
            this.Models.Add(sTex);
            this.Models.Add(sCol);
            this.Models.Add(s);
        }
        protected override void OnLoad(EventArgs e)
        {
            List<ShaderModelRenderer> models = Models;
            base.OnLoad(e);


            camControl.SetPosition(new Vector3(-400, 400, -400), new Vector2((float)-Math.PI / 4, (float)-Math.PI / 4));

        }
        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            time += (float)e.Time;

            cCube.Position = new Vector3(-100, 50*(float)Math.Sin(time), 0);
            cCube.Rotation = new Vector3(0.3f * time, 0.1f * time, 0);


            tCube.Position = new Vector3(100, -50 * (float)Math.Sin(time), 0);
            tCube.Rotation = new Vector3(0.3f * time, 0.1f * time, 0);

            cube.Position = new Vector3(0, -50 * (float)Math.Cos(time), 100);
            cube.Rotation = new Vector3(0.3f * time, 0.1f * time, 0);

            base.OnUpdateFrame(e);
        }
    }

    class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {

            //if (args.Length > 0)
            //    Aar60Translate.Srtm3PathManager.Folder = args[0];
            // The 'using' idiom guarantees proper resource cleanup.
            // We request 30 UpdateFrame events per second, and unlimited
            // RenderFrame events (as fast as the computer can handle).
            using (WorldWindow game = new TestGame())
            {
                game.Run(30.0);
            }
        }
    }
}
