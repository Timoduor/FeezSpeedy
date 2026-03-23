// File: Controllers/DependantController.cs
using FeezSpeedy.Models;
using FeezSpeedy.Web.ViewModels;
using FeezSpeedy.Web.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FeezSpeedy.Web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace FeezSpeedy.Controllers
{
    [Authorize]
    [Route("[controller]")]
    public class DependantController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<Parent> _userManager;

        public DependantController(ApplicationDbContext db, UserManager<Parent> userManager)
        {
            _db = db;
            _userManager = userManager;
        }

        // ------------------- Razor -------------------

        // GET: /Dependant/Create
        [HttpGet("Create")]
        public async Task<IActionResult> Create()
        {
            // Populate schools for dropdown
            ViewBag.Schools = await _db.Schools.ToListAsync();
            var vm = new DependantViewModel();
            return View(vm);
        }

        // POST: /Dependant/Create
        [HttpPost("Create")]
        public async Task<IActionResult> Create(DependantViewModel vm, string? SchoolName)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Schools = await _db.Schools.ToListAsync();
                return View(vm);
            }

            // Get current logged-in parent
            var parent = await _userManager.GetUserAsync(User);
            if (parent == null)
                return Challenge();

            int schoolId = vm.SchoolId;

            // Handle manual school input
            if (!string.IsNullOrWhiteSpace(SchoolName))
            {
                var existingSchool = await _db.Schools.FirstOrDefaultAsync(s => s.Name.ToLower() == SchoolName.ToLower());
                if (existingSchool != null)
                {
                    schoolId = existingSchool.Id;
                }
                else
                {
                    var newSchool = new School
                    {
                        Name = SchoolName,
                        Location = "Unknown" // default placeholder
                    };
                    _db.Schools.Add(newSchool);
                    await _db.SaveChangesAsync();
                    schoolId = newSchool.Id;
                }
            }

            var dependant = new Dependant
            {
                FullName = vm.FullName,
                AdmissionNumber = vm.AdmissionNumber,
                ClassLevel = vm.ClassLevel,
                SchoolId = schoolId,
                ParentId = parent.Id
            };

            _db.Dependants.Add(dependant);
            await _db.SaveChangesAsync();

            TempData["Success"] = "Dependant added successfully!";
            return RedirectToAction("Index", "Dashboard");
        }

        // ------------------- API -------------------

        // GET: /api/dependants
        [HttpGet("/api/dependants")]
        public async Task<IActionResult> GetAll()
        {
            var parent = await _userManager.GetUserAsync(User);
            if (parent == null) return Unauthorized();

            var dependants = await _db.Dependants
                .Where(d => d.ParentId == parent.Id)
                .Include(d => d.School)
                .Select(d => new
                {
                    d.Id,
                    d.FullName,
                    d.AdmissionNumber,
                    d.ClassLevel,
                    School = d.School.Name
                })
                .ToListAsync();

            return Ok(dependants);
        }

        // POST: /api/dependants
        [HttpPost("/api/dependants")]
        public async Task<IActionResult> CreateApi([FromBody] DependantViewModel vm)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var parent = await _userManager.GetUserAsync(User);
            if (parent == null) return Unauthorized();

            int schoolId = vm.SchoolId;

            // Optional: you can allow API to create new schools if needed
            var schoolExists = await _db.Schools.AnyAsync(s => s.Id == schoolId);
            if (!schoolExists) return BadRequest("Invalid school ID.");

            var dependant = new Dependant
            {
                FullName = vm.FullName,
                AdmissionNumber = vm.AdmissionNumber,
                ClassLevel = vm.ClassLevel,
                SchoolId = schoolId,
                ParentId = parent.Id
            };

            _db.Dependants.Add(dependant);
            await _db.SaveChangesAsync();

            var school = await _db.Schools.FindAsync(dependant.SchoolId);

            return Ok(new
            {
                dependant.Id,
                dependant.FullName,
                dependant.AdmissionNumber,
                dependant.ClassLevel,
                School = school?.Name
            });
        }
    }
}