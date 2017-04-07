using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace World3D
{
    public class ModelRenderer
    {
        public Model Model { get; set; }

        protected int indexBufferID;
        protected int vertexBufferID;

        protected ShaderProgram shaderProgram;

        public ModelRenderer(Model model)
        {
            this.Model = model;
        }

        public virtual void Load(EventArgs e)
        {
            GL.GenBuffers(1, out indexBufferID);
            shaderProgram = new ShaderProgram("Shaders/vs.glsl", "Shaders/fs.glsl", true);

            GL.BindBuffer(BufferTarget.ArrayBuffer, shaderProgram.GetBuffer("vPosition"));
            GL.BufferData<Vector3>(BufferTarget.ArrayBuffer, 
                (IntPtr)(Model.Vertices.Length * Vector3.SizeInBytes), 
                Model.Vertices, BufferUsageHint.StaticDraw);

            GL.VertexAttribPointer(shaderProgram.GetAttribute("vPosition"), 3, VertexAttribPointerType.Float, false, 0, 0);


            // Buffer vertex color if shader supports it
            if (shaderProgram.GetAttribute("vColor") != -1 && Model.ColorData.Length > 0)
            {
                GL.BindBuffer(BufferTarget.ArrayBuffer, shaderProgram.GetBuffer("vColor"));
                GL.BufferData<Vector3>(BufferTarget.ArrayBuffer, 
                    (IntPtr)(Model.ColorData.Length * Vector3.SizeInBytes), 
                    Model.ColorData, BufferUsageHint.StaticDraw);
                GL.VertexAttribPointer(shaderProgram.GetAttribute("vColor"), 3, VertexAttribPointerType.Float, true, 0, 0);
            }


            // Buffer texture coordinates if shader supports it
            if (shaderProgram.GetAttribute("vTexcoord") != -1 && Model.TextureCoords.Length > 0)
            {
                GL.BindBuffer(BufferTarget.ArrayBuffer, shaderProgram.GetBuffer("vTexcoord"));
                GL.BufferData<Vector2>(BufferTarget.ArrayBuffer,
                    (IntPtr)(Model.TextureCoords.Length * Vector2.SizeInBytes), 
                    Model.TextureCoords, BufferUsageHint.StaticDraw);
                GL.VertexAttribPointer(shaderProgram.GetAttribute("vTexcoord"), 2, VertexAttribPointerType.Float, true, 0, 0);
            }

            GL.UseProgram(shaderProgram.ProgramID);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

            GL.BindBuffer(BufferTarget.ElementArrayBuffer, indexBufferID);
            GL.BufferData(BufferTarget.ElementArrayBuffer,
                (IntPtr)(Model.Indices.Length * sizeof(int)),
                Model.Indices, BufferUsageHint.StaticDraw);
        }


        public virtual void UpdateFrame(FrameEventArgs e)
        {

        }



        public virtual void RenderFrame(FrameEventArgs e)
        {
            GL.UseProgram(shaderProgram.ProgramID);
            shaderProgram.EnableVertexAttribArrays();
            GL.BindTexture(TextureTarget.Texture2D, Model.TextureID);
            Matrix4 mvp = Model.ModelViewProjectionMatrix;
            GL.UniformMatrix4(shaderProgram.GetUniform("modelview"), false, ref mvp);

            if (shaderProgram.GetAttribute("maintexture") != -1)
            {
                GL.Uniform1(shaderProgram.GetAttribute("maintexture"), Model.TextureID);
            }

            GL.DrawElements(Model.DrawMode, Model.Indices.Length, DrawElementsType.UnsignedInt, 0);
        }
    }
}
