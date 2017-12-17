using System.Data.Entity;
using CheckINN.Repository.Entities;
using CheckINN.Repository.Migrations;

namespace CheckINN.Repository.Contexts
{
    public class CheckINNContext : DbContext
    {
        public CheckINNContext() : this("CheckINN.Datastore") { }

        public CheckINNContext(string databaseName) : base(databaseName) {}

        public virtual IDbSet<ProductListing> ProductListings { get; set; }
        public virtual IDbSet<Check> Checks { get; set; }
        public virtual IDbSet<Product> Products { get; set; }
        public virtual IDbSet<Shop> Shops { get; set; }
    }
}
