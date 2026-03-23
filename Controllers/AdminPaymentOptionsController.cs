using FeezSpeedy.Models;
using FeezSpeedy.Web.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Authorize(Roles = "Admin")]
public class AdminPaymentOptionsController : Controller
{
    private readonly ApplicationDbContext _context;

    public AdminPaymentOptionsController(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        return View(await _context.PaymentOptions.ToListAsync());
    }

    [HttpPost]
    public async Task<IActionResult> Toggle(int id)
    {
        var option = await _context.PaymentOptions.FindAsync(id);
        if (option == null) return NotFound();

        option.IsActive = !option.IsActive;
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }
}
