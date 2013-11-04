using System;
using System.IO;

namespace OpenCAD.Core.Formats.DXF
{
    public class DXFStreamReader : StreamReader
    {
        public DXFStreamReader(string filename):base(filename)
        {

        }

        public GroupValuePair ReadPair()
        {
            return new GroupValuePair(ReadLine(), ReadLine());
        }
    }

    public struct GroupValuePair
    {
        public int Group { get; private set; }
        public string Value { get; private set; }
        public GroupValuePair(int group, string value)
            : this()
        {
            Group = group;
            Value = value;
        }

        public GroupValuePair(string group, string value)
            : this()
        {
            int number;
            if (!Int32.TryParse(group, out number)) throw new Exception("Invalid Group");
            Group = number;
            Value = value;
        }
    }
}