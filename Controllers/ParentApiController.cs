using FeezSpeedy.Data;
using FeezSpeedy.Models;
using FeezSpeedy.Web.Data;
using FeezSpeedy.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FeezSpeedy.Controllers
{
    [ApiController]
    [Route("api/parent")]
    [Authorize(Roles = "Parent")]
    public class ParentApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<Parent> _userManager;

        public ParentApiController(ApplicationDbContext context, UserManager<Parent> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpGet("me")]
        public async Task<IActionResult> Me()
        {
            var user = await _userManager.GetUserAsync(User);
            return Ok(new { user.FullName, user.Email });
        }

        [HttpGet("fee-requests")]
        public async Task<IActionResult> MyRequests()
        {
            var user = await _userManager.GetUserAsync(User);

            var list = await _context.FeeRequests
                .Include(f => f.Dependant)
                .ThenInclude(d => d.School)
                .Where(f => f.Dependant.ParentId == user.Id)
                .Select(f => new
                {
                    id = f.Id,
                    dependant = f.Dependant.FullName,
                    school = f.Dependant.School.Name,
                    amount = f.Amount,
                    totalPayable = f.TotalPayable,
                    status = f.Status.ToString()
                }).ToListAsync();

            return Ok(list);
        }
    }
}