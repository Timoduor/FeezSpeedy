using FeezSpeedy.Web.Models;
using System.ComponentModel.DataAnnotations;

namespace FeezSpeedy.Models
{
    public class Dependant
    {
        public int Id { get; set; }

        [Required, StringLength(100)]
        public string? FullName { get; set; }
        public string? AdmissionNumber { get; set; }
        public string? ClassLevel { get; set; }

        // FK to Parent (Identity uses string Id)
        public string? ParentId { get; set; }
        public Parent Parent { get; set; }

        public int SchoolId { get; set; }
        public School School { get; set; }

        public ICollection<FeeRequest>? FeeRequests { get; set; }
    }
}
