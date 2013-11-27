using System.Linq;

namespace OpenCAD.Core.Modeling.Octree
{
    public class SubtractOperation<T> : OctreeNodeOperation<T>
    {
        /*
         *       b
         *    |E|F|P
         *   E|E|E|E
         * a F|a|E|r
         *   P|a|E|r
         */
        protected override OctreeNode<T> EmptyEmpty(OctreeNode<T> a, OctreeNode<T> b)
        {
            return new OctreeNode<T>(a.Center, a.Size, NodeState.Empty, a.Level, a.MaxLevel);
        }

        protected override OctreeNode<T> EmptyFilled(OctreeNode<T> a, OctreeNode<T> b)
        {
            return new OctreeNode<T>(a.Center, a.Size, NodeState.Empty, a.Level, a.MaxLevel);
        }

        protected override OctreeNode<T> EmptyPartial(OctreeNode<T> a, OctreeNode<T> b)
        {
            return new OctreeNode<T>(a.Center, a.Size, NodeState.Empty, a.Level, a.MaxLevel);
        }

        protected override OctreeNode<T> FilledEmpty(OctreeNode<T> a, OctreeNode<T> b)
        {
            return a;
        }

        protected override OctreeNode<T> FilledFilled(OctreeNode<T> a, OctreeNode<T> b)
        {
            return new OctreeNode<T>(a.Center, a.Size, NodeState.Empty, a.Level, a.MaxLevel);
        }

        protected override OctreeNode<T> FilledPartial(OctreeNode<T> a, OctreeNode<T> b)
        {
            return new OctreeNode<T>(a.Center, a.Size, new OctreeNode<T>(a.Center, a.Size, a.CreateChildren(NodeState.Filled), a.Level, a.MaxLevel).Children.Zip(b.Children, new SubtractOperation<T>().Run).ToArray(), a.Level, a.MaxLevel);
        }

        protected override OctreeNode<T> PartialEmpty(OctreeNode<T> a, OctreeNode<T> b)
        {
            return a;
        }

        protected override OctreeNode<T> PartialFilled(OctreeNode<T> a, OctreeNode<T> b)
        {
            return new OctreeNode<T>(a.Center, a.Size, NodeState.Empty, a.Level, a.MaxLevel);
        }

        protected override OctreeNode<T> PartialPartial(OctreeNode<T> a, OctreeNode<T> b)
        {
            return new OctreeNode<T>(a.Center, a.Size, a.Children.Zip(b.Children, new SubtractOperation<T>().Run).ToArray(), a.Level, a.MaxLevel);
        }
    }
}