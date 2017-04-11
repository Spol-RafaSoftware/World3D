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

                (cube3 as TexturedCube).TextureID = TextureLoader.LoadImage("Textures/opentksquare.png");

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

        public static void ManyCubes()
        {
            using (WorldWindow game = new WorldWindow())
            {
                Vector3 eye = new Vector3(-4, 4, -4);
                game.Camera.LookAt(eye, eye*0.8f);
                int texID = TextureLoader.LoadImage("Textures/opentksquare.png");
                Vector3[] posOffsets = GenerateRandomCubes(game, texID);

                UserInterfaceSimpleTextbox ui = new UserInterfaceSimpleTextbox();
                ShaderModelRenderer ui_sm = new ShaderModelRenderer(ui)
                {
                    VertexShaderFilaneme = "Shaders/vs_tex.glsl",
                    FragmentShaderFilename = "Shaders/fs_tex.glsl",
                };
                game.Models.Add(ui_sm);

                float time = 0;
                game.UpdateFrame += (o, e) =>
                {
                    time += (float)e.Time;
                    ui.Text = CommonStats(game,e.Time);


                    MoveModels(game, time, posOffsets);
                };
                game.Run(60.0);
            }
        }

        static double time = 0;
        public static string CommonStats(WorldWindow game, double timeSinceLastRender)
        {
            time += (float)timeSinceLastRender;
            double fps = timeSinceLastRender == 0 ? 1000 : 1.0 / timeSinceLastRender;
            AzElCamera cam = game.Camera as AzElCamera;
            return "Fps: " + fps.ToString("N1") + " Time: " + time.ToString("N2") + "\n"
                    + "CamLoc:  " + cam.Eye.X.ToString("F2")+","+cam.Eye.Y.ToString("F2") + ","+cam.Eye.Z.ToString("F2") + "\n"
                    + "CamAzEl: " + cam.Azimuth.ToString("F2")+":"+cam.Elevation.ToString("F2");
        }


        static Vector3[] GenerateRandomCubes(WorldWindow game, int texID)
        {
            int numCubes = 100;
            Random rand = new Random();
                
            Vector3[] posOffsets = new Vector3[numCubes];
            for(int i = 0; i < numCubes; i++)
            {
                int type = rand.Next(3);
                Cube cube;
                ShaderModelRenderer sm;
                if (type == 0)
                {
                    cube = new Cube();
                    sm = new ShaderModelRenderer(cube);
                }
                else if(type == 1)
                {
                    cube = new ColouredCube();
                    sm = new ShaderModelRenderer(cube)
                    {
                        VertexShaderFilaneme = "Shaders/vs_col.glsl",
                        FragmentShaderFilename = "Shaders/fs.glsl"
                    };
                }
                else
                {
                    cube = new TexturedCube();
                    sm = new ShaderModelRenderer(cube)
                    {
                        VertexShaderFilaneme = "Shaders/vs_tex.glsl",
                        FragmentShaderFilename = "Shaders/fs_tex.glsl"
                    };
                        (cube as TexturedCube).TextureID = TextureLoader.LoadImage("Textures/opentksquare.png");
                    
                }

                cube.Scale = new Vector3(0.5f, 0.5f, 0.5f);
                game.Models.Add(sm);
                Vector3 v = new Vector3((float)rand.NextDouble(), (float)rand.NextDouble(), (float)rand.NextDouble())*(float)(Math.PI*2);
                posOffsets[i] = v - (Vector3.One * (float)Math.PI);
            }
            return posOffsets;
        }

        static void MoveModels(WorldWindow game, float time, Vector3[] posOffsets)
        {
            for (int i = 0; i < posOffsets.Length; i++)
            {
                MovableModel cube = game.Models[i].Model as MovableModel;
                cube.Position = new Vector3(posOffsets[i].X, (float)Math.Sin(time + posOffsets[i].Y), posOffsets[i].Z);
                cube.Rotation = new Vector3(0.5f * time, 0.1f * time, 0);
            }
        }
    }
}
