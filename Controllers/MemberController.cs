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
    public class MemberController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public MemberController(ApplicationDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GenderCountDto>>> GetAll()
        {
            var query = from dl in _context.DistrictList
                        join bl in _context.BlockList on dl.DistrictCode equals bl.DistrictCode
                        join pl in _context.PanchayatList on bl.BlockCode equals pl.blockCode
                        join vl in _context.VillageList on pl.PanchayatCode equals vl.PanchayatCode

                        // Left Join HouseholdBasicProfile on multiple string fields
                        join hbp in _context.HouseholdBasicProfile
                            on new { D = dl.DistrictName, B = bl.BlockName, P = pl.PanchayatName, V = vl.VillageName }
                            equals new { D = hbp.District, B = hbp.Block, P = hbp.GramPanchayat, V = hbp.RevenueVillage } into hbpGroup
                        from hbp in hbpGroup.DefaultIfEmpty()

                            // Left Join HouseholdFamilyMember
                        join hfm in _context.HouseholdFamilyMember on hbp.UniqueId equals hfm.UniqueId into hfmGroup
                        from hfm in hfmGroup.DefaultIfEmpty()

                        group hfm by new { dl.DistrictName, bl.BlockName, pl.PanchayatName, vl.VillageName } into g
                        orderby g.Key.DistrictName, g.Key.BlockName, g.Key.PanchayatName, g.Key.VillageName
                        select new
                        {
                            g.Key.DistrictName,
                            g.Key.BlockName,
                            g.Key.PanchayatName,
                            g.Key.VillageName,
                            TotalMale = g.Count(x => x.Gender == "Male"),
                            TotalFemale = g.Count(x => x.Gender == "Female"),
                            Total = g.Count(x => x.Gender != null)
                        };

            var result = await query.ToListAsync();
            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        [HttpGet("filter")]
        public async Task<ActionResult<IEnumerable<GenderCountDto>>> GetFiltered(
    string? districtName = null,
    string? blockName = null,
    string? panchayatName = null,
    string? villageName = null)
        {
            var query = from dl in _context.DistrictList
                        join bl in _context.BlockList on dl.DistrictCode equals bl.DistrictCode
                        join pl in _context.PanchayatList on bl.BlockCode equals pl.blockCode
                        join vl in _context.VillageList on pl.PanchayatCode equals vl.PanchayatCode

                        join hbp in _context.HouseholdBasicProfile
                            on new { D = dl.DistrictName, B = bl.BlockName, P = pl.PanchayatName, V = vl.VillageName }
                            equals new { D = hbp.District, B = hbp.Block, P = hbp.GramPanchayat, V = hbp.RevenueVillage } into hbpGroup
                        from hbp in hbpGroup.DefaultIfEmpty()

                        join hfm in _context.HouseholdFamilyMember on hbp.UniqueId equals hfm.UniqueId into hfmGroup
                        from hfm in hfmGroup.DefaultIfEmpty()
                        select new { dl, bl, pl, vl, hfm };

            // Dynamic Filtering
            if (!string.IsNullOrEmpty(districtName))
            {
                query = query.Where(x => x.dl.DistrictName == districtName);

                if (!string.IsNullOrEmpty(blockName))
                {
                    query = query.Where(x => x.bl.BlockName == blockName);

                    if (!string.IsNullOrEmpty(panchayatName))
                    {
                        query = query.Where(x => x.pl.PanchayatName == panchayatName);

                        if (!string.IsNullOrEmpty(villageName))
                            query = query.Where(x => x.vl.VillageName == villageName);
                    }
                }
            }
            var result = await query
                .GroupBy(x => new { x.dl.DistrictName, x.bl.BlockName, x.pl.PanchayatName, x.vl.VillageName })
                .OrderBy(g => g.Key.DistrictName).ThenBy(g => g.Key.BlockName)
                .Select(g => new GenderCountDto
                {
                    DistrictName = g.Key.DistrictName,
                    BlockName = g.Key.BlockName,
                    PanchayatName = g.Key.PanchayatName,
                    VillageName = g.Key.VillageName,
                    Male = g.Count(x => x.hfm.Gender == "Male" && x.hfm.Age > 18 && x.hfm.MigratedInLast3Years == true), 
                    Female = g.Count(x => x.hfm.Gender == "Female" && x.hfm.Age >= 18 && x.hfm.MigratedInLast3Years == true), 
                    Minor = g.Count(x => x.hfm.Age < 18 && x.hfm.MigratedInLast3Years == true),
                    Total = g.Count(x => x.hfm.Gender != null)
                })
                .ToListAsync();

            return Ok(result);
        }
    }
}
