
using Veg.Maths;

namespace OpenCAD.Core.Primatives
{
    public class AABB:IPrimative
    {
        public Vect3 Min { get; private set; }
        public Vect3 Max { get; private set; }
        public Vect3 Center{ get; private set; } 

        public AABB(Vect3 min, Vect3 max)
        {
            Min = min;
            Max = max;
            Center = (Min + Max)*0.5;
        }

        public AABB(Vect3 center, double size)
        {
            Center = center;
            Min = center - new Vect3(size / 2.0, size / 2.0, size / 2.0);
            Max = center + new Vect3(size / 2.0, size / 2.0, size / 2.0);
        }
    }

    public class Triangle : IPrimative
    {
        public Vect3 P0 { get; private set; }
        public Vect3 P1 { get; private set; }
        public Vect3 P2 { get; private set; }
        public Vect3 Normal { get; private set; }

        public Triangle(Vect3 p0, Vect3 p1, Vect3 p2, Vect3 normal)
        {
            P0 = p0;
            P1 = p1;
            P2 = p2;
            Normal = normal;
        }
        public Triangle(Vect3 p0, Vect3 p1, Vect3 p2)
            : this(p0, p1, p2, Vect3.Zero)
        {

        }
    }
}
