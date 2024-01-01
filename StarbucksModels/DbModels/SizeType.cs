using System.ComponentModel.DataAnnotations;

namespace StarbucksModels.DbModels
{
    public class SizeType
    {
        [Key]
        public int SizeTypeId { get; set; }
        public string SizeTypeName { get; set; }
    }
}
