using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CheckINN.Parser.Interfaces;

namespace CheckINN.Parser
{
    public class Parser : IParser
    {
        private Regex r = new Regex("UAB", RegexOptions.IgnoreCase);
        public String ShopName { get; set; }
        public IEnumerable<Tuple<string, double>> Products { get; set; }

        public IEnumerable<string> Content { get; set; }

        public void Parse()
        {
            throw new NotImplementedException();
        }

        public Parser(IEnumerable<string> content)
        {
            this.Content = content;
        }

        public bool FindShopName()
        {
            foreach (var line in Content)
            {
                if (r.Match(line).Length > 0)
                {
                    ShopName = line;
                    return true;
                }
            }
            return false;
        }
    }
}
