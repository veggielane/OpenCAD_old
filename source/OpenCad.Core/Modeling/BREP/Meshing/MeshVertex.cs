using Veg.Maths;

namespace OpenCAD.Core.Modeling.BREP.Meshing
{
    public class MeshVertex : Vect3
    {
        public HalfEdge Edge { get; set; }
        public MeshVertex(Vect3 position)
            : base(position)
        {

        }
    }
}