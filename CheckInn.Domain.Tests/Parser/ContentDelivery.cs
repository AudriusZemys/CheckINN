using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace CheckINN.Domain.Tests.Parser
{
    public class ContentDelivery
    {
        public static IEnumerable<string> GetContent()
        {
            var sample = Assembly
                .GetExecutingAssembly()
                .GetManifestResourceStream("CheckInn.Domain.Tests.Parser.ParserSampleData.checkTest.txt");
            StreamReader reader = new StreamReader(sample);
            string text = reader.ReadToEnd();
            return text.Split(new string[] { "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries).ToList();
        }
    }
}
