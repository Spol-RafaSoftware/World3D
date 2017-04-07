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
        Cube cube = new ColouredCube() { Scale = new Vector3(100, 100, 100) };
        public TestGame()
        {
            this.Models.Add(new ModelRenderer(cube));
        }
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            camControl.SetPosition(new Vector3(-400, 400, -400), new Vector2((float)-Math.PI / 4, (float)-Math.PI / 4));

        }
        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            time += (float)e.Time;

            cube.Position = new Vector3(0.3f, 50*(float)Math.Sin(time), -3.0f);
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
