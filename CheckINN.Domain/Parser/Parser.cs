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
            if (!FindShopName())
            {
                throw new Exception("Shop name cannot be identified");
            }
        }

        public Parser(IEnumerable<string> content)
        {
            this.Content = content;
        }

        public bool FindShopName()
        {
            bool status = false;
            foreach (var line in Content)
            {
                if (shopNameRegex.Match(line).Length > 0)
                {
                    if (line.IndexOf("MAXIMA", StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        ShopName = "MAXIMA";
                    }
                    if (line.IndexOf("RIMI", StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        ShopName = "RIMI";
                    }
                    // official name or corporate entity for IKI is PALINK
                    if (line.IndexOf("PALINK", StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        ShopName = "IKI";
                    }
                    if (line.IndexOf("LIDL", StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        ShopName = "LIDL";
                    }
                    status = true;
                    break;
                }
            }
            return status;
        }

        public bool ExtractProductList()
        {
            bool status = false;

            return status;
        }
    }
}
