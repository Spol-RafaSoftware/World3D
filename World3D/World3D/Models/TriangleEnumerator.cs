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
    public class IndexedTriangleEnumerator : IEnumerator<IndexedTriangle>
    {
        Vector3[] verts;
        int[] inds;
        BeginMode drawMode;
        int pos = -1;

        public IndexedTriangle Current
        {
            get
            {
                int i0 = inds[pos - 2];
                int i1 = inds[pos - 1];
                int i2 = inds[pos];
                Vector4 v0 = new Vector4(verts[i0], 1);
                Vector4 v1 = new Vector4(verts[i1], 1);
                Vector4 v2 = new Vector4(verts[i2], 1);
                if (drawMode == BeginMode.Triangles || (pos & 1) == 1)
                    return new IndexedTriangle() { v0 = v1.Xyz, i0 = i1, v1 = v0.Xyz, i1 = i0, v2 = v2.Xyz, i2 = i2 };
                else
                    return new IndexedTriangle() { v0 = v0.Xyz, i0 = i0, v1 = v1.Xyz, i1 = i1, v2 = v2.Xyz, i2 = i2 };
            }
        }

        object IEnumerator.Current
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public IndexedTriangleEnumerator(IModel model)
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
