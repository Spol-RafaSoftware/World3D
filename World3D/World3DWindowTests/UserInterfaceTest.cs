using System;
using System.Collections.Generic;
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


                game.Run(30);

            }
        }
    }
}
