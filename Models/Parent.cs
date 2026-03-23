using FeezSpeedy.Models; // add this at the top
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FeezSpeedy.Web.Models
{
    public class Parent : IdentityUser
    {
        // IdentityUser already provides:
        // Id (string)
        // Email
        // UserName
        // PhoneNumber
        // PasswordHash, etc.

        [StringLength(100)]
        public string? FullName { get; set; }
        [StringLength(8, MinimumLength = 8)] 
        public string? NationalId { get; set; }
        [StringLength(20)] 
        public string? PassportNumber { get; set; }
        public string? PhotoPath { get; set; }

        public ICollection<Dependant>? Dependants { get; set; }
    }
}
