using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HHSurvey.Data;
using Microsoft.AspNetCore.Authorization;
using HHSurvey.Models;

namespace HHSurvey.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DashboardController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public DashboardController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/StateList
       
        [HttpGet("stat")]
        public async Task<IActionResult> GetDashboardStat()
        {
            string sql = @"select count(h1.uniqueId) as TotalHousehold, count(h2.UniqueId) as [Approved], count(h3.UniqueId) as [Pending],
count(h4.uniqueId) as TotalHouseholdFTM, count(h5.UniqueId) as [ApprovedFTM], count(h6.UniqueId) as [PendingFTM] 
FROM HouseholdBasicProfile h1 left join 
HouseholdBasicProfile h2 on h1.UniqueId  = h2.UniqueId and h2.sstatus = 1 left join
HouseholdBasicProfile h3 on h1.UniqueId  = h3.UniqueId and h3.sstatus = 0 left join
HouseholdBasicProfile h4 on h1.UniqueId = h4.uniqueId and YEAR(h4.entryDate) = YEAR(GETDATE()) AND MONTH(h4.entryDate) = MONTH(GETDATE()) left join 
HouseholdBasicProfile h5 on h4.UniqueId  = h5.UniqueId and h5.sstatus = 1 and YEAR(h4.entryDate) = YEAR(GETDATE()) AND MONTH(h4.entryDate) = MONTH(GETDATE()) left join
HouseholdBasicProfile h6 on h4.UniqueId  = h6.UniqueId and h6.sstatus = 0 and YEAR(h4.entryDate) = YEAR(GETDATE()) AND MONTH(h4.entryDate) = MONTH(GETDATE())";
            var data = await _context
        .Set<DashboardDTO>()
        .FromSqlRaw(sql)
        .AsNoTracking()
        .ToListAsync();
            return Ok(data);
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
