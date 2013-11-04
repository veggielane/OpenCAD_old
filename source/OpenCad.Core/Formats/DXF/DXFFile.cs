using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenCAD.Core.Formats.DXF
{
    public class DXFFile
    {
        public IList<string> Comments { get; private set; } 

        public DXFFile()
        {
            Comments = new List<string>();
        }

        public static DXFFile Load(string filename)
        {

            return new DXFReader(filename).Read();
        }
    }

    public class DXFReader
    {
        private DXFStreamReader _tr;

        public DXFReader(string filename)
        {
            _tr = new DXFStreamReader(filename);
        }

        public DXFFile Read()
        {
            var file = new DXFFile();
            GroupValuePair pair;





            while ((pair = _tr.ReadPair()).Group == 999)
            {
                file.Comments.Add(pair.Value);    
            }


            return file;
        }


    }

    public interface ISection
    {
        
    }

    public class Header:ISection
    {
        
    }

    


}
