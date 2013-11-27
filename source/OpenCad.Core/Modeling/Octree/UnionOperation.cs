using System.Linq;

namespace OpenCAD.Core.Modeling.Octree
{
    public class UnionOperation<T>:OctreeNodeOperation<T>
    {
        /*
         *       b
         *    |E|F|P
         *   E|a|b|b
         * a F|a|a|a
         *   P|a|b|r
         */
        protected override OctreeNode<T> EmptyEmpty(OctreeNode<T> a, OctreeNode<T> b)
        {
            return a;
        }

        protected override OctreeNode<T> EmptyFilled(OctreeNode<T> a, OctreeNode<T> b)
        {
            return b;
        }

        protected override OctreeNode<T> EmptyPartial(OctreeNode<T> a, OctreeNode<T> b)
        {
            return b;
        }

        protected override OctreeNode<T> FilledEmpty(OctreeNode<T> a, OctreeNode<T> b)
        {
            return a;
        }

        protected override OctreeNode<T> FilledFilled(OctreeNode<T> a, OctreeNode<T> b)
        {
            return a;
        }

        protected override OctreeNode<T> FilledPartial(OctreeNode<T> a, OctreeNode<T> b)
        {
            return a;
        }

        protected override OctreeNode<T> PartialEmpty(OctreeNode<T> a, OctreeNode<T> b)
        {
            return a;
        }

        protected override OctreeNode<T> PartialFilled(OctreeNode<T> a, OctreeNode<T> b)
        {
            return b;
        }

        protected override OctreeNode<T> PartialPartial(OctreeNode<T> a, OctreeNode<T> b)
        {
            return new OctreeNode<T>(a.Center, a.Size, a.Children.Zip(b.Children, new UnionOperation<T>().Run).ToArray(), a.Level, a.MaxLevel);
        }
    }
}