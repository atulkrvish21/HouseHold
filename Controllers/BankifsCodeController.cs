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
    public class BankifsCodeController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public BankifsCodeController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/StateList
       
        [HttpGet]
        public async Task<ActionResult<IEnumerable<bankIFSCode>>> GetBankifsCode()
        {
            return await _context.bankIFSCode.ToListAsync();
        }

        // GET: api/StateList/5
        [HttpGet("name/{bankName}")]
        public async Task<ActionResult<IEnumerable<bankIFSCode>>> GetBankifsCodeByName(string bankName)
        {
            var bankList = await _context.bankIFSCode.Where(b => b.BankName == bankName).OrderBy(x => x.BranchName).ToListAsync();

            if (bankList == null)
            {
                return NotFound();
            }

            return bankList;
        }

        [HttpGet("ifsc/{ifscCode}")]
        public async Task<ActionResult<IEnumerable<bankIFSCode>>> GetBranchIfscCode(string ifscCode)
        {
            var bankList = await _context.bankIFSCode.Where(b => b.ifsCode == ifscCode).OrderBy(x => x.ifsCode).ToListAsync();

            if (bankList == null)
            {
                return NotFound();
            }

            return bankList;
        }

        // PUT: api/StateList/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    }
}
