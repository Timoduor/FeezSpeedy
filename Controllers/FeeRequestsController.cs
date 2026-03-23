using FeezSpeedy.Models;
using FeezSpeedy.Web.Data;
using FeezSpeedy.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace FeezSpeedy.Controllers
{
    [Route("[controller]")]
    public class FeeRequestController : Controller
    {
        private readonly ApplicationDbContext _db;

        public FeeRequestController(ApplicationDbContext db)
        {
            _db = db;
        }

        // GET: /FeeRequest/Create?dependantId=1
        [HttpGet("Create")]
        public IActionResult Create(int dependantId)
        {
            var vm = new FeeRequestViewModel
            {
                DependantId = dependantId
            };

            return View(vm);
        }

        // POST: /FeeRequest/Create
        [HttpPost("Create")]
        public async Task<IActionResult> Create(FeeRequestViewModel vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            var parentId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var feeRequest = new FeeRequest
            {
                ParentId = parentId,
                DependantId = vm.DependantId,
                Amount = vm.Amount,
                InterestRate = vm.InterestRate,
                DurationMonths = vm.DurationMonths,
                TotalPayable = vm.Amount + (vm.Amount * vm.InterestRate / 100),
                Status = FeeStatus.Pending,
                CreatedAt = DateTime.UtcNow,
                LoanStatusId = 1
            };

            _db.FeeRequests.Add(feeRequest);
            await _db.SaveChangesAsync();

            TempData["Success"] = "Fee request submitted successfully";

            return RedirectToAction("Index", "Dashboard");
        }

        // GET: /api/feerequests
        [HttpGet("/api/feerequests")]
        public async Task<IActionResult> GetAll()
        {
            var parentId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (parentId == null) return Unauthorized();

            var requests = await _db.FeeRequests
                .Where(f => f.ParentId == parentId)
                .Include(f => f.Dependant)
                .ThenInclude(d => d.School)
                .Select(f => new
                {
                    id = f.Id,
                    dependantId = f.DependantId,
                    dependantName = f.Dependant.FullName,
                    schoolName = f.Dependant.School.Name,
                    amount = f.Amount,
                    totalPayable = f.TotalPayable,
                    status = f.Status.ToString(),
                    isPaid = f.IsPaid,
                    createdAt = f.CreatedAt
                })
                .ToListAsync();

            return Ok(requests);
        }
        

        // API: POST /api/feerequests
        [HttpPost("/api/feerequests")]
        public async Task<IActionResult> CreateApi([FromBody] FeeRequestViewModel vm)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var parentId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var feeRequest = new FeeRequest
            {
                ParentId = parentId,
                DependantId = vm.DependantId,
                Amount = vm.Amount,
                InterestRate = vm.InterestRate,
                DurationMonths = vm.DurationMonths,
                TotalPayable = vm.Amount + (vm.Amount * vm.InterestRate / 100),
                Status = FeeStatus.Pending,
                CreatedAt = DateTime.UtcNow,
                LoanStatusId = 1
            };

            _db.FeeRequests.Add(feeRequest);
            await _db.SaveChangesAsync();

            return Ok(new
            {
                id = feeRequest.Id,
                dependantId = feeRequest.DependantId,
                amount = feeRequest.Amount,
                totalPayable = feeRequest.TotalPayable,
                status = feeRequest.Status,
                createdAt = feeRequest.CreatedAt
            });
        }
    }
}