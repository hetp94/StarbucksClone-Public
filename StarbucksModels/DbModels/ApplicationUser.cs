using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace StarbucksModels.DbModels
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        public string LastName { get; set; }
        [Required]
        public string FirstName { get; set; }
        public string? StreetAddress { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? PostalCode { get; set; }
    }
}
