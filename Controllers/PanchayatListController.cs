using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HHSurvey.Data;
using Microsoft.AspNetCore.Authorization;

namespace HHSurvey.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PanchayatListController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public PanchayatListController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/PanchayatList
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PanchayatList>>> GetPanchayatList()
        {
            return await _context.PanchayatList.ToListAsync();
        }

        // GET: api/PanchayatList/5
        [HttpGet("{blockCode}")]
        public async Task<ActionResult<PanchayatList>> GetPanchayatList(int blockCode)
        {
            var panchayatList = await _context.PanchayatList.Where(x => x.blockCode == blockCode).ToListAsync();

            if (panchayatList == null)
            {
                return NotFound();
            }

            return Ok(panchayatList);
        }

        // PUT: api/PanchayatList/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{panchayatCode}")]
        public async Task<IActionResult> PutPanchayatList(int panchayatCode, PanchayatList panchayatList)
        {
            if (panchayatCode != panchayatList.PanchayatCode)
            {
                return BadRequest();
            }

            _context.Entry(panchayatList).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PanchayatListExists(panchayatCode))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/PanchayatList
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<PanchayatList>> PostPanchayatList(PanchayatList panchayatList)
        {
            _context.PanchayatList.Add(panchayatList);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPanchayatList", new { id = panchayatList.PanchayatCode }, panchayatList);
        }

        // DELETE: api/PanchayatList/5
        [HttpDelete("{panchayatCode}")]
        public async Task<IActionResult> DeletePanchayatList(int panchayatCode)
        {
            var panchayatList = await _context.PanchayatList.FindAsync(panchayatCode);
            if (panchayatList == null)
            {
                return NotFound();
            }

            _context.PanchayatList.Remove(panchayatList);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PanchayatListExists(int panchayatCode)
        {
            return _context.PanchayatList.Any(e => e.PanchayatCode == panchayatCode);
        }
    }
}
