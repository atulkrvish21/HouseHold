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
    public class HouseholdController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly HouseholdValidator _householdValidator;

        public HouseholdController(ApplicationDbContext context, HouseholdValidator householdValidator)
        {
            _context = context;
            _householdValidator = householdValidator;
        }

        // GET: api/StateList

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Household>>> GetHousehold()
        {
            var username = User.FindFirst(ClaimTypes.Name)?.Value;
            var roleId = User.FindFirst(ClaimTypes.Role)?.Value;
            var basicProfiles = new List<HouseholdBasicProfile>();

            var entitlements = new List<HouseholdEntitlement>();
            var migrations = new List<HouseholdMigrationStatus>();
            var occupations = new List<HouseholdOccupationAndLand>();
            var familyMembers = new List<HouseholdFamilyMember>();
            if (roleId == "2")
            {
                basicProfiles = await _context.HouseholdBasicProfile.ToListAsync();
                entitlements = await _context.HouseholdEntitlement.ToListAsync();
                migrations = await _context.HouseholdMigrationStatus.ToListAsync();
                occupations = await _context.HouseholdOccupationAndLand.ToListAsync();
                familyMembers = await _context.HouseholdFamilyMember.ToListAsync();
            }
            else
            {
                basicProfiles = await _context.HouseholdBasicProfile.Where(x => x.entryBy == username).ToListAsync();
                entitlements = await _context.HouseholdEntitlement.ToListAsync();
                migrations = await _context.HouseholdMigrationStatus.ToListAsync();
                occupations = await _context.HouseholdOccupationAndLand.ToListAsync();
                familyMembers = await _context.HouseholdFamilyMember.ToListAsync();
            }
            var households = basicProfiles.Select(bp => new Household
            {
                HouseholdBasicProfile = bp,

                HouseholdEntitlement = entitlements
             .FirstOrDefault(e => e.UniqueId == bp.UniqueId),

                HouseholdMigrationStatus = migrations
             .FirstOrDefault(m => m.UniqueId == bp.UniqueId),

                HouseholdOccupationAndLand = occupations
             .FirstOrDefault(o => o.UniqueId == bp.UniqueId),

                HouseholdFamilyMember = familyMembers
             .Where(f => f.UniqueId == bp.UniqueId)
             .ToList()
            }).OrderByDescending(x => x.HouseholdBasicProfile?.EntryDate)
            .ToList();

            return Ok(households);
        }

        // GET: api/StateList/5
        [HttpGet("{uniqueid}")]
        public async Task<ActionResult<Household>> GetHousehold(string uniqueid)
        {
            var basicProfile = await _context.HouseholdBasicProfile
      .FirstOrDefaultAsync(x => x.UniqueId == uniqueid);

            if (basicProfile == null)
                return NotFound("Household not found");

            var household = new Household
            {
                HouseholdBasicProfile = basicProfile,

                HouseholdEntitlement = await _context.HouseholdEntitlement
                    .FirstOrDefaultAsync(x => x.UniqueId == uniqueid),

                HouseholdMigrationStatus = await _context.HouseholdMigrationStatus
                    .FirstOrDefaultAsync(x => x.UniqueId == uniqueid),

                HouseholdOccupationAndLand = await _context.HouseholdOccupationAndLand
                    .FirstOrDefaultAsync(x => x.UniqueId == uniqueid),

                HouseholdFamilyMember = await _context.HouseholdFamilyMember
                    .Where(x => x.UniqueId == uniqueid)
                    .ToListAsync()
            };

            return household;
        }

        [HttpPost("search-approval")]
        [Authorize]
        public async Task<IActionResult> SearchHouseholdsApproval([FromBody] HouseholdSearchFilter filter)
        {
           // Console.WriteLine("******************* CREATE BY ATUL ******************");
            var username = User.Identity?.Name;
            var roleId = User.FindFirst(ClaimTypes.Role)?.Value;

            var from = filter.FromDate.Date;
            var to = filter.ToDate.Date.AddDays(1);

            IQueryable<HouseholdBasicProfile> query =
                _context.HouseholdBasicProfile
                .Where(x => x.EntryDate >= from && x.EntryDate < to);

            // 🔹 Role based filtering
            if (roleId != "2")
            {
                query = query.Where(x => x.entryBy == username);
            }
            else
            {
                if (!string.IsNullOrEmpty(filter.EntryBy))
                    query = query.Where(x => x.entryBy == filter.EntryBy);
            }

            // 🔹 Dynamic filters
            if (!string.IsNullOrEmpty(filter.District))
                query = query.Where(x => x.District == filter.District);

            if (!string.IsNullOrEmpty(filter.Block))
                query = query.Where(x => x.Block == filter.Block);

            if (!string.IsNullOrEmpty(filter.GramPanchayat))
                query = query.Where(x => x.GramPanchayat == filter.GramPanchayat);

            if (!string.IsNullOrEmpty(filter.RevenueVillage))
                query = query.Where(x => x.RevenueVillage == filter.RevenueVillage);

            if (!string.IsNullOrEmpty(filter.SocialCategory))
                query = query.Where(x => x.SocialCategory == filter.SocialCategory);

            if (filter.HasRationCard.HasValue)
                query = query.Where(x => x.HasRationCard == filter.HasRationCard);

            query = query.Where(x => x.sstatus == 0);
            // 🔹 Total count (before pagination)
            var totalRecords = await query.CountAsync();

            // 🔹 Pagination
            var pageNumber = filter.PageNumber < 1 ? 1 : filter.PageNumber;
            var pageSize = filter.PageSize < 1 ? 10 : filter.PageSize;

            var basicProfiles = await query
                .OrderByDescending(x => x.EntryDate)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var uniqueIds = basicProfiles.Select(x => x.UniqueId).ToList();

            // 🔹 Related data
            var entitlements = await _context.HouseholdEntitlement
                .Where(x => uniqueIds.Contains(x.UniqueId))
                .ToListAsync();

            var migrations = await _context.HouseholdMigrationStatus
                .Where(x => uniqueIds.Contains(x.UniqueId))
                .ToListAsync();

            var occupations = await _context.HouseholdOccupationAndLand
                .Where(x => uniqueIds.Contains(x.UniqueId))
                .ToListAsync();

            var familyMembers = await _context.HouseholdFamilyMember
                .Where(x => uniqueIds.Contains(x.UniqueId))
                .ToListAsync();

            var households = basicProfiles.Select(bp => new Household
            {
                HouseholdBasicProfile = bp,
                HouseholdEntitlement = entitlements.FirstOrDefault(e => e.UniqueId == bp.UniqueId),
                HouseholdMigrationStatus = migrations.FirstOrDefault(m => m.UniqueId == bp.UniqueId),
                HouseholdOccupationAndLand = occupations.FirstOrDefault(o => o.UniqueId == bp.UniqueId),
                HouseholdFamilyMember = familyMembers.Where(f => f.UniqueId == bp.UniqueId).ToList()
            }).OrderByDescending(x => x.HouseholdBasicProfile?.EntryDate).ToList();

            // 🔹 Final response
            var response = new PagedResponse<Household>
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalRecords = totalRecords,
                TotalPages = (int)Math.Ceiling(totalRecords / (double)pageSize),
                Data = households
            };

            return Ok(response);
        }



    [HttpPost("search")]
        [Authorize]
        public async Task<IActionResult> SearchHouseholds([FromBody] HouseholdSearchFilter filter)
        {
            var username = User.Identity?.Name;
            var roleId = User.FindFirst(ClaimTypes.Role)?.Value;
            Console.WriteLine("******************* CREATE BY ATUL ******************");
            var from = filter.FromDate.Date;
            var to = filter.ToDate.Date.AddDays(1);

            IQueryable<HouseholdBasicProfile> query =
                _context.HouseholdBasicProfile
                .Where(x => x.EntryDate >= from && x.EntryDate < to);

            // 🔹 Role based filtering
            if (roleId != "2")
            {
                query = query.Where(x => x.entryBy == username);
            }
            else
            {
                if (!string.IsNullOrEmpty(filter.EntryBy))
                    query = query.Where(x => x.entryBy == filter.EntryBy);
            }

            // 🔹 Dynamic filters
            if (!string.IsNullOrEmpty(filter.District))
                query = query.Where(x => x.District == filter.District);

            if (!string.IsNullOrEmpty(filter.Block))
                query = query.Where(x => x.Block == filter.Block);

            if (!string.IsNullOrEmpty(filter.GramPanchayat))
                query = query.Where(x => x.GramPanchayat == filter.GramPanchayat);

            if (!string.IsNullOrEmpty(filter.RevenueVillage))
                query = query.Where(x => x.RevenueVillage == filter.RevenueVillage);

            if (!string.IsNullOrEmpty(filter.SocialCategory))
                query = query.Where(x => x.SocialCategory == filter.SocialCategory);

            if (filter.HasRationCard.HasValue)
                query = query.Where(x => x.HasRationCard == filter.HasRationCard);
            
            if(filter.status.HasValue)
            {
                query = query.Where(x => x.sstatus == filter.status);
            }   

            Console.WriteLine(query);
            // 🔹 Total count (before pagination)
            var totalRecords = await query.CountAsync();

            // 🔹 Pagination
            var pageNumber = filter.PageNumber < 1 ? 1 : filter.PageNumber;
            var pageSize = filter.PageSize < 1 ? 10 : filter.PageSize;

            var basicProfiles = await query
                .OrderByDescending(x => x.EntryDate)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var uniqueIds = basicProfiles.Select(x => x.UniqueId).ToList();

            // 🔹 Related data
            var entitlements = await _context.HouseholdEntitlement
                .Where(x => uniqueIds.Contains(x.UniqueId))
                .ToListAsync();

            var migrations = await _context.HouseholdMigrationStatus
                .Where(x => uniqueIds.Contains(x.UniqueId))
                .ToListAsync();

            var occupations = await _context.HouseholdOccupationAndLand
                .Where(x => uniqueIds.Contains(x.UniqueId))
                .ToListAsync();

            var familyMembers = await _context.HouseholdFamilyMember
                .Where(x => uniqueIds.Contains(x.UniqueId))
                .ToListAsync();

            var households = basicProfiles.Select(bp => new Household
            {
                HouseholdBasicProfile = bp,
                HouseholdEntitlement = entitlements.FirstOrDefault(e => e.UniqueId == bp.UniqueId),
                HouseholdMigrationStatus = migrations.FirstOrDefault(m => m.UniqueId == bp.UniqueId),
                HouseholdOccupationAndLand = occupations.FirstOrDefault(o => o.UniqueId == bp.UniqueId),
                HouseholdFamilyMember = familyMembers.Where(f => f.UniqueId == bp.UniqueId).ToList()
            }).OrderByDescending(x => x.HouseholdBasicProfile?.EntryDate).ToList();

            // 🔹 Final response
            var response = new PagedResponse<Household>
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalRecords = totalRecords,
                TotalPages = (int)Math.Ceiling(totalRecords / (double)pageSize),
                Data = households
            };

            return Ok(response);
        }





        [HttpGet("date/{fromDate}/{toDate}")]
        public async Task<ActionResult<IEnumerable<Household>>> GetHouseholdByDateRange(string fromDate, string toDate)
        {



            var username = User.FindFirst(ClaimTypes.Name)?.Value;
            var roleId = User.FindFirst(ClaimTypes.Role)?.Value;
            var basicProfiles = new List<HouseholdBasicProfile>();

            if (string.IsNullOrWhiteSpace(fromDate) || string.IsNullOrWhiteSpace(toDate))
            {
                return BadRequest("Both fromDate and toDate are required.");
            }
            if (!DateTime.TryParse(fromDate, out DateTime from) ||
                !DateTime.TryParse(toDate, out DateTime to))
            {
                return BadRequest("Invalid date format. Use yyyy-MM-dd.");
            }
            if (from > to)
            {
                return BadRequest("fromDate cannot be greater than toDate.");
            }


            var entitlements = new List<HouseholdEntitlement>();
            var migrations = new List<HouseholdMigrationStatus>();
            var occupations = new List<HouseholdOccupationAndLand>();
            var familyMembers = new List<HouseholdFamilyMember>();
            if (roleId == "2")
            {
                basicProfiles = await _context.HouseholdBasicProfile.Where(x => x.EntryDate >= from.Date && x.EntryDate < to.Date.AddDays(1)).ToListAsync();

            }
            else
            {
                basicProfiles = await _context.HouseholdBasicProfile.Where(x => x.entryBy == username && x.EntryDate >= from.Date && x.EntryDate < to.Date.AddDays(1)).ToListAsync();

            }
            entitlements = await _context.HouseholdEntitlement.Where(x => x.entryDate >= from.Date && x.entryDate < to.Date.AddDays(1)).ToListAsync();
            migrations = await _context.HouseholdMigrationStatus.Where(x => x.entryDate >= from.Date && x.entryDate < to.Date.AddDays(1)).ToListAsync();
            occupations = await _context.HouseholdOccupationAndLand.Where(x => x.entryDate >= from.Date && x.entryDate < to.Date.AddDays(1)).ToListAsync();
            familyMembers = await _context.HouseholdFamilyMember.Where(x => x.entryDate >= from.Date && x.entryDate < to.Date.AddDays(1)).ToListAsync();
            var households = basicProfiles.Select(bp => new Household
            {
                HouseholdBasicProfile = bp,

                HouseholdEntitlement = entitlements
             .FirstOrDefault(e => e.UniqueId == bp.UniqueId),

                HouseholdMigrationStatus = migrations
             .FirstOrDefault(m => m.UniqueId == bp.UniqueId),

                HouseholdOccupationAndLand = occupations
             .FirstOrDefault(o => o.UniqueId == bp.UniqueId),

                HouseholdFamilyMember = familyMembers
             .Where(f => f.UniqueId == bp.UniqueId)
             .ToList()
            })
            .ToList();
            return Ok(households);
        }

        // POST: api/StateList
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<ActionResult> PostHousehold([FromForm] string householdJson,[FromForm] IFormFile? respondentPhoto)
        {
             var household = JsonSerializer.Deserialize<Household>(
        householdJson,
        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            var username = User.FindFirst(ClaimTypes.Name)?.Value;
            if (household == null || household.HouseholdBasicProfile == null)
                return BadRequest("Invalid household data");
            household.HouseholdBasicProfile.EntryDate = DateTime.Now;
            household.HouseholdBasicProfile.entryBy = username;
            
            var uniqueId = Guid.NewGuid().ToString();
            if (string.IsNullOrWhiteSpace(uniqueId))
                return BadRequest("UniqueId is required");

 var validationResult = await _householdValidator.ValidateAsync(household);
 if (!validationResult.IsValid)
    {
        return Ok(new
        {
            success = false,
            title = "One or more validation errors occurred.",
            errors = validationResult.Errors
                .GroupBy(e => e.PropertyName)
                .ToDictionary(
                    g => g.Key,
                    g => g.Select(e => e.ErrorMessage).ToArray()
                ),
            traceId = HttpContext.TraceIdentifier
        });
    }
if (respondentPhoto != null && respondentPhoto.Length > 0)
    {
        var uploadsPath = Path.Combine("Uploads", "RespondentPhotos");
        Directory.CreateDirectory(uploadsPath);

        var fileName = $"{uniqueId}{Path.GetExtension(respondentPhoto.FileName)}";
        var filePath = Path.Combine(uploadsPath, fileName);

        using var stream = new FileStream(filePath, FileMode.Create);
        await respondentPhoto.CopyToAsync(stream);

        // Example: save path in model
        household.HouseholdMigrationStatus!.RespondentPhotoPathOrUrl = filePath;
    }



            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                household.HouseholdBasicProfile.UniqueId = uniqueId;
                household.HouseholdBasicProfile.entryBy = username;
                household.HouseholdBasicProfile.sstatus = 0;
                // 1️⃣ Basic Profile (Required)
                _context.HouseholdBasicProfile.Add(household.HouseholdBasicProfile);

                // 2️⃣ Entitlement (Optional)
                if (household.HouseholdEntitlement != null)
                {
                    household.HouseholdEntitlement.UniqueId = uniqueId;
                    _context.HouseholdEntitlement.Add(household.HouseholdEntitlement);
                }

                // 3️⃣ Migration Status (Optional)
                if (household.HouseholdMigrationStatus != null)
                {
                    household.HouseholdMigrationStatus.UniqueId = uniqueId;
                    _context.HouseholdMigrationStatus.Add(household.HouseholdMigrationStatus);
                }

                // 4️⃣ Occupation & Land (Optional)
                if (household.HouseholdOccupationAndLand != null)
                {
                    household.HouseholdOccupationAndLand.UniqueId = uniqueId;
                    if(household.HouseholdOccupationAndLand.FRA_LandAmountInAcres == null)
                    {
                        household.HouseholdOccupationAndLand.FRA_LandAmountInAcres = 0;
                    }
                    _context.HouseholdOccupationAndLand.Add(household.HouseholdOccupationAndLand);
                }

                // 5️⃣ Family Members (0..N)
                if (household.HouseholdFamilyMember?.Any() == true)
                {
                    foreach (var member in household.HouseholdFamilyMember)
                    {
                        member.UniqueId = uniqueId;
                    }

                    _context.HouseholdFamilyMember.AddRange(household.HouseholdFamilyMember);
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return CreatedAtAction(
                
                    nameof(GetHousehold),
                    new { uniqueid = uniqueId },
                    new { success = true, 
        message = "Household saved successfully",UniqueId = uniqueId }
                );

         

            }



            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return Ok(new
        {
            success = false,
            title = "Something went wrong.",
            errors = ex.Message.ToString(),
            traceId = HttpContext.TraceIdentifier
        });
        }
        }

[HttpPost("{uniqueId}")]
public async Task<ActionResult> UpdateHousehold(string uniqueId, [FromForm] string householdJson, [FromForm] IFormFile? respondentPhoto)
{
    try 
    {
        // 1. Deserialize the incoming JSON
        var incoming = JsonSerializer.Deserialize<Household>(householdJson, 
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true, NumberHandling = JsonNumberHandling.AllowReadingFromString });

        if (incoming == null) return BadRequest("Invalid Data");

        // 2. Fetch the MAIN Profile
        var existingProfile = await _context.HouseholdBasicProfile
            .FirstOrDefaultAsync(x => x.UniqueId == uniqueId);

        if (existingProfile == null) return NotFound("Household not found");
var originalGeo = existingProfile.GeoLocation;
        var originalEntryBy = existingProfile.entryBy;
        var originalEntryDate = existingProfile.EntryDate; // Good practice to keep original date
        // 3. Update Basic Profile
        // Copy values from 'incoming' to 'existing'
        _context.Entry(existingProfile).CurrentValues.SetValues(incoming.HouseholdBasicProfile);

existingProfile.GeoLocation = originalGeo;
        existingProfile.entryBy = originalEntryBy;
        existingProfile.EntryDate = originalEntryDate;
        // 4. Update Entitlements (Fetch -> Check -> Update)
        var existingEntitlement = await _context.HouseholdEntitlement
            .FirstOrDefaultAsync(x => x.UniqueId == uniqueId);
            
        if (existingEntitlement != null && incoming.HouseholdEntitlement != null)
        {
            // Preserve the Primary Key (Id) so EF knows it's the same row
            incoming.HouseholdEntitlement.Id = existingEntitlement.Id; 
            incoming.HouseholdEntitlement.UniqueId = uniqueId; 
            _context.Entry(existingEntitlement).CurrentValues.SetValues(incoming.HouseholdEntitlement);
        }


        // 5. Update Occupation (Fetch -> Check -> Update)
        var existingOcc = await _context.HouseholdOccupationAndLand
            .FirstOrDefaultAsync(x => x.UniqueId == uniqueId);

        if (existingOcc != null && incoming.HouseholdOccupationAndLand != null)
        {
            incoming.HouseholdOccupationAndLand.Id = existingOcc.Id;
            incoming.HouseholdOccupationAndLand.UniqueId = uniqueId;
            _context.Entry(existingOcc).CurrentValues.SetValues(incoming.HouseholdOccupationAndLand);
        }


        // 6. Update Migration (Fetch -> Check -> Update)
        var existingMig = await _context.HouseholdMigrationStatus
            .FirstOrDefaultAsync(x => x.UniqueId == uniqueId);

        if (existingMig != null && incoming.HouseholdMigrationStatus != null)
        {
            // Preserve Photo Path if user didn't upload a new one
            if (respondentPhoto == null && !string.IsNullOrEmpty(existingMig.RespondentPhotoPathOrUrl))
            {
                incoming.HouseholdMigrationStatus.RespondentPhotoPathOrUrl = existingMig.RespondentPhotoPathOrUrl;
            }

            incoming.HouseholdMigrationStatus.Id = existingMig.Id;
            incoming.HouseholdMigrationStatus.UniqueId = uniqueId;
            _context.Entry(existingMig).CurrentValues.SetValues(incoming.HouseholdMigrationStatus);
        }

        // 7. Handle Photo Upload (If new photo provided)
        if (respondentPhoto != null && respondentPhoto.Length > 0 && existingMig != null)
        {
             var uploadsPath = Path.Combine("Uploads", "RespondentPhotos");
             var fileName = $"{uniqueId}_v2{Path.GetExtension(respondentPhoto.FileName)}";
             var filePath = Path.Combine(uploadsPath, fileName);
             
             using var stream = new FileStream(filePath, FileMode.Create);
             await respondentPhoto.CopyToAsync(stream);
             
             existingMig.RespondentPhotoPathOrUrl = filePath; // Update the path directly
        }


        // 8. Update Family Members (Delete Old -> Insert New)
        // This is the safest way for lists
        var existingMembers = _context.HouseholdFamilyMember.Where(x => x.UniqueId == uniqueId);
        _context.HouseholdFamilyMember.RemoveRange(existingMembers);
        
        if (incoming.HouseholdFamilyMember != null)
        {
            foreach (var member in incoming.HouseholdFamilyMember)
            {
                member.Id = 0; // Reset ID to 0 to force Insert
                member.UniqueId = uniqueId; // Ensure link
                _context.HouseholdFamilyMember.Add(member);
            }
        }

        // 9. Save All Changes in one transaction
        await _context.SaveChangesAsync();

        return Ok(new { success = true, message = "Updated successfully" });
    }
    catch (Exception ex)
    {
        return StatusCode(500, new { success = false, message = ex.Message });
    }
}
        // DELETE: api/StateList/5
        [HttpDelete("{uniqueId}")]
        public async Task<IActionResult> DeleteHousehold(string uniqueId)
        {
            var stateList = await _context.StateList.FindAsync(uniqueId);
            if (stateList == null)
            {
                return NotFound();
            }

            _context.StateList.Remove(stateList);
            await _context.SaveChangesAsync();

            return NoContent();
        }


         [HttpPost("update-status")]
        public async Task<ActionResult> PostUpdateStatus(householdStatusChange householdStatusChange)
        {
            if(householdStatusChange == null)
            {
                return BadRequest("Invalid data");
            }
            
        

// logged-in user
var username = User?.FindFirst(ClaimTypes.Name)?.Value ?? "SYSTEM";

// update fields
try
    {
        var rows = await _context.Database.ExecuteSqlRawAsync(
            @"UPDATE HouseholdBasicProfile
              SET sstatus = @sstatus,
                  approvedBy = @approvedBy,
                  approvedDate = @approvedDate,
                  reason = @reason
              WHERE UniqueId = @uniqueId",
            new SqlParameter("@sstatus", householdStatusChange.sstatus),
            new SqlParameter("@approvedBy", username),
            new SqlParameter("@approvedDate", DateTime.Now),
             new SqlParameter("@reason", householdStatusChange.reason),
            new SqlParameter("@uniqueId", householdStatusChange.uniqueId)
        );

        if (rows == 0)
        {
            return Ok(new
{
    success = false,
    message = "No record found"
});
        }

        return Ok(new
{
    success = true,
    message = "Status updated successfully"
});
}
    catch (Exception ex)
    {
        return StatusCode(200, new
        {
            message = "Database update failed",
            error = ex.Message,
            inner = ex.InnerException?.Message,
            success=false
        });
    }

        }

[HttpGet("survey-progress")]
public async Task<IActionResult> GetSurveyProgress()
{
    // 1. Fetch raw data (Projection is faster than fetching full entities)
    var rawData = await _context.HouseholdBasicProfile
        .Select(h => new 
        { 
            h.District, 
            h.Block, 
            h.GramPanchayat, 
            h.RevenueVillage,
            h.sstatus // 0=Pending, 1=Approved, 10=Rejected
        })
        .ToListAsync();

    // 2. Group by Admin Unit (e.g., Block level summary)
    // You can change this to group by Panchayat if you want more detail
    var report = rawData
        .GroupBy(x => new { x.District, x.Block, x.GramPanchayat }) 
        .Select(g => new SurveySummaryDto
        {
            District = g.Key.District,
            Block = g.Key.Block,
            Panchayat = g.Key.GramPanchayat, // Aggregated
            TotalVillages = g.Select(x => x.RevenueVillage).Distinct().Count(),
            
            // Logic: Assume Target is 500 per village for this example
            // In real app, join with a TargetMaster table
            TargetHouseholds = g.Select(x => x.RevenueVillage).Distinct().Count() * 500, 
            
            CompletedSurveys = g.Count(x => x.sstatus == 1),
            PendingApproval = g.Count(x => x.sstatus == 0),
            Rejected = g.Count(x => x.sstatus == 10)
        })
        .OrderByDescending(x => x.CompletionPercentage)
        .ToList();

    return Ok(report);
}
        private bool StateListExists(int id)
        {
            return _context.StateList.Any(e => e.StateCode == id);
        }
    }
}
