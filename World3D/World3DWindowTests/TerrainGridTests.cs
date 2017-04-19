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
    public class TerrainGridTests
    {
        public static void SquareGridTest()
        {
            using (WorldWindow game = new WorldWindow())
            {
                Vector3 eye = new Vector3(-4, 4, -4);
                game.Camera.LookAt(eye, new Vector3());


                SquareGrid grid = new SquareGrid(4, 4, 2.3f, 1.1f);
                ShaderModelRenderer sm = new ShaderModelRenderer(grid);

                game.Models.Add(sm);


                UserInterface ui = new UserInterface(game);

                float time = 0;
                game.UpdateFrame += (o, e) =>
                {
                    time += (float)e.Time;
                    double fps = e.Time == 0 ? 1000 : 1.0 / e.Time;
                    ui.PrintCommonStats(e.Time);
                    
                };

                game.Run(30);

            }
        }
    }
}
