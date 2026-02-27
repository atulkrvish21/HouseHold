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
    public class BankListController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public BankListController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/StateList
       
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BankList>>> GetStateList()
        {
            return await _context.BankList.ToListAsync();
        }

        // GET: api/StateList/5
        [HttpGet("{id}")]
        public async Task<ActionResult<BankList>> GetBankList(int id)
        {
            var bankList = await _context.BankList.FindAsync(id);

            if (bankList == null)
            {
                return NotFound();
            }

            return bankList;
        }

        // PUT: api/StateList/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBankList(int id, BankList bankList)
        {
            if (id != bankList.Id)
            {
                return BadRequest();
            }

            _context.Entry(bankList).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BankListExists(id))
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
        public async Task<ActionResult<BankList>> PostBankList(BankList bankList)
        {
            _context.BankList.Add(bankList);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetStateList", new { id = bankList.Id }, bankList);
        }

        // DELETE: api/StateList/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStateList(int id)
        {
            var bankList = await _context.BankList.FindAsync(id);
            if (bankList == null)
            {
                return NotFound();
            }

            _context.BankList.Remove(bankList);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool BankListExists(int id)
        {
            return _context.BankList.Any(e => e.Id == id);
        }
    }
}
