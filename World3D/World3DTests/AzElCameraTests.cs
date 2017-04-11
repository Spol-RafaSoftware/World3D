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
            Vector3 eye = new Vector3(0,-10,0);
            cam.LookAt(eye, eye*0.8f);
            double v = cam.Elevation;
            double t = Math.PI / 2;
            Assert.IsTrue(v > t - 0.11f, "Looking up elevation: " + v + " != " + t);
        }

        [TestMethod()]
        public void LookDownTest()
        {
            AzElCamera cam = new AzElCamera();
            Vector3 eye = new Vector3(0, 10, 0);
            cam.LookAt(eye, eye * 0.8f);
            double v = cam.Elevation;
            double t = -Math.PI / 2;
            Assert.IsTrue(v < t + 0.11f, "Looking down elevation: " + v + " != " + t);
        }

        [TestMethod()]
        public void LookNorthTest()
        {
            AzElCamera cam = new AzElCamera();
            Vector3 eye = new Vector3(0, 0, -10);
            cam.LookAt(eye, eye * 0.8f);
            double v = cam.Azimuth;
            double t = 0;
            Assert.IsTrue(v < t+0.01 && v > t-0.01, "Looking north azimuth: " + v + " != " + t);
        }

        [TestMethod()]
        public void LookSouthTest()
        {
            AzElCamera cam = new AzElCamera();
            Vector3 eye = new Vector3(0, 0, 10);
            cam.LookAt(eye, eye * 0.8f);
            double v = cam.Azimuth;
            double t = Math.PI;
            Assert.IsTrue((v < -t + 0.01 && v > -t - 0.01) || (v < t + 0.01 && v > t - 0.01), "Looking south azimuth: " + v + " != " + t);
        }

        [TestMethod()]
        public void LookEastTest()
        {
            AzElCamera cam = new AzElCamera();
            Vector3 eye = new Vector3(-10, 0, 0);
            cam.LookAt(eye, eye * 0.8f);
            double v = cam.Azimuth;
            double t = Math.PI / 2;
            Assert.IsTrue(v < t + 0.01 && v > t - 0.01, "Looking east azimuth: " + v + " != " + t);
        }
    }
}