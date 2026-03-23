using FeezSpeedy.Web.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FeezSpeedy.Controllers.Api
{
    [ApiController]
    [Route("api/admin")]
    [Authorize(Roles = "Admin")]
    public class AdminApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public AdminApiController(ApplicationDbContext context) => _context = context;

        [HttpGet("fee-requests")]
        public async Task<IActionResult> FeeRequests()
        {
            var fees = await _context.FeeRequests
                .Include(f => f.Dependant)
                .ThenInclude(d => d.Parent)
                .Include(f => f.Dependant.School)
                .Include(f => f.LoanStatus)
                .Select(f => new
                {
                    id = f.Id,
                    dependantName = f.Dependant.FullName,
                    parentName = f.Dependant.Parent.FullName,
                    schoolName = f.Dependant.School.Name,
                    amount = f.Amount,
                    totalPayable = f.TotalPayable,
                    interestRate = f.InterestRate,
                    status = f.LoanStatus.Name
                }).ToListAsync();

            return Ok(fees);
        }

        [HttpGet("parents")]
        public async Task<IActionResult> Parents()
        {
            var parents = await _context.Parents
                .Select(p => new
                {
                    id = p.Id,
                    fullName = p.FullName,
                    email = p.Email,
                    phone = p.PhoneNumber
                }).ToListAsync();
            return Ok(parents);
        }

        [HttpGet("schools")]
        public async Task<IActionResult> Schools()
        {
            var schools = await _context.Schools
                .Select(s => new
                {
                    id = s.Id,
                    name = s.Name,
                    location = s.Location
                }).ToListAsync();
            return Ok(schools);
        }

        [HttpGet("disbursements")]
        public async Task<IActionResult> Disbursements()
        {
            var disbursed = await _context.FeeRequests
                .Where(f => f.IsDisbursed)
                .Include(f => f.Dependant)
                .ThenInclude(d => d.Parent)
                .Include(f => f.Dependant.School)
                .Select(f => new
                {
                    id = f.Id,
                    dependantName = f.Dependant.FullName,
                    parentName = f.Dependant.Parent.FullName,
                    schoolName = f.Dependant.School.Name,
                    amount = f.Amount,
                    paid = f.IsPaid,
                    disbursedAt = f.DisbursedAt
                }).ToListAsync();
            return Ok(disbursed);
        }
    }

}