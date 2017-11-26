using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace CheckINN.Domain.Parser
{
    public class Parser : IParser
    {
        private Regex shopNameRegex = new Regex("UAB", RegexOptions.IgnoreCase);
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
                if (shopNameRegex.Match(line).Length > 0)
                {
                    this.ShopName = line;
                    return true;
                }
            }
            return false;
        }
    }
}
