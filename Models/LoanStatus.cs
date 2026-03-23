namespace FeezSpeedy.Models
{
    public class LoanStatus
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        // Optional: ordering / UI friendliness
        public int SortOrder { get; set; }
    }
}