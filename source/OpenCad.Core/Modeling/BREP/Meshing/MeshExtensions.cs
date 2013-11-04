using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenCAD.Core.Modeling.BREP.Meshing;
using OpenCAD.Core.Primatives;

namespace OpenCAD.Core.Meshing
{
    public static class MeshExtensions
    {
        public static Mesh ToMesh(this AABB c)
        {
            return new Mesh();
        }
    }
}
