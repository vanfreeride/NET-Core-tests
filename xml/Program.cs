using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace xml
{
    class Program
    {
        static void Main(string[] args)
        {
            var  stream = new FileStream("/home/van/Dev/NET-Core-tests/xml/test.xml", FileMode.Open);

            var ser = new XmlSerializer(typeof(el));

            var res = ser.Deserialize(stream);

        }
    }

    public class el {
        public string to { get; set; }
        public string from { get; set; }
        public string heading { get; set; }
        public note[] notes { get; set; }
    }
    public class note{
        public string text {get;set;}
    }
}
