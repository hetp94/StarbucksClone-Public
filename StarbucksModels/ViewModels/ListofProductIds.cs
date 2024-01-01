using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarbucksModels.ViewModels
{
    public class ListofProductIds
    {
        [Required]
        public int ProductId { get; set; }
        public string ProductName { get; set; } = null!;
    }
}
