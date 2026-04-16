using HHSurvey.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
namespace HHSurvey.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class MigrationSurveyController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public MigrationSurveyController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MigrationSurvey>>> GetAll()
        {
            return await _context.MigrationSurvey.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<MigrationSurvey>> Get(int id)
        {
            var survey = await _context.MigrationSurvey.FindAsync(id);
            if (survey == null) return NotFound();
            return survey;
        }

        [HttpPost]
        public async Task<ActionResult<MigrationSurvey>> Create(MigrationSurvey survey)
        {
            _context.MigrationSurvey.Add(survey);
            await _context.SaveChangesAsync();
            // return CreatedAtAction(nameof(Get), new { id = survey.Id }, survey);
 return CreatedAtAction(
                
                    nameof(Create),
                    new { id = survey.Id },
                    new { success = true, 
        message = "Migration survey saved successfully",id = survey.Id }
                );
            
        }

        [HttpPost("{id}")]
        public async Task<IActionResult> Update(int id, MigrationSurvey survey)
        {
            if (id != survey.Id) return BadRequest();
            _context.Entry(survey).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpPost("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var survey = await _context.MigrationSurvey.FindAsync(id);
            if (survey == null) return NotFound();
            _context.MigrationSurvey.Remove(survey);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}