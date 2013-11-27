using System;
using System.Text;
using System.Threading.Tasks;
using OpenCAD.Core.Primatives;
using Veg.Maths;

namespace OpenCAD.Core.Modeling.Octree
{
    public class Octree<T> : OctreeNode<T>
    {
        public Octree(Vect3 center, Double size, int maxLevel)
            : base(center, size, NodeState.Empty, 0, maxLevel)
        {

        }
    }
}
