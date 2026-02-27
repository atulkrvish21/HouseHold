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
using System.Security.Claims;
using System.Text.Json;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Data.SqlClient;
using System.Text.Json.Serialization;

namespace HHSurvey.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ReportsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly HouseholdValidator _householdValidator;

        public ReportsController(ApplicationDbContext context, HouseholdValidator householdValidator)
        {
            _context = context;
            _householdValidator = householdValidator;
        }

   [HttpGet("survey-progress-panchayat")]
public async Task<IActionResult> GetPanchayatLevelProgress()
{
    try
    {
        // 1. Fetch Master List (To ensure we show GPs with 0 surveys)
        var masterData = await _context.PanchayatList
            .Select(p => new
            {
                PanchayatCode = p.PanchayatCode,
                PanchayatName = p.PanchayatName,
                BlockCode = p.blockCode,
                TotalVillages = _context.VillageList.Count(v => v.PanchayatCode == p.PanchayatCode)
            })
            .ToListAsync();

        // 2. Fetch Block/District Maps (Fast In-Memory Lookup)
        var blocks = await _context.BlockList.ToDictionaryAsync(x => x.BlockCode, x => new { x.BlockName, x.DistrictCode });
        var districts = await _context.DistrictList.ToDictionaryAsync(x => x.DistrictCode, x => x.DistrictName);

        // 3. Fetch Actual Survey Counts
        var surveyData = await _context.HouseholdBasicProfile
            .GroupBy(h => h.GramPanchayat)
            .Select(g => new 
            {
                PanchayatCode = g.Key,
                Completed = g.Count(x => x.sstatus == 1),
                Pending = g.Count(x => x.sstatus == 0),
                Rejected = g.Count(x => x.sstatus == 10)
            })
            .ToListAsync();

        // 4. Merge & Calculate Totals
        var report = masterData.Select(m => 
        {
            var blockName = "Unknown";
            var districtName = "Unknown";
            
            if (blocks.TryGetValue(m.BlockCode, out var block))
            {
                blockName = block.BlockName;
                districts.TryGetValue(block.DistrictCode, out districtName);
            }

            var stats = surveyData.FirstOrDefault(s => s.PanchayatCode == m.PanchayatCode.ToString());
            
            int completed = stats?.Completed ?? 0;
            int pending = stats?.Pending ?? 0;
            int rejected = stats?.Rejected ?? 0;

            return new PanchayatProgressDto
            {
                District = districtName,
                Block = blockName,
                Panchayat = m.PanchayatName,
                PanchayatCode = m.PanchayatCode,
                TotalVillages = m.TotalVillages,
                
                // ✅ NEW LOGIC: Total = Sum of all statuses
                TotalSubmitted = completed + pending + rejected,
                
                Completed = completed,
                Pending = pending,
                Rejected = rejected
            };
        })
        .OrderByDescending(x => x.TotalSubmitted) // Show highest activity first
        .ToList();

        return Ok(report);
    }
    catch (Exception ex)
    {
        return StatusCode(500, new { error = ex.Message });
    }
}
[HttpGet("entitlement-gap")]
public async Task<IActionResult> GetEntitlementGapReport()
{
    // 1. Fetch Hierarchy (District -> Block -> GP)
    var masterData = await _context.PanchayatList
        .Select(p => new
        {
            PanchayatCode = p.PanchayatCode,
            PanchayatName = p.PanchayatName,
            BlockCode = p.blockCode
        })
        .ToListAsync();

    var blocks = await _context.BlockList.ToDictionaryAsync(x => x.BlockCode, x => new { x.BlockName, x.DistrictCode });
    var districts = await _context.DistrictList.ToDictionaryAsync(x => x.DistrictCode, x => x.DistrictName);

    // 2. Fetch & Join Survey Data Manually
    // We join 'BasicProfile' with 'Entitlement' on UniqueId
    var surveyData = await (from h in _context.HouseholdBasicProfile
                            join e in _context.HouseholdEntitlement on h.UniqueId equals e.UniqueId into entGroup
                            from ent in entGroup.DefaultIfEmpty()// Left Join logic
                            where h.sstatus != 10
                            select new
                            {
                                h.GramPanchayat, // Grouping Key
                                
                                // Criteria Flags
                                NoRation = !h.HasRationCard ? 1 : 0,
                                WomenExcluded = h.IsWomenCoveredUnderSHG == true ? 0 : 1,

                                // Access Entitlement Table (Handle nulls just in case)
                                NoHousing = (ent == null || !ent.HasRuralHousingSchemeHouse) ? 1 : 0,
                                NoToilet = (ent == null || !ent.HasIndividualHouseholdLatrine) ? 1 : 0
                            })
                            .ToListAsync(); // Execute query and bring to memory

    // 3. Group In-Memory (Easier/Faster for complex aggregates after join)
    var groupedSurvey = surveyData
        .GroupBy(x => x.GramPanchayat)
        .Select(g => new
        {
            PanchayatCode = g.Key,
            TotalHH = g.Count(),
            NoRationCount = g.Sum(x => x.NoRation),
            NoHousingCount = g.Sum(x => x.NoHousing),
            NoToiletCount = g.Sum(x => x.NoToilet),
            WomenExcludedCount = g.Sum(x => x.WomenExcluded)
        })
        .ToList();

    // 4. Merge Master & Survey Data
    var report = masterData.Select(m =>
    {
        var blockName = "Unknown";
        var districtName = "Unknown";

        if (blocks.TryGetValue(m.BlockCode, out var block))
        {
            blockName = block.BlockName;
            districts.TryGetValue(block.DistrictCode, out districtName);
        }

        var stats = groupedSurvey.FirstOrDefault(s => s.PanchayatCode == m.PanchayatCode.ToString());

        return new EntitlementGapDto
        {
            District = districtName,
            Block = blockName,
            Panchayat = m.PanchayatName,

            TotalHouseholds = stats?.TotalHH ?? 0,
            NoRationCard = stats?.NoRationCount ?? 0,
            NoHousing = stats?.NoHousingCount ?? 0,
            NoToilet = stats?.NoToiletCount ?? 0,
            WomenNotInSHG = stats?.WomenExcludedCount ?? 0
        };
    })
    .Where(x => x.TotalHouseholds > 0)
    .OrderByDescending(x => x.NoRationCard)
    .ToList();

    return Ok(report);
}

[HttpGet("dashboard-charts")]
public async Task<IActionResult> GetDashboardCharts()
{
    // 1. Fetch raw data needed for aggregation
    // We only select the specific columns to keep it fast
    var data = await _context.HouseholdBasicProfile
        .Where(h => h.sstatus != 10) // Exclude Rejected
        .Select(h => new 
        { 
            h.SocialCategory,
            // Assume Housing status is joined or flat. 
            // Using a simple check here for demonstration.
            HasHousing = _context.HouseholdEntitlement
                        .Any(e => e.UniqueId == h.UniqueId && e.HasRuralHousingSchemeHouse)
        })
        .ToListAsync();

    // 2. Aggregate in Memory
    var stats = new ChartDataDto
    {
        CountSC = data.Count(x => x.SocialCategory == "SC"),
        CountST = data.Count(x => x.SocialCategory == "ST"),
        CountOBC = data.Count(x => x.SocialCategory == "OBC"),
        CountGen = data.Count(x => x.SocialCategory == "General"),

        HasHousing = data.Count(x => x.HasHousing),
        NoHousing = data.Count(x => !x.HasHousing)
    };

    return Ok(stats);
}
    }


    
    
}
