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
            Assert.AreEqual(float.IsNaN(expected.X), float.IsNaN(actual.X), "X.IsNaN: "+message);
            Assert.AreEqual(float.IsNaN(expected.Y), float.IsNaN(actual.Y), "Y.IsNaN: " + message);
            Assert.AreEqual(float.IsNaN(expected.Z), float.IsNaN(actual.Z), "Z.IsNaN: " + message);
            Assert.AreEqual(expected.X, actual.X, delta,"X: "+message);
            Assert.AreEqual(expected.Y, actual.Y, delta, "Y: " + message);
            Assert.AreEqual(expected.Z, actual.Z, delta, "Z: " + message);
        }
    }
}
