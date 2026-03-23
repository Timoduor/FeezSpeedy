public class PaymentOption
{
    public int Id { get; set; }
    public string Name { get; set; } = null!; // e.g., "MPESA", "Bank Transfer"
    public string ProviderCode { get; set; } = null!; // For APIs
    public bool IsActive { get; set; } = true;
}
