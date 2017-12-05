using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CheckINN.Repository.Entities
{
    public class ProductListing
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ProductListingId { get; set; }
        [ForeignKey("CheckId")]
        public Check Check { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
    }
}
