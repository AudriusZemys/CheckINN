using System.Collections.Generic;
using System.IO;

namespace CheckINN.Domain.Parser
{
    class ContentDelivery
    {
        public static IEnumerable<string> getContent()
        {
            return File.ReadLines(@"C:\Users\a\source\repos\CheckINN\CheckINN.Frontend\Parsing.Test.Data\checkTest.txt");
        }
    }
}
