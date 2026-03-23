using System;

namespace FeezSpeedy.Models
{
    public class RepaymentSchedule
    {
        public int Id { get; set; }

        public int FeeRequestId { get; set; }
        public FeeRequest FeeRequest { get; set; } = null!;

        public int InstallmentNumber { get; set; }

        public decimal DueAmount { get; set; }

        public DateTime DueDate { get; set; }

        public bool IsPaid { get; set; } = false;

        public DateTime? PaidAt { get; set; }
    }
}
