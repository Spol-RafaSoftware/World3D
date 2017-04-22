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
                    SouthWestLatLong = new Vector2(-37, 174),
                    NorthEastLatLong = new Vector2(-36.9f, 174.1f),
                    DegreesLatitudePerPixel = 0.2 / (double)rows,
                    DegreesLongitudePerPixel = 0.2 / (double)cols
                };
                float[,] altInMetres = info.CreateFlatAltitudes(rows, cols);
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

                Vector2 SouthWestLatLong = new Vector2(-37, 174);
                Vector2 NorthEastLatLong = new Vector2(-36.9f, 174.2f);
                Terrain terrain = new Terrain();
                
                TiledAltitudeMapReader mapReader = new CosineAltitudeMapReader();
                Vector3[,] lla = mapReader.GetMap(SouthWestLatLong, NorthEastLatLong);
             

                terrain.Recreate(lla);

                ShaderModelRenderer sm = new ShaderModelRenderer(terrain) { VertexShaderFilaneme = "Shaders/vs_norm.glsl" };

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



        public static void AucklandTerrainGridTest()
        {
            using (WorldWindow game = new WorldWindow())
            {
                Vector3 eye = new Vector3(-1000, 4000, -1000);
                game.Camera.LookAt(eye, new Vector3());
                game.CameraControl.MoveSpeed = 1000;
                game.SetZRange(100, 100000);

                Vector2 SouthWestLatLong = new Vector2(-37, 174);
                Vector2 NorthEastLatLong = new Vector2(-36, 175);
                Terrain terrain = new Terrain();

                TiledAltitudeMapReader mapReader = new Srtm3Reader() { AltitudeScale = 10 };
                Vector3[,] lla = mapReader.GetMap(SouthWestLatLong, NorthEastLatLong);


                Vector2 auckland = new Vector2(-36.8468f, 174.7908f);
                terrain.Recreate(lla, auckland);

                ShaderModelRenderer sm = new ShaderModelRenderer(terrain) { VertexShaderFilaneme = "Shaders/vs_norm.glsl" };

                game.Models.Add(sm);


                UserInterface ui = new UserInterface(game);

                game.RenderFrame += (o, e) => ui.PrintCommonStats(e.Time);
                

                game.Run(30);

            }
        }
    }
}
