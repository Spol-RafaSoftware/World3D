using Microsoft.VisualStudio.TestTools.UnitTesting;
using World3D;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace World3D.Tests
{
    [TestClass()]
    public class EarthConverterTests
    {
        [TestMethod()]
        public void AltToECEFAt0Test()
        {
            Vector3 p1 = new Vector3();
            Vector3 p2 = new Vector3(0, 0, 1000);
            Vector3 e1 = EarthConverter.LatLongAltToECEF(p1);
            Vector3 e2 = EarthConverter.LatLongAltToECEF(p2);
            Vector3 d = e2 - e1;
            VectorAssertions.AreEqual(new Vector3(1000, 0, 0), d, 0.01);
        }
        [TestMethod()]
        public void AltToECEFAt90Test()
        {
            Vector3 p1 = new Vector3(0, 90, 0);
            Vector3 p2 = new Vector3(0, 90, 1000);
            Vector3 e1 = EarthConverter.LatLongAltToECEF(p1);
            Vector3 e2 = EarthConverter.LatLongAltToECEF(p2);
            Vector3 d = e2 - e1;
            VectorAssertions.AreEqual(new Vector3(0, 1000, 0), d, 0.01);
        }
        [TestMethod()]
        public void LatToECEFAt0Test()
        {
            Vector3 p1 = new Vector3(0, 0, 0);
            Vector3 p2 = new Vector3(1, 0, 0);
            Vector3 e1 = EarthConverter.LatLongAltToECEF(p1);
            Vector3 e2 = EarthConverter.LatLongAltToECEF(p2);
            Vector3 d = e2 - e1;
            VectorAssertions.AreEqual(new Vector3(0, 0, 111000), d, 1000);
        }

        [TestMethod()]
        public void LatToECEFAtNZTest()
        {
            Vector3 p1 = new Vector3(-37, 174, 0);
            Vector3 p2 = new Vector3(-36, 173, 0);
            Vector3 e1 = EarthConverter.LatLongAltToECEF(p1);
            Vector3 e2 = EarthConverter.LatLongAltToECEF(p2);
            Vector3 d = e2 - e1;
            VectorAssertions.AreEqual(new Vector3(-55518, 96492, 89143), d, 100);
        }

        [TestMethod()]
        public void RotateECEFUpAt0Test()
        {
            Vector3 p1 = new Vector3(0, 0, 0);
            Vector3 p2 = new Vector3(1, 0, 0);
            Vector3 e1 = EarthConverter.LatLongAltToECEF(p1);
            Vector3 e2 = EarthConverter.LatLongAltToECEF(p2);
            Vector3 de = e2 - e1;
            Vector3 dr = EarthConverter.RotateECEF(de, 0, 0);
            VectorAssertions.AreEqual(new Vector3(110000, 0, 0), dr, 1400);
        }

        [TestMethod()]
        public void RotateECEFUpAt90Test()
        {
            Vector3 p1 = new Vector3(0, 90, 0);
            Vector3 p2 = new Vector3(1, 90, 0);
            Vector3 e1 = EarthConverter.LatLongAltToECEF(p1);
            Vector3 e2 = EarthConverter.LatLongAltToECEF(p2);
            Vector3 de = e2 - e1;
            Vector3 dr = EarthConverter.RotateECEF(de, 0, 90);
            VectorAssertions.AreEqual(new Vector3(110000, 0, 0), dr, 1400);
        }


        [TestMethod()]
        public void RotateECEFRightAt0Test()
        {
            Vector3 p1 = new Vector3(0, 0, 0);
            Vector3 p2 = new Vector3(0, 1, 0);
            Vector3 e1 = EarthConverter.LatLongAltToECEF(p1);
            Vector3 e2 = EarthConverter.LatLongAltToECEF(p2);
            Vector3 de = e2 - e1;
            Vector3 dr = EarthConverter.RotateECEF(de, 0, 0);
            VectorAssertions.AreEqual(new Vector3(0, 0, 110000), dr, 1400);
        }

        [TestMethod()]
        public void RotateECEFRightAt90Test()
        {
            Vector3 p1 = new Vector3(0, 90, 0);
            Vector3 p2 = new Vector3(0, 91, 0);
            Vector3 e1 = EarthConverter.LatLongAltToECEF(p1);
            Vector3 e2 = EarthConverter.LatLongAltToECEF(p2);
            Vector3 de = e2 - e1;
            Vector3 dr = EarthConverter.RotateECEF(de, 0, 90);
            VectorAssertions.AreEqual(new Vector3(0, 0, 110000), dr, 1400);
        }
        [TestMethod()]
        public void RotateECEFUpAtNZTest()
        {
            Vector3 p1 = new Vector3(-37, 174, 0);
            Vector3 p2 = new Vector3(-36, 173, 100);
            Vector3 e1 = EarthConverter.LatLongAltToECEF(p1);
            Vector3 e2 = EarthConverter.LatLongAltToECEF(p2);
            Vector3 de = e2 - e1;
            Vector3 dr = EarthConverter.RotateECEF(de, p1.X, p1.Y);
            VectorAssertions.AreEqual(new Vector3(110490, -1496, -90160), dr, 100);
        }
    }
}