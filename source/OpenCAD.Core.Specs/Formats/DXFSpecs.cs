using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Machine.Specifications;
using OpenCAD.Core.Formats;
using OpenCAD.Core.Formats.DXF;
using developwithpassion.specifications.fakeiteasy;

namespace OpenCAD.Core.Specs.Formats
{
    [Subject(typeof(DXFFile))]
    public class with_DXF : Observes<DXFFile>
    {

    }
}
