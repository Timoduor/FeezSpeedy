using FeezSpeedy.Models;
using FeezSpeedy.Web.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Authorize(Roles = "Admin")]
public class AdminDisbursementController : Controller
{
    private readonly ApplicationDbContext _context;

    public AdminDisbursementController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet("requests")]
    public async Task<IActionResult> GetFeeRequests()
    {
        var requests = await _context.FeeRequests
            .Include(f => f.Dependant)
            .Include(f => f.Dependant.Parent)
            .Include(f => f.Dependant.School)
            .ToListAsync();

        return Ok(requests);
    }

    [HttpPost("update-status")]
    public async Task<IActionResult> UpdateStatus(int feeRequestId, string status, string? message)
    {
        var fee = await _context.FeeRequests.FindAsync(feeRequestId);
        if (fee == null) return NotFound();

        if (status == "Approved") fee.Status = FeeStatus.Approved;
        else if (status == "Declined") fee.Status = FeeStatus.Declined;

        fee.ApprovalMessage = message;
        await _context.SaveChangesAsync();

        return Ok(fee);
    }
}
