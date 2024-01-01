using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarbucksModels.DbModels
{
    public class OrderHeader
    {
        [Key]
        public int Id { get; set; }
        public string ApplicationUserId { get; set; } = null!;
        [DataType(DataType.Date)]
        public DateTime OrderDate { get; set; } = DateTime.Now;
        public decimal OrderTotal { get; set; }
        [Required]
        public string OrderStatus { get; set; } = null!;
        [Required]
        public string PaymentStatus { get; set; } = null!;
        public string? TrackingNumber { get; set; }
        public string? SessionId { get; set; }
        public string? PaymentIntentId { get; set; }
        public DateTime Dt_Rec_Added { get; set; } = DateTime.Now;

        public DateTime? Dt_Rec_Modified { get; set; }
    }
}
