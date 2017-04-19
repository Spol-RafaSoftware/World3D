using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace World3D.Tests
{
    public class VectorAssertions
    {
        public static void AreEqual(Vector3 expected, Vector3 actual, double delta)
        {
            AreEqual(expected, actual, delta, "");
        }
        public static void AreEqual(Vector3 expected, Vector3 actual, double delta, string message)
        {
            Assert.AreEqual(expected.X, actual.X, delta,message);
            Assert.AreEqual(expected.Y, actual.Y, delta, message);
            Assert.AreEqual(expected.Z, actual.Z, delta, message);
        }
    }
}
