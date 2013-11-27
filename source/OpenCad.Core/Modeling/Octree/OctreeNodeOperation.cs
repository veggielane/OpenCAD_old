using System;

namespace OpenCAD.Core.Modeling.Octree
{
    public abstract class OctreeNodeOperation<T>
    {
        public OctreeNode<T> Run(OctreeNode<T> a, OctreeNode<T> b)
        {
            if (a.State == NodeState.Empty && b.State == NodeState.Empty)
            {
                return EmptyEmpty(a, b);
            }
            if (a.State == NodeState.Empty && b.State == NodeState.Filled)
            {
                return EmptyFilled(a, b);
            }
            if (a.State == NodeState.Empty && b.State == NodeState.Partial)
            {
                return EmptyPartial(a, b);
            }

            if (a.State == NodeState.Filled && b.State == NodeState.Empty)
            {
                return FilledEmpty(a, b);
            }
            if (a.State == NodeState.Filled && b.State == NodeState.Filled)
            {
                return FilledFilled(a, b);
            }
            if (a.State == NodeState.Filled && b.State == NodeState.Partial)
            {
                return FilledPartial(a, b);
            }

            if (a.State == NodeState.Partial && b.State == NodeState.Empty)
            {
                return PartialEmpty(a, b);
            }
            if (a.State == NodeState.Partial && b.State == NodeState.Filled)
            {
                return PartialFilled(a, b);
            }
            if (a.State == NodeState.Partial && b.State == NodeState.Partial)
            {
                return PartialPartial(a, b);
            }
            throw new Exception();
        }

        protected abstract OctreeNode<T> EmptyEmpty(OctreeNode<T> a, OctreeNode<T> b);
        protected abstract OctreeNode<T> EmptyFilled(OctreeNode<T> a, OctreeNode<T> b);
        protected abstract OctreeNode<T> EmptyPartial(OctreeNode<T> a, OctreeNode<T> b);

        protected abstract OctreeNode<T> FilledEmpty(OctreeNode<T> a, OctreeNode<T> b);
        protected abstract OctreeNode<T> FilledFilled(OctreeNode<T> a, OctreeNode<T> b);
        protected abstract OctreeNode<T> FilledPartial(OctreeNode<T> a, OctreeNode<T> b);

        protected abstract OctreeNode<T> PartialEmpty(OctreeNode<T> a, OctreeNode<T> b);
        protected abstract OctreeNode<T> PartialFilled(OctreeNode<T> a, OctreeNode<T> b);
        protected abstract OctreeNode<T> PartialPartial(OctreeNode<T> a, OctreeNode<T> b);
    }
}