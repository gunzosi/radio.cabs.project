using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AuthenticationService.Models;

public class Company
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    [Required]
    public string CompanyName { get; set; }
    [Required]
    public string CompanyTaxCode { get; set; }
    public string ContactPerson { get; set; }
    public string Designation { get; set; }
    public long ContactPersonNumber { get; set; }
    public long CompanyTelephone { get; set; }
    public long FaxNumber { get; set; }
    [Required]
    public string CompanyEmail { get; set; }
    [Required]
    public string Password { get; set; }
    public int MembershipTypeId { get; set; }
    
    public string? RefreshToken { get; set; }
    public DateTime? RefreshTokenExpiryTime { get; set; }
    
    public MembershipType MembershipType { get; set; }
    public ICollection<CompanyAddress> CompanyAddresses { get; set; }
    public ICollection<Driver> Drivers { get; set; }
    public ICollection<ServiceCity> ServiceCities { get; set; }
}