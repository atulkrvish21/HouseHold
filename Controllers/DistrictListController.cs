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
    public class DistrictListController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public DistrictListController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/DistrictList
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DistrictList>>> GetDistrictList()
        {
            return await _context.DistrictList.ToListAsync();
        }

        // GET: api/DistrictList/5
        [HttpGet("{stateCode}")]
        public async Task<ActionResult<DistrictList>> GetDistrictList(int stateCode)
        {
            var districtList = await _context.DistrictList.Where(x => x.stateCode == stateCode).ToListAsync();

            if (districtList == null)
            {
                return NotFound();
            }

            return Ok(districtList);
        }

        // PUT: api/DistrictList/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{districtCode}")]
        public async Task<IActionResult> PutDistrictList(int districtCode, DistrictList districtList)
        {
            if (districtCode != districtList.DistrictCode)
            {
                return BadRequest();
            }

            _context.Entry(districtList).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DistrictListExists(districtCode))
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

        // POST: api/DistrictList
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<DistrictList>> PostDistrictList(DistrictList districtList)
        {
            _context.DistrictList.Add(districtList);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDistrictList", new { districtCode = districtList.DistrictCode }, districtList);
        }

        // DELETE: api/DistrictList/5
        [HttpDelete("{districtCode}")]
        public async Task<IActionResult> DeleteDistrictList(int districtCode)
        {
            var districtList = await _context.DistrictList.FindAsync(districtCode);
            if (districtList == null)
            {
                return NotFound();
            }

            _context.DistrictList.Remove(districtList);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool DistrictListExists(int districtCode)
        {
            return _context.DistrictList.Any(e => e.DistrictCode == districtCode);
        }
    }
}
