
using HHSurvey.Models;
using Microsoft.EntityFrameworkCore;

namespace HHSurvey.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
           : base(options)
        {
        }


        public DbSet<UserLogin> UserLogin { get; set; }
        public DbSet<RoleMaster> RoleMaster { get; set; }
        public DbSet<DashboardDTO> DashboardDTO {get;set;}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<StateList>().HasKey(e => e.StateCode);
            modelBuilder.Entity<DashboardDTO>().HasNoKey();
        }
        public DbSet<StateList> StateList { get; set; } = default!;
        public DbSet<DistrictList> DistrictList { get; set; } = default!;
        public DbSet<BlockList> BlockList { get; set; } = default!;
        public DbSet<PanchayatList> PanchayatList { get; set; } = default!;
          public DbSet<VillageList> VillageList { get; set; } = default!;
           public DbSet<BankList> BankList { get; set; } = default!;
        public DbSet<bankIFSCode> bankIFSCode { get; set; } = default!;
        public DbSet<LoginHistory> LoginHistory { get; set; } = default!;
        public DbSet<ApiLog> ApiLog { get; set; }
        public DbSet<MigrationSurvey> MigrationSurvey { get; set; }
        public DbSet<HouseholdBasicProfile> HouseholdBasicProfile { get; set; }
        public DbSet<HouseholdEntitlement> HouseholdEntitlement { get; set; }
        public DbSet<HouseholdFamilyMember> HouseholdFamilyMember { get; set; }
        public DbSet<HouseholdMigrationStatus> HouseholdMigrationStatus { get; set; }
        public DbSet<HouseholdOccupationAndLand> HouseholdOccupationAndLand { get; set; }

    }
}
