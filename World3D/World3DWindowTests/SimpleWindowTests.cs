using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using World3D;

namespace World3DWindowTests
{
    public class SimpleWindowTests
    {
        public static void EmptyWindow()
        {
            using (WorldWindow game = new WorldWindow() { Title = "EmptyWindow" })
                game.Run(30);

        }
        
        public static void SingleObect()
        {
            Cube cube = new Cube();
            ShaderModelRenderer sm = new ShaderModelRenderer(cube);
            using (WorldWindow game = new WorldWindow() { Title = "SingleMovingObject" })
            {
                game.Camera.LookAt(new Vector3(-4, 4, -4), new Vector3());
                game.Models.Add(sm);
                game.Run();
            }
        }

        public static void TwoMovingObjects()
        {
            Cube cube1 = new Cube();
            Cube cube2 = new Cube();
            ShaderModelRenderer sm1 = new ShaderModelRenderer(cube1);
            ShaderModelRenderer sm2 = new ShaderModelRenderer(cube2, sm1.ShaderProgram);
            using (WorldWindow game = new WorldWindow() { Title = "TwoMovingObjects" })
            {
                game.Camera.LookAt(new Vector3(-4, 4, -4), new Vector3());
                game.Models.Add(sm1);
                game.Models.Add(sm2);
                float time = 0;
                game.UpdateFrame += (o, e) =>
                {
                    time += (float)e.Time;
                    cube1.Position = new Vector3(-1, 0.5f * (float)Math.Sin(time), 0);
                    cube1.Rotation = new Vector3(0.3f * time, 0.1f * time, 0);

                    cube2.Position = new Vector3(1, -0.5f * (float)Math.Sin(time), 0);
                    cube2.Rotation = new Vector3(0.3f * time, 0.1f * time, 0);
                };
                game.Run();
            }
        }

    }
}
