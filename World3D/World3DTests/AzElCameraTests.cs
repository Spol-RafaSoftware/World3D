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
    public class AzElCameraTests
    {
        [TestMethod()]
        public void LookUpTest()
        {
            AzElCamera cam = new AzElCamera();
            Vector3 eye = new Vector3(0, -10, 0);
            cam.LookAt(eye, eye * 0.8f);
            double v = cam.Elevation;
            double t = Math.PI / 2;
            Assert.IsTrue(v > t - 0.11f, "Looking up elevation: " + v + " != " + t);
        }

        [TestMethod()]
        public void LookDownTest()
        {
            AzElCamera cam = new AzElCamera();
            Vector3 eye = new Vector3(0, 10, 0);
            cam.LookAt(eye, new Vector3());
            double v = cam.Elevation;
            double t = -Math.PI / 2;
            Assert.IsTrue(v < t + 0.11f, "Looking down elevation: " + v + " != " + t);
        }

        [TestMethod()]
        public void LookNorthTest()
        {
            AzElCamera cam = new AzElCamera();
            Vector3 eye = new Vector3(-1, 0, 0);
            cam.LookAt(eye, new Vector3());
            double v = cam.Azimuth;
            if (v < 0) v = v + 2 * Math.PI;
            double t = 0;
            Assert.AreEqual(t, v, 0.01, "Azimuth wrong");
            v = cam.Elevation;
            t = 0;
            Assert.AreEqual(t, v, 0.01, "Elevation wrong");
            VectorAssertions.AreEqual(new Vector3(0, 0, 0), cam.Target, 0.01);
        }

        [TestMethod()]
        public void LookSouthTest()
        {
            AzElCamera cam = new AzElCamera();
            Vector3 eye = new Vector3(1, 0, 0);
            cam.LookAt(eye, new Vector3());
            double v = cam.Azimuth;
            if (v < 0) v = v + 2 * Math.PI;
            double t = Math.PI;
            Assert.AreEqual(t, v, 0.01, "Azimuth wrong");
            v = cam.Elevation;
            t = 0;
            Assert.AreEqual(t, v, 0.01, "Elevation wrong");
            VectorAssertions.AreEqual(new Vector3(0, 0, 0), cam.Target, 0.01);
        }

        [TestMethod()]
        public void LookEastTest()
        {
            AzElCamera cam = new AzElCamera();
            Vector3 eye = new Vector3(0, 0, -1);
            cam.LookAt(eye, new Vector3());
            double v = cam.Azimuth;
            double t = Math.PI / 2;
            Assert.AreEqual(t, v, 0.01, "Azimuth wrong");
            v = cam.Elevation;
            t = 0;
            Assert.AreEqual(t, v, 0.01, "Elevation wrong");
            VectorAssertions.AreEqual(new Vector3(0, 0, 0), cam.Target, 0.01);
        }
        [TestMethod()]
        public void LookWestTest()
        {
            AzElCamera cam = new AzElCamera();
            Vector3 eye = new Vector3(0, 0, 1);
            cam.LookAt(eye, new Vector3());
            double v = cam.Azimuth;
            double t = -Math.PI / 2;
            Assert.AreEqual(t, v, 0.01, "Azimuth wrong");
            v = cam.Elevation;
            t = 0;
            Assert.AreEqual(t, v, 0.01, "Elevation wrong");
            VectorAssertions.AreEqual(new Vector3(0, 0, 0), cam.Target, 0.01);
        }

        [TestMethod()]
        public void LookDiagonal()
        {
            AzElCamera cam = new AzElCamera();
            float d = (float)(1 / Math.Sqrt(3));
            Vector3 eye = new Vector3(-d, d, -d);
            cam.LookAt(eye, new Vector3());
            double v = cam.Azimuth;
            double t = Math.PI / 4;
            Assert.AreEqual(t, v, 0.01, "Looking diagonally: " + v + " != " + t);
            VectorAssertions.AreEqual(new Vector3(0, 0, 0), cam.Target, 0.01);
        }

        [TestMethod()]
        public void MoveNorth()
        {
            // start facing north, then move forwards.
            AzElCamera cam = new AzElCamera();
            Vector3 eye = new Vector3(-4, 0, 0);
            cam.LookAt(eye, new Vector3());
            Assert.AreEqual(0, cam.Azimuth);
            cam.MoveForwards(4);
            VectorAssertions.AreEqual(new Vector3(0, 0, 0), cam.Eye, 0.01);
            Assert.AreEqual(0, cam.Azimuth, 0.01);
        }

        [TestMethod()]
        public void MoveEast()
        {
            // start facing north, then move east.
            AzElCamera cam = new AzElCamera();
            Vector3 eye = new Vector3(0, 0, -4);
            cam.LookAt(eye, new Vector3(4, 0, -4));
            Assert.AreEqual(0, cam.Azimuth);
            cam.MoveSideways(4);
            VectorAssertions.AreEqual(new Vector3(0, 0, 0), cam.Eye, 0.01);
            Assert.AreEqual(0, cam.Azimuth, 0.01);
        }

        [TestMethod()]
        public void MoveDiagonal()
        {
            // start facing northeast.
            AzElCamera cam = new AzElCamera();
            Vector3 eye = new Vector3(-4, 0, -4);
            cam.LookAt(eye, new Vector3(0, 0, 0));
            Assert.AreEqual(Math.PI/4, cam.Azimuth,0.01);
            // Move forwards
            cam.MoveForwards(Math.Sqrt(32));
            VectorAssertions.AreEqual(new Vector3(0, 0, 0), cam.Eye, 0.01, "Move Forwards failed");
            Assert.AreEqual(Math.PI / 4, cam.Azimuth,0.01);
            // Move sidways, left
            cam.MoveSideways(-Math.Sqrt(32));
            VectorAssertions.AreEqual(new Vector3(4, 0, -4), cam.Eye, 0.01,"Move Sideways failed");
            Assert.AreEqual(Math.PI / 4, cam.Azimuth, 0.01);
        }


        [TestMethod()]
        public void IncreaseAzimuth()
        {
            AzElCamera cam = new AzElCamera();
            Vector3 eye = new Vector3(-1, 0, 0);
            cam.LookAt(eye, new Vector3(0, 0, 0));
            Assert.AreEqual(0, cam.Azimuth, 0.01);

            cam.Azimuth += Math.PI / 2;
            VectorAssertions.AreEqual(new Vector3(-1, 0, 1), cam.Target, 0.01);
            Assert.AreEqual(Math.PI/2, cam.Azimuth, 0.01);
        }

    }
}