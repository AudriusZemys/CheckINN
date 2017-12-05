using System.Data.Entity;
using CheckINN.Repository.Entities;

namespace CheckINN.Repository.Contexts
{
    public class ReceiptsContext : DbContext
    {
        public virtual IDbSet<ProductListing> ProductListings { get; set; }
        public virtual IDbSet<Check> Checks { get; set; }
    }
}
