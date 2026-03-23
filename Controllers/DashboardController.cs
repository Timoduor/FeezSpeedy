using FeezSpeedy.Models;
using FeezSpeedy.Web.Data;
using FeezSpeedy.Web.Models;
using FeezSpeedy.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FeezSpeedy.Web.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<Parent> _userManager;

        public DashboardController(ApplicationDbContext context, UserManager<Parent> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // =========================
        // PARENT DASHBOARD
        // =========================
        public async Task<IActionResult> Index()
        {
            var parent = await _userManager.GetUserAsync(User);
            if (parent == null) return Challenge();

            var dependants = await _context.Dependants
                .Include(d => d.School)
                .Where(d => d.ParentId == parent.Id)
                .ToListAsync();

            var feeRequests = await _context.FeeRequests
                .Include(f => f.Dependant)
                .Where(f => f.Dependant.ParentId == parent.Id)
                .ToListAsync();

            var outstanding = feeRequests
                .Where(f => !f.IsPaid)
                .Sum(f => f.TotalPayable);

            var vm = new DashboardViewModel
            {
                Dependants = dependants,
                DependantsCount = dependants.Count,
                FeeRequests = feeRequests,
                FeeRequestsCount = feeRequests.Count,
                OutstandingBalance = outstanding
            };

            return View(vm);
        }

        // =========================
        // ADD DEPENDANT
        // =========================
        public async Task<IActionResult> AddDependant()
        {
            ViewBag.Schools = await _context.Schools.ToListAsync();
            return View(new DependantViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddDependant(DependantViewModel vm, string? SchoolName)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Schools = await _context.Schools.ToListAsync();
                return View(vm);
            }

            var parent = await _userManager.GetUserAsync(User);
            if (parent == null) return Challenge();

            int schoolId;

            if (!string.IsNullOrWhiteSpace(SchoolName))
            {
                var school = await _context.Schools
                    .FirstOrDefaultAsync(s => s.Name.ToLower() == SchoolName.ToLower());

                if (school == null)
                {
                    school = new School
                    {
                        Name = SchoolName,
                        Location = "Unknown"
                    };

                    _context.Schools.Add(school);
                    await _context.SaveChangesAsync();
                }

                schoolId = school.Id;
            }
            else
            {
                schoolId = vm.SchoolId;
            }

            var dependant = new Dependant
            {
                FullName = vm.FullName,
                AdmissionNumber = vm.AdmissionNumber,
                ClassLevel = vm.ClassLevel,
                SchoolId = schoolId,
                ParentId = parent.Id
            };

            _context.Dependants.Add(dependant);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Dependant added successfully!";
            return RedirectToAction(nameof(Index));
        }

        // =========================
        // APPLY FOR SCHOOL FEE LOAN
        // =========================
        public IActionResult ApplyFee(int dependantId)
        {
            var vm = new FeeRequestViewModel
            {
                DependantId = dependantId
            };

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ApplyFee(FeeRequestViewModel vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            var parent = await _userManager.GetUserAsync(User);
            if (parent == null) return Challenge();

            var totalPayable = vm.Amount + (vm.Amount * vm.InterestRate / 100);

            var feeRequest = new FeeRequest
            {
                DependantId = vm.DependantId,
                Amount = vm.Amount,
                InterestRate = vm.InterestRate,
                TotalPayable = totalPayable,
                DurationMonths = vm.DurationMonths,
                Status = FeeStatus.Pending,
                CreatedAt = DateTime.UtcNow,
                ParentId = parent.Id,
                LoanStatusId = 1
            };

            _context.FeeRequests.Add(feeRequest);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Fee request submitted successfully!";
            return RedirectToAction(nameof(Index));
        }

        // =========================
        // ENTERPRISE BATCH LOAN PREVIEW
        // =========================
        [HttpGet]
        public async Task<IActionResult> GetLoanPreviews()
        {
            var parent = await _userManager.GetUserAsync(User);
            if (parent == null) return Unauthorized();

            var previews = await _context.FeeRequests
                .Include(f => f.Dependant)
                .Where(f => f.ParentId == parent.Id)
                .GroupBy(f => f.DependantId)
                .Select(g => g.OrderByDescending(f => f.CreatedAt).First())
                .Select(f => new
                {
                    dependantId = f.DependantId,

                    totalPayable =
                        f.Amount +
                        (f.Amount * (f.InterestRate / 100) * (f.DurationMonths / 12m)),

                    monthlyRepayment =
                        (f.Amount +
                        (f.Amount * (f.InterestRate / 100) * (f.DurationMonths / 12m)))
                        / f.DurationMonths,

                    payoffDate =
                        DateTime.Today
                        .AddMonths(f.DurationMonths)
                        .ToString("MMMM yyyy")
                })
                .ToListAsync();

            return Ok(previews);
        }

        // =========================
        // ADMIN APPROVAL PANEL
        // =========================
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ApproveFeeRequests()
        {
            var requests = await _context.FeeRequests
                .Include(f => f.Dependant)
                .ThenInclude(d => d.Parent)
                .Where(f => f.Status == FeeStatus.Pending)
                .ToListAsync();

            return View(requests);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ChangeFeeStatus(int id, FeeStatus status)
        {
            var request = await _context.FeeRequests.FindAsync(id);

            if (request == null)
                return NotFound();

            request.Status = status;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(ApproveFeeRequests));
        }
    }
}