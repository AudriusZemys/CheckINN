﻿using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using log4net;

namespace CheckINN.Domain.Parser
{
    public class Parser : IParser
    {
        private readonly string _separatorDash = "----------";
        private readonly string _separatorDoubleDash = "==========";
        private readonly string _separatorKvitas = "Kvitas";


        private readonly Regex _shopNameRegex = new Regex("UAB", RegexOptions.IgnoreCase);
        private readonly Regex _priceRegex = new Regex("[0-9]+,[0-9][0-9]");
        private readonly Regex _discountRegex = new Regex("-[0-9]+,[0-9][0-9]");

        public String ShopName { get; set; }
        public string ShopAddress { get; set; }
        public List<Tuple<string, decimal>> Products { get; set; }

        public IEnumerable<string> Content { get; set; }


        public void Parse()
        {
            if (!FindShopName())
            {
                throw new Exception("Shop name cannot be identified");
            }
            ExtractProductList();
        }

        public Parser(IEnumerable<string> content)
        {
            Products = new List<Tuple<string, decimal>>();
            this.Content = content;
        }

        public bool FindShopName()
        {
            bool status = false;
            foreach (var line in Content)
            {
                if (status)
                {
                    if (string.IsNullOrEmpty(line)) continue;
                    ShopAddress = line;
                    break;
                }
                if (_shopNameRegex.Match(line).Length > 0)
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
                }
            }
            return status;
        }

        public bool ExtractProductList()
        {
            bool status = true;
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
            decimal price;
            bool nextline = false;
            foreach (var line in Content)
            {
                if (_discountRegex.Match(line).Success)
                {
                    continue;
                }
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
                    string match = _priceRegex.Match(line).Value;
                    if (nextline)
                    {
                        price = Convert.ToDecimal(match.Replace(",", "."));
                        Products.Add(new Tuple<string, decimal>(product, price));
                        nextline = false;
                        continue;
                    }
                    if (match.Length > 0)
                    {
                        product = line.Substring(0, line.IndexOf(match)).Trim();
                        price = Convert.ToDecimal(match.Replace(",", "."));
                        Products.Add(new Tuple<string, decimal>(product, price));
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
