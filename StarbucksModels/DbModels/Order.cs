using System.ComponentModel.DataAnnotations;

namespace StarbucksModels.DbModels
{
    public class Order
    {
        [Key]
        public int OrderId { get; set; }
        public string AppUserId { get; set; }
        public string PaymenthMethod { get; set; }
        public string PaymentStatus { get; set; }
        public decimal SubTotal { get; set; }
        public decimal? Tax { get; set; }
        public decimal Total { get; set; }
        public DateTime Dt_Rec_Added { get; set; } = DateTime.Now;
    }
}
