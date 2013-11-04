using System.Collections.Generic;

using OpenCAD.Core.Modeling.Features;
using Veg.Maths.Geometry;

namespace OpenCAD.Core.Modeling.Sections
{
    public interface ISection:IFeature 
    {
        Plane Location { get; }
        //IList<Vertex> Points { get; } 
        //IList<IEntity> Entities { get; } 
    }
}