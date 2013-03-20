using System.Collections.Generic;
using OpenCAD.Core.Graphics;


namespace OpenCAD.GUI.SceneGraph
{

    public class OpenGLSceneGraph:BaseSceneGraph
    {
        public override void Render()
        {
            foreach (var node in Nodes)
            {
                node.Render();
            }
        }

        public override void Clear()
        {
            Nodes.Clear();
        }
    }
}
