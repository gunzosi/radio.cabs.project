using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AuthenticationService.Models;

public class DriverApplications
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public int DriverId { get; set; }
    public int CompanyId { get; set; }
    public DateTime ApplicationDate { get; set; }
    public string Status { get; set; }
    
    public Driver Driver { get; set; }
    public Company Company { get; set; }
}