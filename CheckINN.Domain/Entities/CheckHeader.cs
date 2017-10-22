namespace CheckINN.Domain.Entities
{
    public class CheckHeader
    {
        public string ShopIdentifier { get; }

        public CheckHeader(string shopIdentifier)
        {
            ShopIdentifier = shopIdentifier;
        }
    }
}