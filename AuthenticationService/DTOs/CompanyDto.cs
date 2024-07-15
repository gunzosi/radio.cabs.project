namespace AuthenticationService.DTOs;

public class CompanyDto
{
    public string CompanyName { get; set; }
    public string CompanyTaxCode { get; set; }
    public string ContactPerson { get; set; }
    public string Designation { get; set; }
    public long CpMobile { get; set; }
    public long CompanyTelephone { get; set; }
    public long FaxNumber { get; set; }
    public string CompanyEmail { get; set; }
    public string Password { get; set; }
}