using Microsoft.EntityFrameworkCore;

namespace RegistrationService.Models;

public class RegistrationDbContext : DbContext
{
    public RegistrationDbContext(DbContextOptions<RegistrationDbContext> options) : base(options)
    {
    }
    
    public DbSet<Driver> Drivers { get; set; }
    public DbSet<Company> Companies { get; set; }
    public DbSet<CompanyAddress> CompanyAddresses { get; set; }
    public DbSet<ServiceCity> ServiceCities { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<MembershipType> MembershipTypes { get; set; }
    public DbSet<MembershipFee> MembershipFees { get; set; }
    

    public DbSet<DriverApplications> DriverApplications { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Other configurations...

        modelBuilder.Entity<DriverApplications>()
            .HasOne(da => da.Driver)
            .WithMany(d => d.DriverApplications)
            .HasForeignKey(da => da.DriverId);

        modelBuilder.Entity<DriverApplications>()
            .HasOne(da => da.Company)
            .WithMany(c => c.DriverApplications)
            .HasForeignKey(da => da.CompanyId);
    }
    
}