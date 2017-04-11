using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using World3D;

namespace World3DWindowTests
{
    public class ShadedObjectTests
    {
        public static void ThreeShaders()
        {
            Cube cube1 = new Cube();
            Cube cube2 = new ColouredCube();
            Cube cube3 = new TexturedCube();
            ShaderModelRenderer sm1 = new ShaderModelRenderer(cube1);
            ShaderModelRenderer sm2 = new ShaderModelRenderer(cube2)
            {
                VertexShaderFilaneme = "Shaders/vs_col.glsl"
            };
            ShaderModelRenderer sm3 = new ShaderModelRenderer(cube3)
            {
                VertexShaderFilaneme = "Shaders/vs_tex.glsl",
                FragmentShaderFilename = "Shaders/fs_tex.glsl"
            };

            using (WorldWindow game = new WorldWindow() { Title = "ThreeShaders" })
            {
                game.Camera.LookAt(new Vector3(-4, 4, -4), new Vector3());

                (cube3 as TexturedCube).TextureID = TextureLoader.loadImage("Textures/opentksquare.png");

                game.Models.Add(sm1);
                game.Models.Add(sm2);
                game.Models.Add(sm3);
                float time = 0;
                game.UpdateFrame += (o, e) =>
                            {
                                time += (float)e.Time;
                                cube1.Position = new Vector3(-1, 0.5f * (float)Math.Sin(time), 0);
                                cube1.Rotation = new Vector3(0.3f * time, 0.1f * time, 0);

                                cube2.Position = new Vector3(1, -0.5f * (float)Math.Sin(time), 0);
                                cube2.Rotation = new Vector3(0.3f * time, 0.1f * time, 0);

                                cube3.Position = new Vector3(0, -0.5f * (float)Math.Cos(time), 1);
                                cube3.Rotation = new Vector3(0.3f * time, 0.1f * time, 0);
                            };
                game.Run();
            }
        }
    }
}
