using System.Collections.Generic;

namespace OpenCAD.Core.Graphics
{
    public interface ISceneGraph
    {
        IList<ILeafNode> Nodes { get; }
        void Render();
        void Clear();
    }

    public abstract class BaseSceneGraph:ISceneGraph
    {
        public IList<ILeafNode> Nodes { get; private set; }

        public BaseSceneGraph()
        {
            Nodes = new List<ILeafNode>();
        }

        public abstract void Render();
        public abstract void Clear();
    }

    public interface IGroupNode
    {

    }

    public interface ILeafNode
    {
        void Render();
    }
}