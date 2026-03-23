using FeezSpeedy.Web.Models; // For Parent
using System;
using System.ComponentModel.DataAnnotations;

namespace FeezSpeedy.Models
{
    public enum FeeStatus { Pending, Approved, Rejected, Declined, Paid }

    public class FeeRequest
    {
        public int Id { get; set; }

        public int DependantId { get; set; }
        public Dependant Dependant { get; set; } = null!;

        public string ParentId { get; set; } // FK to Parent
        public Parent Parent { get; set; } = null!; // 👈 Added navigation property

        [Range(0, 1000000)]
        public decimal Amount { get; set; }

        [Range(0, 100)]
        public decimal InterestRate { get; set; } = 0;

        public decimal TotalPayable { get; set; }
        public FeeStatus Status { get; set; } = FeeStatus.Pending;

        public DateTime CreatedAt { get; set; }

        public string? ApprovalMessage { get; set; }

        // Payment tracking
        public PaymentMethod? PaymentMethod { get; set; }
        public bool IsPaid { get; set; } = false;
        public DateTime? PaidAt { get; set; }
        public bool IsDisbursed { get; set; }
        public DateTime? DisbursedAt { get; set; }
        public int DurationMonths { get; set; } = 1;

        public int LoanStatusId { get; set; }
        public LoanStatus LoanStatus { get; set; } = null!;

        public ICollection<RepaymentSchedule> RepaymentSchedules { get; set; } = new List<RepaymentSchedule>();


    }

    public enum PaymentMethod
    {
        Mpesa,
        BankTransfer,
        Card
    }
}
