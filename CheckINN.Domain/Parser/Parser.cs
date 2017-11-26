using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace CheckINN.Domain.Parser
{
    public class Parser : IParser
    {
        private readonly string separator_dash = "----------";
        private readonly string separator_double_dash = "==========";
        private readonly string separator_kvitas = "Kvitas";


        private Regex shopNameRegex = new Regex("UAB", RegexOptions.IgnoreCase);
        private Regex priceRegex = new Regex("[0-9]+,[0-9][0-9]", RegexOptions.IgnoreCase);

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
            bool triggerred = false;
            string beginningSeparator = "";
            string endingSeparator = "";
            if (ShopName.Equals("MAXIMA"))
            {
                beginningSeparator = separator_kvitas;
                endingSeparator = separator_double_dash;
            }
            foreach (var line in Content)
            {
                if (!triggerred && line.Contains(beginningSeparator))
                {
                    triggerred = true;
                    continue;
                }
                if (triggerred && line.Contains(endingSeparator))
                {
                    break;
                }
                if (triggerred)
                {
                    string match = priceRegex.Match(line).Value;
                    if (match.Length > 0)
                    {
                        string sub = line.Substring(0, line.IndexOf(match)).Trim();
                        double price = Convert.ToDouble(match);
                    }
                }
            }

            return status;
        }
    }
}
