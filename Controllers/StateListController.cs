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
    public class StateListController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public StateListController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/StateList
       
        [HttpGet]
        public async Task<ActionResult<IEnumerable<StateList>>> GetStateList()
        {
            return await _context.StateList.ToListAsync();
        }

        // GET: api/StateList/5
        [HttpGet("{stateCode}")]
        public async Task<ActionResult<StateList>> GetStateByCode(int stateCode)
        {
            var stateList = await _context.StateList.FindAsync(stateCode);

            if (stateList == null)
            {
                return NotFound();
            }

           return Ok(stateList);
        }

        // PUT: api/StateList/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{stateCode}")]
        public async Task<IActionResult> PutStateList(int stateCode, StateList stateList)
        {
            if (stateCode != stateList.StateCode)
            {
                return BadRequest();
            }

            _context.Entry(stateList).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StateListExists(stateCode))
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

        // POST: api/StateList
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<StateList>> PostStateList(StateList stateList)
        {
            _context.StateList.Add(stateList);
            await _context.SaveChangesAsync();

           return CreatedAtAction(
        nameof(GetStateByCode),
        new { stateCode = stateList.StateCode },
        stateList
    );
        }

        // DELETE: api/StateList/5
        [HttpDelete("{stateCode}")]
        public async Task<IActionResult> DeleteStateList(int stateCode)
        {
            var stateList = await _context.StateList.FindAsync(stateCode);
            if (stateList == null)
            {
                return NotFound();
            }

            _context.StateList.Remove(stateList);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool StateListExists(int stateCode)
        {
            return _context.StateList.Any(e => e.StateCode == stateCode);
        }
    }
}
