using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using OpenTK.Graphics.OpenGL;

namespace World3D
{
    public class TriangleEnumerator : IEnumerator<Triangle>
    {
        Vector3[] verts;
        int[] inds;
        BeginMode drawMode;
        int pos = -1;

        public Triangle Current
        {
            get
            {
                Vector4 v0 = new Vector4(verts[inds[pos - 2]], 1);
                Vector4 v1 = new Vector4(verts[inds[pos - 1]], 1);
                Vector4 v2 = new Vector4(verts[inds[pos]], 1);
                if(drawMode == BeginMode.Triangles || (pos & 1) == 1)
                    return new Triangle(v1.Xyz, v0.Xyz, v2.Xyz);
                else
                    return new Triangle(v0.Xyz, v1.Xyz, v2.Xyz);
            }
        }

        object IEnumerator.Current
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public TriangleEnumerator(IModel model)
        {
            verts = model.Vertices;
            inds = model.Indices;
            drawMode = model.DrawMode;
            if (!(drawMode == BeginMode.TriangleStrip || drawMode == BeginMode.Triangles))
                throw new NotImplementedException("Iterator only implemented for Triangles or TriangleStrip draw modes.");
            Reset();
        }

        public void Dispose() { }

        public bool MoveNext()
        {
            if (drawMode == BeginMode.Triangles) pos += 3;
            else pos += 1;
            
            return pos < inds.Length;
        }

        public void Reset()
        {
            if (drawMode == BeginMode.Triangles) pos = -1;
            else pos = 1;
        }
    }

}
