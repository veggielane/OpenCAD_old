﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using OpenCAD.Core;
using OpenCAD.Core.Maths;
using OpenCAD.Core.Topology;

namespace OpenCAD.Test
{

    [TestFixture]
    public class Test
    {
        [Test]
        public void Testing()
        {
            const double s = 5.0;
            var v0 = new Vertex("v0", new Vect3(0, 0, 0));
            var v1 = new Vertex("v1", new Vect3(0, 0, s));
            var v2 = new Vertex("v2", new Vect3(s, 0, s));
            var v3 = new Vertex("v3", new Vect3(s, 0, 0));
            var v4 = new Vertex("v4", new Vect3(0, s, 0));
            var v5 = new Vertex("v5", new Vect3(0, s, s));
            var v6 = new Vertex("v6", new Vect3(s, s, s));
            var v7 = new Vertex("v7", new Vect3(s, s, 0));


            var f0 = new Face("f0");
            var f1 = new Face("f1");
            var f2 = new Face("f2");
            var f3 = new Face("f3");
            var f4 = new Face("f4");
            var f5 = new Face("f5");
            LineHalfEdge e0a = null;
            LineHalfEdge e0b = null;
            LineHalfEdge e1a = null;
            LineHalfEdge e1b = null;
            LineHalfEdge e2a = null;
            LineHalfEdge e2b = null;
            LineHalfEdge e3a = null;
            LineHalfEdge e3b = null;
            LineHalfEdge e4a = null;
            LineHalfEdge e4b = null;
            LineHalfEdge e5a = null;
            LineHalfEdge e5b = null;
            LineHalfEdge e6a = null;
            LineHalfEdge e6b = null;
            LineHalfEdge e7a = null;
            LineHalfEdge e7b = null;
            LineHalfEdge e8a = null;
            LineHalfEdge e8b = null;
            LineHalfEdge e9a = null;
            LineHalfEdge e9b = null;
            LineHalfEdge e10a = null;
            LineHalfEdge e10b = null;
            LineHalfEdge e11a = null;
            LineHalfEdge e11b = null;

            e0a = new LineHalfEdge("e0a", v1, new Lazy<IHalfEdge>(() => e0b));
            e1a = new LineHalfEdge("e1a", v2, new Lazy<IHalfEdge>(() => e1b));
            e2a = new LineHalfEdge("e2a", v3, new Lazy<IHalfEdge>(() => e2b));
            e3a = new LineHalfEdge("e3a", v0, new Lazy<IHalfEdge>(() => e3b));
            f0.Add(new EdgeLoop(f0, e0a, e1a, e2a, e3a));

            e0b = new LineHalfEdge("e0b", v0, new Lazy<IHalfEdge>(() => e0a));
            e7a = new LineHalfEdge("e7a", v4, new Lazy<IHalfEdge>(() => e7b));
            e8a = new LineHalfEdge("e8a", v5, new Lazy<IHalfEdge>(() => e8b));
            e4a = new LineHalfEdge("e4a", v1, new Lazy<IHalfEdge>(() => e4b));
            f1.Add(new EdgeLoop(f1, e0b, e7a, e8a, e4a));

            e1b = new LineHalfEdge("e1b", v1, new Lazy<IHalfEdge>(() => e1a));
            e4b = new LineHalfEdge("e4b", v5, new Lazy<IHalfEdge>(() => e4a));
            e9a = new LineHalfEdge("e9a", v6, new Lazy<IHalfEdge>(() => e9b));
            e5a = new LineHalfEdge("e5a", v2, new Lazy<IHalfEdge>(() => e5b));
            f2.Add(new EdgeLoop(f2, e1b, e4b, e9a, e5a));


            e2b = new LineHalfEdge("e2b", v2, new Lazy<IHalfEdge>(() => e2a));
            e5b = new LineHalfEdge("e5b", v6, new Lazy<IHalfEdge>(() => e5a));
            e10a = new LineHalfEdge("e10a", v7, new Lazy<IHalfEdge>(() => e10b));
            e6a = new LineHalfEdge("e6a", v3, new Lazy<IHalfEdge>(() => e6b));
            f3.Add(new EdgeLoop(f3, e2b, e5b, e10a, e6a));


            e3b = new LineHalfEdge("e3b", v3, new Lazy<IHalfEdge>(() => e3a));
            e6b = new LineHalfEdge("e6b", v7, new Lazy<IHalfEdge>(() => e6a));
            e11a = new LineHalfEdge("e11a", v4, new Lazy<IHalfEdge>(() => e11b));
            e7b = new LineHalfEdge("e7b", v0, new Lazy<IHalfEdge>(() => e7a));
            f4.Add(new EdgeLoop(f4, e3b, e6b, e11a, e7b));


            e8b = new LineHalfEdge("e8b", v4, new Lazy<IHalfEdge>(() => e8a));
            e11b = new LineHalfEdge("e11b", v7, new Lazy<IHalfEdge>(() => e11a));
            e10b = new LineHalfEdge("e10b", v6, new Lazy<IHalfEdge>(() => e10a));
            e9b = new LineHalfEdge("e9b", v5, new Lazy<IHalfEdge>(() => e9b));
            f5.Add(new EdgeLoop(f5, e8b, e11b, e10b, e9b));

            var shell = new Shell(f0, f1, f2, f3, f4, f5);

            foreach (var face in shell.Faces)
            {
                Debug.WriteLine("---------------");
                Debug.WriteLine("{0}".fmt(face.Name));
                foreach (var loop in face.Loops)
                {
                    foreach (var edge in loop.Edges)
                    {
                        Debug.WriteLine(edge.Name);
                    }
                }
            }
            Assert.True(true);
        }
    }
}
