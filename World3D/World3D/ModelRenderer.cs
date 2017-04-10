using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;

namespace World3D
{
    public interface IModelRenderer
    {
        IModel Model { get; }
        void Load(EventArgs e);
        void UpdateFrame(FrameEventArgs e);
        void RenderFrame(FrameEventArgs e);
    }
    public class ShaderModelRenderer : IModelRenderer
    {
        public IModel Model { get; protected set; }
        public ShaderProgram ShaderProgram { get; protected set; }

        public string VertexShaderFilaneme { get; set; } = "Shaders/vs.glsl";
        public string FragmentShaderFilename { get; set; } = "Shaders/fs.glsl";

        protected int indexBufferID;
        protected int vertexBufferID;
        


        List<IModelRenderer> featureRenderers = new List<IModelRenderer>();

        public ShaderModelRenderer(IModel model, ShaderProgram existingProgram)
        {
            this.Model = model;
            this.ShaderProgram = existingProgram;

            if (model is IColourModel)
                featureRenderers.Add(new ColourRenderer(model as IColourModel, ShaderProgram));

            if (model is ITexturedModel)
                featureRenderers.Add(new TextureRenderer(model as ITexturedModel, ShaderProgram));
            
        }

        public ShaderModelRenderer(IModel model) : this(model, new ShaderProgram())
        {

        }
        

        public virtual void Load(EventArgs e)
        {
            GL.GenBuffers(1, out indexBufferID);
            ShaderProgram.Load(VertexShaderFilaneme, FragmentShaderFilename, true);


            foreach(var r in featureRenderers)
            {
                r.Load(e);
            }
            

            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            

        }


        public virtual void UpdateFrame(FrameEventArgs e)
        {
            GL.UseProgram(ShaderProgram.ProgramID);
            foreach (var r in featureRenderers)
            {
                r.UpdateFrame(e);
            }

            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

        }
        

        public virtual void RenderFrame(FrameEventArgs e)
        {
            GL.UseProgram(ShaderProgram.ProgramID);


            GL.BindBuffer(BufferTarget.ArrayBuffer, ShaderProgram.GetBuffer("vPosition"));
            GL.BufferData(BufferTarget.ArrayBuffer,
                (IntPtr)(Model.Vertices.Length * Vector3.SizeInBytes),
                Model.Vertices, BufferUsageHint.StaticDraw);

            GL.VertexAttribPointer(ShaderProgram.GetAttribute("vPosition"), 3, VertexAttribPointerType.Float, false, 0, 0);


            ShaderProgram.EnableVertexAttribArrays();
            Matrix4 mvp = Model.WorldViewProjectionMatrix;
            GL.UniformMatrix4(ShaderProgram.GetUniform("modelview"), false, ref mvp);


            GL.BindBuffer(BufferTarget.ElementArrayBuffer, indexBufferID);
            GL.BufferData(BufferTarget.ElementArrayBuffer,
                (IntPtr)(Model.Indices.Length * sizeof(int)),
                Model.Indices, BufferUsageHint.StaticDraw);
            foreach (var r in featureRenderers)
            {
                r.RenderFrame(e);
            }

            GL.DrawElements(Model.DrawMode, Model.Indices.Length, DrawElementsType.UnsignedInt, 0);

        }


        internal class ColourRenderer : IModelRenderer
        {
            IColourModel model;
            protected ShaderProgram shaderProgram;
            public IModel Model { get { return model; } }
            public ColourRenderer(IColourModel model, ShaderProgram shaderProgram)
            {
                this.model = model;
                this.shaderProgram = shaderProgram;
            }

            public void Load(EventArgs e)
            {
            }

            public void UpdateFrame(FrameEventArgs e)
            {

            }

            public void RenderFrame(FrameEventArgs e)
            {
                // Buffer vertex color if shader supports it
                if (shaderProgram.GetAttribute("vColor") != -1 && model.ColourData.Length > 0)
                {
                    GL.BindBuffer(BufferTarget.ArrayBuffer, shaderProgram.GetBuffer("vColor"));
                    GL.BufferData<Vector3>(BufferTarget.ArrayBuffer,
                        (IntPtr)(model.ColourData.Length * Vector3.SizeInBytes),
                        model.ColourData, BufferUsageHint.StaticDraw);
                    GL.VertexAttribPointer(shaderProgram.GetAttribute("vColor"), 3, VertexAttribPointerType.Float, true, 0, 0);
                }
            }
        }

        internal class TextureRenderer : IModelRenderer
        {
            ITexturedModel model;
            protected ShaderProgram shaderProgram;
            public IModel Model { get { return model; } }
            public TextureRenderer(ITexturedModel model, ShaderProgram shaderProgram)
            {
                this.model = model;
                this.shaderProgram = shaderProgram;
            }

            public void Load(EventArgs e)
            {
            }

            public void UpdateFrame(FrameEventArgs e)
            {

            }

            public void RenderFrame(FrameEventArgs e)
            {
                // Buffer texture coordinates if shader supports it
                if (shaderProgram.GetAttribute("vTexcoord") != -1 && model.TextureCoords.Length > 0)
                {
                    GL.BindBuffer(BufferTarget.ArrayBuffer, shaderProgram.GetBuffer("vTexcoord"));
                    GL.BufferData<Vector2>(BufferTarget.ArrayBuffer,
                        (IntPtr)(model.TextureCoords.Length * Vector2.SizeInBytes),
                        model.TextureCoords, BufferUsageHint.StaticDraw);
                    GL.VertexAttribPointer(shaderProgram.GetAttribute("vTexcoord"), 2, VertexAttribPointerType.Float, true, 0, 0);
                }
                GL.BindTexture(TextureTarget.Texture2D, model.TextureID);
                if (shaderProgram.GetAttribute("maintexture") != -1)
                {
                    GL.Uniform1(shaderProgram.GetAttribute("maintexture"), model.TextureID);
                }

            }
        }
    }

}
