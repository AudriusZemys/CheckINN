using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace CheckINN.Domain.Parser
{
    public class Parser : IParser
    {
        private readonly string _separatorDash = "----------";
        private readonly string _separatorDoubleDash = "==========";
        private readonly string _separatorKvitas = "Kvitas";


        private Regex shopNameRegex = new Regex("UAB", RegexOptions.IgnoreCase);
        private Regex priceRegex = new Regex("[0-9]+,[0-9][0-9]", RegexOptions.IgnoreCase);

        public String ShopName { get; set; }
        public List<Tuple<string, double>> Products { get; set; }

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
                beginningSeparator = _separatorKvitas;
                endingSeparator = _separatorDoubleDash;
            }
            if (ShopName.Equals("RIMI"))
            {
                beginningSeparator = _separatorDash;
                endingSeparator = _separatorDash;
            }
            if (ShopName.Equals("IKI"))
            {
                beginningSeparator = _separatorDash;
                endingSeparator = _separatorDash;
            }
            if (ShopName.Equals("LIDL"))
            {
                beginningSeparator = _separatorKvitas;
                endingSeparator = _separatorDash;
            }
            string product = "";
            double price;
            bool nextline = false;
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
                    if (nextline)
                    {
                        price = Convert.ToDouble(match);
                        Products.Add(new Tuple<string, double>(product, price));
                        nextline = false;
                    }
                    if (match.Length > 0 && !nextline)
                    {
                        product = line.Substring(0, line.IndexOf(match)).Trim();
                        price = Convert.ToDouble(match);
                        Products.Add(new Tuple<string, double>(product, price));
                    }
                    else
                    {
                        product = line.Trim();
                        nextline = true;
                    }

                }
            }

            return status;
        }
    }
}
