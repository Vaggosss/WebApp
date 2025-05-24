using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApp.Data;
using WebApp.Models;

namespace WebApp.Controllers
{
    [ApiController]
    [Route("api/admin")]
    public class AdminController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AdminController(AppDbContext context)
        {
            _context = context;
        }

        // Μέθοδος για απλή έλεγχο API key
        private bool IsAuthorized()
        {
            var apiKey = HttpContext.Request.Headers["X-Api-Key"].ToString();
            return _context.Admins.Any(a => a.ApiKey == apiKey);
        }


        [HttpPost("jobs")]
        public async Task<IActionResult> CreateJob(Job job)
        {
            if (!IsAuthorized())
                return Unauthorized();

            job.PostedAt = DateTime.UtcNow;
            _context.Jobs.Add(job);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(UpdateJob), new { id = job.Id }, job);
        }

        [HttpPut("jobs/{id}")]
        public async Task<IActionResult> UpdateJob(int id, Job updatedJob)
        {
            if (!IsAuthorized())
                return Unauthorized();

            if (id != updatedJob.Id)
                return BadRequest("Job ID mismatch.");

            var existingJob = await _context.Jobs.FindAsync(id);
            if (existingJob == null)
                return NotFound();

            existingJob.Title = updatedJob.Title;
            existingJob.Description = updatedJob.Description;
            existingJob.Location = updatedJob.Location;
            existingJob.IsFilled = updatedJob.IsFilled;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("jobs/{id}")]
        public async Task<IActionResult> DeleteJob(int id)
        {
            if (!IsAuthorized())
                return Unauthorized();

            var job = await _context.Jobs.FindAsync(id);
            if (job == null)
                return NotFound();

            _context.Jobs.Remove(job);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPost("jobs/{id}/fill")]
        public async Task<IActionResult> MarkJobAsFilled(int id)
        {
            if (!IsAuthorized())
                return Unauthorized();

            var job = await _context.Jobs.FindAsync(id);
            if (job == null)
                return NotFound();

            job.IsFilled = true;
            await _context.SaveChangesAsync();

            return Ok(job);
        }

        [HttpPost("companies")]
        public async Task<IActionResult> CreateCompany(Company company)
        {
            if (!IsAuthorized())
                return Unauthorized();

            _context.Companies.Add(company);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCompanyById), new { id = company.Id }, company);
        }

        [HttpGet("companies/{id}")]
        public async Task<ActionResult<Company>> GetCompanyById(int id)
        {
            var company = await _context.Companies.Include(c => c.Jobs).FirstOrDefaultAsync(c => c.Id == id);
            if (company == null)
                return NotFound();

            return company;
        }


    }
}
