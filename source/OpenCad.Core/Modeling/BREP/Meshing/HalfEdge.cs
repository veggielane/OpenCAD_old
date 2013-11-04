namespace OpenCAD.Core.Modeling.BREP.Meshing
{
    //clockwise half edge
    public class HalfEdge
    {
        public MeshVertex ToVertex { get; set; }
        public HalfEdge Opposite { get; set; }
        public HalfEdgeFace HalfEdgeFace { get; set; }
        public HalfEdge Next { get; set; }
        public MeshVertex FromVertex { get { return Opposite.ToVertex; } }

        public HalfEdge()
        {
            
        }
        public override string ToString()
        {
            return "HalfEdge<From:{0},To{1}>".Fmt(ToVertex,FromVertex);
        }
    }
}
