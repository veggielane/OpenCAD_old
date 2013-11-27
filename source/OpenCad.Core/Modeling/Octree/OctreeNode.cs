using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Veg.Maths;
using Veg.Maths.Geometry;

namespace OpenCAD.Core.Modeling.Octree
{
    public class OctreeNode<T>
    {
        public int Level { get; private set; }
        public int MaxLevel { get; private set; }
        public Vect3 Center { get; private set; }
        public double Size { get; private set; }
        public T Data { get; private set; }

        public IEnumerable<OctreeNode<T>> Children { get; private set; }
        public AABB AABB { get { return new AABB(Center + new Vect3(-Size, -Size, -Size) / 2.0, Center + new Vect3(Size, Size, Size) / 2.0); } }
        public NodeState State { get; private set; }

        public OctreeNode(Vect3 center, Double size, NodeState state, int level, int maxLevel)
        {
            Center = center;
            Size = size;
            State = state;
            Level = level;
            MaxLevel = maxLevel;
            Children = Enumerable.Empty<OctreeNode<T>>();
        }

        public OctreeNode(Vect3 center, Double size, IEnumerable<OctreeNode<T>> children, int level, int maxLevel)
            : this(center, size, NodeState.Partial, level, maxLevel)
        {
            Children = children;
        }

        public IEnumerable<OctreeNode<T>> CreateChildren(NodeState state = NodeState.Empty)
        {
            var newSize = Size / 2.0;
            var half = Size / 4.0;
            return new[]
            {
                //top-front-right
                new OctreeNode<T>(Center + new Vect3(+half, +half, +half),newSize, state, Level + 1, MaxLevel),
                //top-back-right
                new OctreeNode<T>(Center + new Vect3(-half, +half, +half),newSize, state, Level + 1, MaxLevel),
                //top-back-left
                new OctreeNode<T>(Center + new Vect3(-half, -half, +half),newSize, state, Level + 1, MaxLevel),
                //top-front-left
                new OctreeNode<T>(Center + new Vect3(+half, -half, +half),newSize, state, Level + 1, MaxLevel),
                //bottom-front-right
                new OctreeNode<T>(Center + new Vect3(+half, +half, -half),newSize, state, Level + 1, MaxLevel),
                //bottom-back-right
                new OctreeNode<T>(Center + new Vect3(-half, +half, -half),newSize, state, Level + 1, MaxLevel),
                //bottom-back-left
                new OctreeNode<T>(Center + new Vect3(-half, -half, -half),newSize, state, Level + 1, MaxLevel),
                //bottom-front-left
                new OctreeNode<T>(Center + new Vect3(+half, -half, -half),newSize, state, Level + 1, MaxLevel)
            };
        }

        public OctreeNode<T> this[int index]
        {
            get
            {
                if (index > 7 || index < 0) throw new ArgumentOutOfRangeException("index");
                return Children.ElementAtOrDefault(index);
            }
        }

        public IEnumerable<OctreeNode<T>> Flatten()
        {
            yield return this;
            foreach (var child in Children)
            {
                foreach (var c in child.Flatten())
                {
                    yield return c;
                }
            }
        }


        public OctreeNode<T> Intersect(Func<OctreeNode<T>, NodeIntersectResult> func)
        {
            if (Level > MaxLevel) return this;
            switch (func(this))
            {
                case NodeIntersectResult.False:
                    return this;
                case NodeIntersectResult.True:
                    if (Level == MaxLevel)
                    {
                        return new OctreeNode<T>(Center, Size, NodeState.Filled, Level, MaxLevel);
                    }
                    return new OctreeNode<T>(Center, Size, CreateChildren().Select(c=>c.Intersect(func)), Level, MaxLevel);
                case NodeIntersectResult.Inside:
                    return new OctreeNode<T>(Center, Size, NodeState.Filled, Level, MaxLevel);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }


        public OctreeNode<T> Union(OctreeNode<T> b)
        {
            return new UnionOperation<T>().Run(this, b);
        }

        public OctreeNode<T> Subtract(OctreeNode<T> b)
        {
            return new SubtractOperation<T>().Run(this, b);
        }
        
        public OctreeNode<T> Intersect(OctreeNode<T> b)
        {
            return new IntersectOperation<T>().Run(this, b);
        }
        
    }

    public enum NodeIntersectResult : byte { False = 0, True = 1, Inside = 2 }
}