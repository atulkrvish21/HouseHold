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
    public class BlockListController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public BlockListController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/BlockList
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BlockList>>> GetBlockList()
        {
            return await _context.BlockList.ToListAsync();
        }

        // GET: api/BlockList/5
        [HttpGet("{districtCode}")]
        public async Task<ActionResult<BlockList>> GetBlockList(int districtCode)
        {
            var blockList = await _context.BlockList.Where(x => x.DistrictCode == districtCode).ToListAsync();

            if (blockList == null)
            {
                return NotFound();
            }

            return Ok(blockList);
        }

        // PUT: api/BlockList/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{districtCode}")]
        public async Task<IActionResult> PutBlockList(int blockCode, BlockList blockList)
        {
            if (blockCode != blockList.BlockCode)
            {
                return BadRequest();
            }

            _context.Entry(blockList).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BlockListExists(blockCode))
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

        // POST: api/BlockList
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<BlockList>> PostBlockList(BlockList blockList)
        {
            _context.BlockList.Add(blockList);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBlockList", new { id = blockList.BlockCode }, blockList);
        }

        // DELETE: api/BlockList/5
        [HttpDelete("{blockCode}")]
        public async Task<IActionResult> DeleteBlockList(int blockCode)
        {
            var blockList = await _context.BlockList.FindAsync(blockCode);
            if (blockList == null)
            {
                return NotFound();
            }

            _context.BlockList.Remove(blockList);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool BlockListExists(int blockCode)
        {
            return _context.BlockList.Any(e => e.BlockCode == blockCode);
        }
    }
}
