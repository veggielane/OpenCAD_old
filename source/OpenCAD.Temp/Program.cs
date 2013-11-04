using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenCAD.Core.Formats.DXF;

namespace OpenCAD.Temp
{
    class Program
    {
        static void Main(string[] args)
        {
            var test = DXFFile.Load("test.dxf");
            Console.ReadLine();
        }
    }
}
