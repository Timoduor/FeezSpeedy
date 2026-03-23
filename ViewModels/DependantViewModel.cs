// Web/ViewModels/DependantViewModel.cs
namespace FeezSpeedy.Web.ViewModels
{
    public class DependantViewModel
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string AdmissionNumber { get; set; } = string.Empty;
        public string ClassLevel { get; set; } = string.Empty;
        public int SchoolId { get; set; } // for existing school selection
        public string? SchoolName { get; set; } // for manual input
        public string ParentId { get; set; } = string.Empty;
    }
}