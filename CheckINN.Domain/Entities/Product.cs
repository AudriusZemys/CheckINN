namespace CheckINN.Domain.Entities
{
    public struct Product
    {
        public string ProductEntry { get; }
        public decimal Cost { get; }

        public Product(string productEntry, decimal cost)
        {
            ProductEntry = productEntry;
            Cost = cost;
        }
    }
}