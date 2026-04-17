using HHSurvey.Data;
using HHSurvey.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
namespace HHSurvey.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class MasterController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public MasterController(ApplicationDbContext context)
        {
            _context = context;
        }
    [HttpGet("all")]
    public async Task<IActionResult> GetAllMasterData()
    {
        var response = new MasterDataResponse
        {
            Relationship = await _context.Relationship.ToListAsync(),
            GenderType = await _context.GenderType.ToListAsync(),
            EducationalQualification = await _context.EducationalQualification.ToListAsync(),
            SocialCategory = await _context.SocialCategory.ToListAsync(),
            DrinkingWaterSource = await _context.DrinkingWaterSource.ToListAsync(),
            RespondentIdentity = await _context.RespondentIdentity.ToListAsync(),
            MigrationSector = await _context.MigrationSector.ToListAsync(),
            MigrationPeriod = await _context.MigrationPeriod.ToListAsync(),
            SourcesOfIrrigation = await _context.SourcesOfIrrigation.ToListAsync(),
            PrimaryOccupation = await _context.PrimaryOccupation.ToListAsync(),
            ApproximatePrivateLandHolding = await _context.ApproximatePrivateLandHolding.ToListAsync(),
            InvolvedInLivestockActivity = await _context.InvolvedInLivestockActivity.ToListAsync(),
            KishanSchemeCoverage = await _context.KishanSchemeCoverage.ToListAsync()
        };

        return Ok(response);
    }
 private async Task<IActionResult> GetData<T>() where T : class
    {
        var data = await _context.Set<T>().AsNoTracking().ToListAsync();
        return Ok(data);
    }
        [HttpGet("relationship")]
    public async Task<IActionResult> GetRelationship()
        => await GetData<Relationship>();

    [HttpGet("gender")]
    public async Task<IActionResult> GetGender()
        => await GetData<GenderType>();

    [HttpGet("education")]
    public async Task<IActionResult> GetEducation()
        => await GetData<EducationalQualification>();

    [HttpGet("social-category")]
    public async Task<IActionResult> GetSocialCategory()
        => await GetData<SocialCategory>();

    [HttpGet("drinking-water-source")]
    public async Task<IActionResult> GetDrinkingWaterSource()
        => await GetData<DrinkingWaterSource>();

    [HttpGet("respondent-identity")]
    public async Task<IActionResult> GetRespondentIdentity()
        => await GetData<RespondentIdentity>();

    [HttpGet("migration-sector")]
    public async Task<IActionResult> GetMigrationSector()
        => await GetData<MigrationSector>();

    [HttpGet("migration-period")]
    public async Task<IActionResult> GetMigrationPeriod()
        => await GetData<MigrationPeriod>();

    [HttpGet("irrigation-source")]
    public async Task<IActionResult> GetIrrigationSource()
        => await GetData<SourcesOfIrrigation>();

    [HttpGet("primary-occupation")]
    public async Task<IActionResult> GetPrimaryOccupation()
        => await GetData<PrimaryOccupation>();

    [HttpGet("land-holding")]
    public async Task<IActionResult> GetLandHolding()
        => await GetData<ApproximatePrivateLandHolding>();

    [HttpGet("livestock-activity")]
    public async Task<IActionResult> GetLivestockActivity()
        => await GetData<InvolvedInLivestockActivity>();

    [HttpGet("kishan-scheme")]
    public async Task<IActionResult> GetKishanScheme()
        => await GetData<KishanSchemeCoverage>();
}
}
