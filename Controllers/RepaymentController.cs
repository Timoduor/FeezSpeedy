using FeezSpeedy.Models;
using FeezSpeedy.Web.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


[Authorize]
public class RepaymentController : Controller
{
    private readonly ApplicationDbContext _context;

    public RepaymentController(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index(int feeRequestId)
    {
        var schedules = await _context.RepaymentSchedules
            .Where(r => r.FeeRequestId == feeRequestId)
            .ToListAsync();

        ViewBag.PaymentOptions = await _context.PaymentOptions
            .Where(p => p.IsActive)
            .ToListAsync();

        return View(schedules);
    }

    [HttpPost]
    public async Task<IActionResult> Pay(int scheduleId, decimal amount, PaymentMethod method)
    {
        var schedule = await _context.RepaymentSchedules.FindAsync(scheduleId);
        if (schedule == null || schedule.IsPaid) return BadRequest();

        _context.Repayments.Add(new Repayment
        {
            RepaymentScheduleId = scheduleId,
            Amount = amount,
            PaymentMethod = method
        });

        schedule.IsPaid = true;
        schedule.PaidAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index), new { feeRequestId = schedule.FeeRequestId });
    }
}
