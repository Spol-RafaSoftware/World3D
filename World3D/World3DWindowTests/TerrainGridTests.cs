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

        public static void FlatTerrainGridTest()
        {
            using (WorldWindow game = new WorldWindow())
            {
                Vector3 eye = new Vector3(-4000, 4000, -4000);
                game.Camera.LookAt(eye, new Vector3());
                game.CameraControl.MoveSpeed = 1000;

                Terrain terrain = new Terrain();
                int rows = 80;
                int cols = 100;
                TerrainInfo info = new TerrainInfo()
                {
                    BottomLeftLatLong = new Vector2(-37, 174),
                    CentreLatLong = new Vector2(-36.9f, 174.1f),
                    DegreesLatitudePerPixel = 0.2 / (double)rows,
                    DegreesLongitudePerPixel = 0.2 / (double)cols
                };
                float[][] altInMetres = info.CreateFlatAltitudes(rows, cols);
                terrain.Recreate(info, altInMetres);
                
                ShaderModelRenderer sm = new ShaderModelRenderer(terrain);

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



        public static void CosineTerrainGridTest()
        {
            using (WorldWindow game = new WorldWindow())
            {
                Vector3 eye = new Vector3(-400, 400, -400);
                game.Camera.LookAt(eye, new Vector3());
                game.CameraControl.MoveSpeed = 100;

                Terrain terrain = new Terrain();
                int rows = 80;
                int cols = 100;
                TerrainInfo info = new TerrainInfo()
                {
                    BottomLeftLatLong = new Vector2(-37, 174),
                    CentreLatLong = new Vector2(-36.99f, 174.01f),
                    DegreesLatitudePerPixel = 0.02 / (double)rows,
                    DegreesLongitudePerPixel = 0.02 / (double)cols
                };
                float[][] altInMetres = info.CreateFlatAltitudes(rows, cols);

                for(int x = 0; x < altInMetres.Length; x++)
                {
                    double dx = (double)x / (double)altInMetres.Length;
                    for (int z = 0; z < altInMetres[0].Length; z++)
                    {
                        double dz = (double)z / (double)altInMetres[0].Length;
                        altInMetres[x][z] = -100*(float)(Math.Cos(4*dz * Math.PI) * Math.Cos(4*dx * Math.PI));
                    }
                }

                terrain.Recreate(info, altInMetres);

                ShaderModelRenderer sm = new ShaderModelRenderer(terrain);

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
