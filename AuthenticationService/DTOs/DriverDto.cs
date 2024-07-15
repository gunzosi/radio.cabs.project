namespace AuthenticationService.DTOs;

public class DriverDto
{
    public string DriverName { get; set; }
    public string DriverCode { get; set; }
    public long DriverMobile { get; set; }
    public string DriverEmail { get; set; }
    public string Password { get; set; }
    public string DriverLicense { get; set; }
    public int CompanyId { get; set; }
    public string Address { get; set; }
    public string Ward { get; set; }
    public string District { get; set; }
    public string City { get; set; }
}