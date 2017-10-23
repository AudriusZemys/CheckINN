using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CheckINN.Parser
{
    class ContentDelivery
    {
        public static IEnumerable<string> getContent()
        {
            return File.ReadLines(@"C:\Users\a\source\repos\CheckINN\CheckINN.Frontend\Parsing.Test.Data\checkTest.txt");
        }
    }
}
