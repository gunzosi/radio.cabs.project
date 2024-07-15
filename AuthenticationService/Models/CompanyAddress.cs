using System.ComponentModel.DataAnnotations;

namespace AuthenticationService.Models;

public class CompanyAddress
{
    [Key]
    public int Id { get; set; }
    [Required]
    public int CompanyId { get; set; }
    public string Address { get; set; }
    public string Ward { get; set; }
    public string District { get; set; }
    public string City { get; set; }
    
    public string FullAddress => $"{Address}, {Ward}, {District}, {City}";
    
    public Company Company { get; set; }
}