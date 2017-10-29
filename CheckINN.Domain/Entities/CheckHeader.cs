using System;
using static CheckINN.Domain.Entities.ShopIdentifier;

namespace CheckINN.Domain.Entities
{
    public struct CheckHeader
    {
        public ShopIdentifier ShopIdentifier { get; }

        public string ShopIdentifierString => ShopIdentifier.ToString();

        public CheckHeader(ShopIdentifier shopIdentifier = Unknown)
        {
            ShopIdentifier = shopIdentifier;
        }

        public CheckHeader(string shopIdentifierString)
        {
            ShopIdentifier = (ShopIdentifier)Enum.Parse(typeof(ShopIdentifier), shopIdentifierString);
        }
    }
}