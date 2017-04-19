using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace World3DWindowTests
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            //SimpleWindowTests.EmptyWindow();
            //SimpleWindowTests.SingleObect();
            //SimpleWindowTests.TwoMovingObjects();
            //ShadedObjectTests.ThreeShaders();
            //ShadedObjectTests.ManyCubes();
            //UserInterfaceTest.TextboxInterface();
            //UserInterfaceTest.TextboxInterfaceWithObject();
            TerrainGridTests.SquareGridTest();
        }
    }
}
