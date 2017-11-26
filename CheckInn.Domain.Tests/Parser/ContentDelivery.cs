using System.Collections.Generic;
using System.IO;

namespace CheckINN.Domain.Parser
{
    public class ContentDelivery
    {
        public static IEnumerable<string> GetContent()
        {
            return File.ReadLines(@".\ParserSampleData\checkTest.txt");
        }
    }
}
