using OpenTK;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using World3D;

namespace World3DWindowTests
{
    public class UserInterfaceTest
    {

        public static void TextboxInterface()
        {
            using (WorldWindow game = new WorldWindow())
            {
                Vector3 eye = new Vector3(0, 2, 0);
                game.Camera.LookAt(eye, eye * 0.8f);
                UserInterface ui = new UserInterface(game);
                
                game.UpdateFrame += (o, e) =>
                {
                    ui.PrintCommonStats(e.Time);
                };

                game.Run(30);

            }
        }

        
        public static void TextboxInterfaceWithObject()
        {
            using (WorldWindow game = new WorldWindow())
            {
                Vector3 eye = new Vector3(0, 0, -4);
                game.Camera.LookAt(eye, eye * 0.8f);
                UserInterfaceSimpleTextbox ui = new UserInterfaceSimpleTextbox();
                ui.BoxBackground = Color.FromArgb(40, 0, 0, 0);
                ShaderModelRenderer ui_sm = new ShaderModelRenderer(ui)
                {
                    VertexShaderFilaneme = "Shaders/vs_tex.glsl",
                    FragmentShaderFilename = "Shaders/fs_tex.glsl",
                };

                ColouredCube cube = new ColouredCube();
                ShaderModelRenderer sm2 = new ShaderModelRenderer(cube)
                {
                    VertexShaderFilaneme = "Shaders/vs_col.glsl",
                    FragmentShaderFilename = "Shaders/fs.glsl",
                };
                game.Models.Add(sm2);
                game.Models.Insert(0, ui_sm);
                float time = 0;
                game.UpdateFrame += (o, e) =>
                {
                    time += (float)e.Time;
                    double fps = e.Time == 0 ? 1000 : 1.0 / e.Time;

                    ui.Text = "Fps: " + fps.ToString("N1") + " Time: " + time.ToString("N2") + "\nnewline" + "\nnewline1" + "\nnewline2" + "\nnewline3";

                    cube.Position = new Vector3(-1, 0.5f * (float)Math.Sin(time), 0);
                    cube.Rotation = new Vector3(0.3f * time, 0.1f * time, 0);
                };

                game.Run(30);

            }
        }

    }
}
