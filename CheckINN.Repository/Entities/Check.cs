using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CheckINN.Repository.Entities
{
    public class Check
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CheckId { get; set; }
        public DateTime Date { get; set; }
        public int ShopId { get; set; }
        public int UserId { get; set; }
        public bool IsValid { get; set; }

        [ForeignKey("ShopId")]
        public Shop Shop { get; set; }
        [ForeignKey("UserId")]
        public User User { get; set; }

    }
}