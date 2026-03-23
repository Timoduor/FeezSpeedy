using FeezSpeedy.Models;

public class Repayment
{
    public int Id { get; set; }

    public int FeeRequestId { get; set; }
    public FeeRequest FeeRequest { get; set; } = null!;

    public decimal Amount { get; set; }

    public PaymentMethod PaymentMethod { get; set; }

    public DateTime PaidAt { get; set; } = DateTime.UtcNow;

    public string? Reference { get; set; } // Mpesa receipt / bank ref

    public int RepaymentScheduleId { get; set; }
    public RepaymentSchedule RepaymentSchedule { get; set; } = null!;
}
