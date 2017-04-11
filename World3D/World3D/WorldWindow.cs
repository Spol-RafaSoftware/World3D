// Released to the public domain. Use, modify and relicense at will.

using System;

using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using System.Collections.Generic;
using System.Linq;

namespace World3D
{
    public interface GameWorld : IDisposable
    {
        Camera Camera { get; }
        int Width { get; }
        int Height { get; }
        MouseDevice Mouse { get; }
        Matrix4 Projection { get; }
    }

    public class WorldWindow : GameWindow, GameWorld
    {

        protected AzElCamera camera;
        protected AzElCameraControl camControl;
        volatile bool ShowMesh;
        
        public Matrix4 Projection { get; private set; }
        public Camera Camera { get { return camera; } }

        public List<ShaderModelRenderer> Models { get; set; } = new List<ShaderModelRenderer>();

        /// <summary>Creates a 800x600 window with the specified title.</summary>
        public WorldWindow()
            : base(800, 600, GraphicsMode.Default, "World3D")
        {
            VSync = VSyncMode.On;
            camera = new AzElCamera();
            camControl = new AzElCameraControl(this, camera) ;
        }

        


        /// <summary>Load resources here.</summary>
        /// <param name="e">Not used.</param>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            
            foreach(ShaderModelRenderer m in Models)
            {
                m.Load(e);
            }
            
        }

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            if (e.Button == MouseButton.Left)
                return;

            var size = new System.Drawing.Size(Width, Height);
            Vector3 point = new Vector3(e.X, e.Y, 1.0f);
            Vector4 rayFar = Math3D.UnProject(Projection, Camera.View, size, point);
            point.Z = 0.0f;
            Vector4 rayNear = Math3D.UnProject(Projection, Camera.View, size, point);

            Vector4 rayDir = Vector4.Normalize(rayFar - rayNear);
            Vector2 azEl = Math3D.Cart2Sph(rayDir.Xyz).Xy;
        }
        

        /// <summary>
        /// Called when your window is resized. Set your viewport here. It is also
        /// a good place to set up your projection matrix (which probably changes
        /// along when the aspect ratio of your window).
        /// </summary>
        /// <param name="e">Not used.</param>
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            GL.Viewport(ClientRectangle.X, ClientRectangle.Y, ClientRectangle.Width, ClientRectangle.Height);

            Projection = Matrix4.CreatePerspectiveFieldOfView((float)Math.PI / 4, Width / (float)Height, 1.0f, 100000.0f);
            //Matrix4 p = Projection;
            //GL.MatrixMode(MatrixMode.Projection);
            //GL.LoadMatrix(ref p);
        }

        /// <summary>
        /// Called when it is time to setup the next frame. Add your game logic here.
        /// </summary>
        /// <param name="e">Contains timing information for framerate independent logic.</param>
        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);

            foreach (ShaderModelRenderer mr in Models)
            {
                IModel m = mr.Model;
                if(m is IMovableModel)
                    (m as IMovableModel).UpdateFrame();
                m.ViewProjectionMatrix = Camera.View * Projection;
                m.WorldViewProjectionMatrix = m.WorldMatrix * m.ViewProjectionMatrix;
                mr.UpdateFrame(e);
            }

            camControl.Update(e.Time);
        }


        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            char c = e.KeyChar;

            if (c == ' ')
                ShowMesh = !ShowMesh;
            if (Keyboard[Key.Escape])
                Exit();
        }

        /// <summary>
        /// Called when it is time to render the next frame. Add your rendering code here.
        /// </summary>
        /// <param name="e">Contains timing information.</param>
        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);
            GL.ClearColor(0.6f, 0.7f, 1.0f, 0.0f);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
            GL.Enable(EnableCap.Blend);
            GL.Enable(EnableCap.DepthTest);

            if (ShowMesh)
            {
                GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line);
            }
            else
            {
                GL.PolygonMode(MaterialFace.Front, PolygonMode.Fill);
                GL.PolygonMode(MaterialFace.Back, PolygonMode.Fill);
            }

            List<ShaderModelRenderer> sortedRenderers = Models.OrderBy(s => s.ShaderProgram.ProgramID).ToList();

            foreach (ShaderModelRenderer mr in sortedRenderers)
            {
                mr.RenderFrame(e);
            }
            GL.Flush();
            SwapBuffers();
        }
        

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            System.Windows.Forms.Application.Exit();
        }

        bool disposed = false;
        protected override void Dispose(bool manual)
        {
            if (!disposed)
            {
             
            }
            base.Dispose(manual);

        }

    }

}