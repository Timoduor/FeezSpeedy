using FeezSpeedy.Models;

namespace FeezSpeedy.Web.ViewModels
{
    public class DashboardViewModel
    {
        public int DependantsCount { get; set; }
        public int FeeRequestsCount { get; set; }
        public List<Dependant> Dependants { get; set; } = new();
        public List<FeeRequest> FeeRequests { get; set; } = new();
        public decimal OutstandingBalance { get; set; }
    }
}
