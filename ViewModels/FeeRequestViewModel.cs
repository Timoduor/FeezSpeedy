using System.ComponentModel.DataAnnotations;

namespace FeezSpeedy.Web.ViewModels
{
    public class FeeRequestViewModel
    {
        public int Id { get; set; }

        [Required]
        public int DependantId { get; set; }

        [Required]
        [Range(0, 1000000)]
        public decimal Amount { get; set; }

        [Range(0, 100)]
        public decimal InterestRate { get; set; } = 5;

        public decimal TotalPayable { get; set; }

        public int DurationMonths { get; set; } = 1;
    }
}