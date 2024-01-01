using System.ComponentModel.DataAnnotations;

namespace StarbucksModels.DbModels
{
    public class CustomizationVisibility
    {
        [Key]
        public int Id { get; set; }
        public int ProductId { get; set; }
        public bool AddIns { get; set; } = false;
        public bool BlendedOption { get; set; } = false;
        public bool ButterAndSpread { get; set; } = false;
        public bool CupOption { get; set; } = false;
        public bool EspresspAndShot { get; set; } = false;
        public bool Flavors { get; set; } = false;
        public bool GrindOption { get; set; } = false;
        public bool Ice { get; set; } = false;
        public bool JuiceOption { get; set; } = false;
        public bool Lemonade { get; set; } = false;
        public bool Milk { get; set; } = false;
        public bool OatmealTopping { get; set; } = false;
        public bool PreparationMethod { get; set; } = false;
        public bool SandwichOption { get; set; } = false;

        public bool StarbucksRefreshers { get; set; } = false;
        public bool Sweetners { get; set; } = false;
        public bool Tea { get; set; } = false;
        public bool Topping { get; set; } = false;
        public bool Warmed { get; set; } = false;
        public bool Water { get; set; } = false;


        public string? AddIns1 { get; set; }
        public string? BlendedOption1 { get; set; }
        public string? ButterAndSpread1 { get; set; }
        public string? CupOption1 { get; set; }
        public string? EspresspAndShot1 { get; set; }
        public string? Flavors1 { get; set; }
        public string? GrindOption1 { get; set; }
        public string? Ice1 { get; set; }
        public string? JuiceOption1 { get; set; }
        public string? Lemonade1 { get; set; }
        public string? Milk1 { get; set; }
        public string? OatmealTopping1 { get; set; }
        public string? PreparationMethod1 { get; set; }
        public string? SandwichOption1 { get; set; }

        public string? StarbucksRefreshers1 { get; set; }
        public string? Sweetners1 { get; set; }
        public string? Tea1 { get; set; }
        public string? Topping1 { get; set; }
        public string? Warmed1 { get; set; }
        public string? Water1 { get; set; }
    }
}
