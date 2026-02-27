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
    public class VillageListController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public VillageListController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/PanchayatList
        [HttpGet]
        public async Task<ActionResult<IEnumerable<VillageList>>> GetVillageList()
        {
            return await _context.VillageList.ToListAsync();
        }

        // GET: api/PanchayatList/5
        [HttpGet("{panchayatCode}")]
        public async Task<ActionResult<VillageList>> GetVillageList(long panchayatCode)
        {
            var villageList = await _context.VillageList.Where(x => x.PanchayatCode == panchayatCode).ToListAsync();

            if (villageList == null)
            {
                return NotFound();
            }

            return Ok(villageList);
        }

        // PUT: api/PanchayatList/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{villageCode}")]
        public async Task<IActionResult> PutVillageList(int villageCode, VillageList villageList)
        {
            if (villageCode != villageList.villageCode)
            {
                return BadRequest();
            }

            _context.Entry(villageList).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VillageListExists(villageCode))
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
        public async Task<ActionResult<VillageList>> PostPanchayatList(VillageList villageList)
        {
            _context.VillageList.Add(villageList);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetVillageList", new { villageCode = villageList.villageCode }, villageList);
        }

        // DELETE: api/PanchayatList/5
        [HttpDelete("{villageCode}")]
        public async Task<IActionResult> DeleteVillageList(int villageCode)
        {
            var villageList = await _context.VillageList.FindAsync(villageCode);
            if (villageList == null)
            {
                return NotFound();
            }

            _context.VillageList.Remove(villageList);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool VillageListExists(int villageCode)
        {
            return _context.VillageList.Any(e => e.villageCode == villageCode);
        }
    }
}
