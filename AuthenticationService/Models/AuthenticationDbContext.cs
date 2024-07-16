using Microsoft.EntityFrameworkCore;

namespace AuthenticationService.Models;

public class AuthenticationDbContext : DbContext
{
    public AuthenticationDbContext(DbContextOptions<AuthenticationDbContext> options) : base(options)
    {
    }
    
    public DbSet<Driver> Drivers { get; set; }
    public DbSet<Company> Companies { get; set; }
    public DbSet<CompanyAddress> CompanyAddresses { get; set; }
    public DbSet<ServiceCity> ServiceCities { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<MembershipType> MembershipTypes { get; set; }
    public DbSet<MembershipFee> MembershipFees { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        // 1. MembershipFee - MembershipType
        modelBuilder.Entity<MembershipFee>()
            .HasOne(mf => mf.MembershipType)
            .WithMany(mt => mt.MembershipFees)
            .HasForeignKey(mf => mf.MembershipTypeId);
        
        // 2. Company - MembershipType
        modelBuilder.Entity<Company>()
            .HasOne(c => c.MembershipType)
            .WithMany(mt => mt.Companies)
            .HasForeignKey(mf => mf.MembershipTypeId);
        
        // 3. CompanyAddress - Company
        modelBuilder.Entity<CompanyAddress>()
            .HasOne(ca => ca.Company)
            .WithMany(c => c.CompanyAddresses)
            .HasForeignKey(ca => ca.CompanyId);
        
        // 4. Driver - Company
        modelBuilder.Entity<Driver>()
            .HasOne(d => d.Company)
            .WithMany(c => c.Drivers)
            .HasForeignKey(d => d.CompanyId);
        
        // 5. ServiceCity - Company
        modelBuilder.Entity<ServiceCity>()
            .HasOne(sc => sc.Company)
            .WithMany(c => c.ServiceCities)
            .HasForeignKey(sc => sc.CompanyId);
        
        
        
    }
}