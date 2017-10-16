namespace CheckINN.Domain.Entities
{
    internal class CheckHeader
    {
        public string ShopIdentifier { get; }

        public CheckHeader(string shopIdentifier)
        {
            ShopIdentifier = shopIdentifier;
        }
    }
}