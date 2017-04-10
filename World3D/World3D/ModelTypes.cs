using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace World3D
{
    /// <summary>
    /// A Model that contains vertex position (Vector3) information.
    /// Basically every model should implement this.
    /// </summary>
    public interface IModel
    {
        Vector3[] Vertices { get; }
        int[] Indices { get;  }
        Matrix4 WorldMatrix { get; set; }
        Matrix4 ViewProjectionMatrix { get; set; }
        Matrix4 WorldViewProjectionMatrix { get; set; } 
        BeginMode DrawMode { get;  }

    }
    /// <summary>
    /// A Model that can have independently changed world Position, Rotation and Scale
    /// </summary>
    public interface IMovableModel
    {
        Vector3 Position { get; set; } 
        Vector3 Rotation { get; set; }
        Vector3 Scale { get; set; }
        void CalculateModelMatrix();
    }

    /// <summary>
    /// A Model that contains per-vertex colour (Vector3) information.
    /// </summary>
    public interface IColourModel : IModel
    {
        Vector3[] ColourData { get; }
    }
    /// <summary>
    /// A Model that contains per-vertex texture-coordinates (Vector2) information.
    /// </summary>
    public interface ITexturedModel : IModel
    {
        int TextureID { get; set; }
        Vector2[] TextureCoords { get; }
    }
}
