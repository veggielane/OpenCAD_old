using System;
using System.Collections.Generic;
using Veg.Maths;

namespace OpenCAD.Core.Modeling.BREP.Topology
{

    public interface IFace
    {
        string Name { get; }
        IEnumerable<IEdgeLoop> Loops { get; }
        void Add(IEdgeLoop loop);
    }

    public interface IHalfEdge
    {
        string Name { get; }

        Vect3 Start { get; }
        Vect3 End { get; }
        Lazy<IHalfEdge> Opposite { get; }
        
        IFace Face { get; }
        IHalfEdge Next { get; set; }

        Func<double, Vect3> Equation { get; }
        void Connect(IHalfEdge next, IFace face);
    }

    public class Vertex : Vect3
    {
        public string Name { get; private set; }
        public Vertex(string name, double x, double y, double z)
            : base(x, y, z)
        {
            Name = name;
        }

        public Vertex(string name, Vect3 v)
            : base(v)
        {
            Name = name;
        }

        public Vertex(string name):base(0,0,0)
        {
            Name = name;
        }
    }


    public class Face : IFace
    {
        public string Name { get; private set; }
        public IEnumerable<IEdgeLoop> Loops { get { return _loops; } }

        private readonly List<IEdgeLoop> _loops = new List<IEdgeLoop>();

        public Face(string name)
        {
            Name = name;
        }

        public void Add(IEdgeLoop loop)
        {
            _loops.Add(loop);
        }

    }

    public interface IEdgeLoop
    {
        IHalfEdge Start { get; }
        IEnumerable<IHalfEdge> Edges { get; }
    }

    public class EdgeLoop:IEdgeLoop
    {
        public IHalfEdge Start { get; private set; }
        public IEnumerable<IHalfEdge> Edges
        {
            get
            {
                var e = Start;
                do
                {
                    yield return e;
                    e = e.Next;
                } while (e != Start);
            }
        }


        public EdgeLoop(IFace face, params IHalfEdge[] edges)
        {
            Start = edges[0];
            for (var i = 0; i < edges.Length - 1; i++)
            {
                edges[i].Connect(edges[i + 1],face);
            }
            edges[edges.Length - 1].Connect(Start,face);
        }
    }
  

    public abstract class BaseHalfEdge : IHalfEdge
    {
        public string Name { get; private set; }

        public Vect3 Start { get { return Opposite.Value.End; } }
        public Vect3 End { get; protected set; }

        public  Lazy<IHalfEdge> Opposite { get; private set; }

        public IFace Face { get; private set; }
        public IHalfEdge Next { get; set; }

        public Func<double, Vect3> Equation { get; protected set; }

        protected BaseHalfEdge(string name, Vect3 end, Lazy<IHalfEdge> opposite)
        {
            Name = name;
            End = end;
            Opposite = opposite;
        }

        public void Connect(IHalfEdge next, IFace face)
        {
            Next = next;
            Face = face;
        }

    }

    public class LineHalfEdge : BaseHalfEdge
    {
        public LineHalfEdge(string name, Vect3 end, Lazy<IHalfEdge> opposite)
            : base(name, end, opposite)
        {
            Equation = t => Vect3.Lerp(Start, End, t);
        }
    }


    public class HalfEdgePair : IHalfEdgePair
    {
        public IHalfEdge Edge1 { get; private set; }
        public IHalfEdge Edge2 { get; private set; }

        
    }



    public interface IHalfEdgePair
    {
        IHalfEdge Edge1 { get; }
        IHalfEdge Edge2 { get; }
    }



    //public class LineEdge:IEdge<LineHalfEdge>
    //{
    //    public LineHalfEdge Edge1 { get; private set; }
    //    public LineHalfEdge Edge2 { get; private set; }

    //    public LineEdge(string name, Vect3 start, Vect3 end,IFace face1,IFace face2)
    //    {
    //        Edge1 = new LineHalfEdge(name + "-1", end, Edge2, face1);
    //        Edge2 = new LineHalfEdge(name + "-2", start, Edge1, face2);
    //    }
    //    //public static Tuple<Line> 
    //}

    public interface IShell
    {
        IList<IFace> Faces { get; }
    }

    public class Shell : IShell
    {
        public IList<IFace> Faces { get; private set; }
        public Shell(params IFace[] edges)
        {
            Faces = edges;
        }
    }


    public interface ITopology
    {

        IEnumerable<IFace> Faces { get; }
        IEnumerable<Vertex> Vertices { get; }
        IEnumerable<IHalfEdge> Edges { get; }

    }





}