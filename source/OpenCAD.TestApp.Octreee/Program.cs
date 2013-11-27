using System;
using OpenCAD.Core;
using OpenCAD.Core.Modeling.Octree;
using Veg.Maths;
using Veg.Maths.Geometry;

namespace OpenCAD.TestApp.Octree
{
    class Program
    {
        static void Main(string[] args)
        {
            var s1 = new Sphere { Center = Vect3.Zero, Radius = 4 };
            var s2 = new Sphere { Center = new Vect3(3, 3, 3), Radius = 4 };

            var b = new Octree<TestVoxel>(Vect3.Zero, 16, 8);

            var t1 = b.Intersect(node =>
                {
                    if (node.AABB.Inside(s1)) return NodeIntersectResult.Inside;
                    if (s1.Intersects(node.AABB)) return NodeIntersectResult.True;
                    return NodeIntersectResult.False;
                });
            var t2 = b.Intersect(node =>
            {
                if (node.AABB.Inside(s2)) return NodeIntersectResult.Inside;
                if (s2.Intersects(node.AABB)) return NodeIntersectResult.True;
                return NodeIntersectResult.False;
            });

            using (var win = new Window(t1.Subtract(t2)))
            {
                win.Run();
                Console.ReadLine();
            }
        }
    }
}
