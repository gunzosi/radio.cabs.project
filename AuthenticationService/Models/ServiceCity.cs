using System.ComponentModel.DataAnnotations;

namespace AuthenticationService.Models;

public class ServiceCity
{
    [Key]
    public int Id { get; set; }
    public int CompanyId { get; set; }
    public string City { get; set; }
    public Company Company { get; set; }
}