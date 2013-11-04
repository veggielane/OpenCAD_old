using Veg.Maths;
using Veg.Maths.Geometry;

namespace OpenCAD.Core.Datums
{
    public class DatumPlane : Plane, IDatum
    {
        public string Name { get; private set; }
        public DatumPlane(string name, Vect3 normal, double distance)
            : base(normal, distance)
        {
            Name = name;
        }

        public DatumPlane(string name, Vect3 normal, Vect3 point)
            : base(normal, point)
        {
            Name = name;
        }

        public DatumPlane(string name, Vect3 point1, Vect3 point2, Vect3 point3)
            : base(point1, point2, point3)
        {
            Name = name;
        }
    }
}