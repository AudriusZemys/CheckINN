using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using CheckINN.Parser.Interfaces;

namespace CheckINN.Parser
{
    public class Parser : IParser
    {
        public String ShopName { get; set; }
        public IEnumerable<Tuple<string, double>> Products { get; set; }

        public IEnumerable<string> Content { get; set; }

        public void Parse()
        {
            throw new NotImplementedException();
        }

        public Parser()
        {
            //change this to something more reasonable later
            Content = ContentDelivery.getContent();
        }



        //public bool ShopName(string path, string pattern = "UAB")
        //{
        //    //path = @"C:\Users\Audrius\checkText.txt"
        //    IEnumerable<string> result = File.ReadLines(path)
        //        .Where(l => l.Contains(pattern));
        //    {
        //        return true;
        //    }
        //    return false;
        //}
    }
}
