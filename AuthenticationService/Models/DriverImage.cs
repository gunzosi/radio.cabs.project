using System.ComponentModel.DataAnnotations;

namespace AuthenticationService.Models;

public class DriverImage
{
    [Key]
    public int Id { get; set; }
    public int DriverId { get; set; }
    public string DriverPersonalImage { get; set; }
    public string DriverLicenseImage { get; set; }
    public Driver Driver { get; set; }
}