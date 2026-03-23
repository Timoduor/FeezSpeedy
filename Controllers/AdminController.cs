using FeezSpeedy.Models;
using FeezSpeedy.Web.Data;
using FeezSpeedy.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Authorize(Roles = "Admin")]
public class AdminController : Controller
{
    private readonly ApplicationDbContext _context;
    public AdminController(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var feeRequests = await _context.FeeRequests
            .Include(f => f.Dependant)
            .ThenInclude(d => d.Parent)
            .ToListAsync();

        var vm = new DashboardViewModel
        {
            FeeRequests = feeRequests,
            FeeRequestsCount = feeRequests.Count,
            // You can add other fields like DependantsCount if needed
            DependantsCount = await _context.Dependants.CountAsync()
        };

        return View(vm); // pass the ViewModel
    }


    [HttpPost]
    public async Task<IActionResult> UpdateStatus(
    int feeRequestId,
    string status,
    string? message,
    PaymentMethod? paymentMethod,
    bool? markPaid)
    {
        var feeRequest = await _context.FeeRequests.FindAsync(feeRequestId);
        if (feeRequest == null) return NotFound();

        feeRequest.Status = Enum.Parse<FeeStatus>(status);
        feeRequest.ApprovalMessage = message;

        if (paymentMethod != null)
            feeRequest.PaymentMethod = paymentMethod;

        if (markPaid == true)
        {
            feeRequest.IsPaid = true;
            feeRequest.PaidAt = DateTime.UtcNow;
        }

        // create repayment schedule if approved
        if (feeRequest.Status == FeeStatus.Approved)
        {
            var monthly = feeRequest.TotalPayable / feeRequest.DurationMonths;

            for (int i = 1; i <= feeRequest.DurationMonths; i++)
            {
                _context.RepaymentSchedules.Add(new RepaymentSchedule
                {
                    FeeRequestId = feeRequest.Id,
                    InstallmentNumber = i,
                    DueAmount = monthly,
                    DueDate = DateTime.UtcNow.AddMonths(i)
                });
            }
        }

        await _context.SaveChangesAsync();

        TempData["Message"] = "Fee request updated successfully!";
        return RedirectToAction("Index");
    }
}
