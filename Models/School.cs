// Models/School.cs
using System.ComponentModel.DataAnnotations;

namespace FeezSpeedy.Models
{
    public class School
    {
        public int Id { get; set; }

        [Required, StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required, StringLength(100)]
        public string Location { get; set; } = "Unknown"; // default for manual input

        public ICollection<Dependant> Dependants { get; set; } = new List<Dependant>();
    }
}