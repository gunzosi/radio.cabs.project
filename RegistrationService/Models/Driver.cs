using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RegistrationService.Models
{
    public class Driver
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string DriverName { get; set; }
        public string? DriverCode { get; set; }
        public long DriverMobile { get; set; }
        public string? DriverEmail { get; set; }
        public string Password { get; set; }
        public string DriverLicense { get; set; }
        public string Address { get; set; }
        public string Street { get; set; }
        public string Ward { get; set; }
        public string District { get; set; }
        public string City { get; set; }
        public string? DriverPersonalImage { get; set; }
        public string? DriverLicenseImage { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiryTime { get; set; }

        public int? CompanyId { get; set; } // Allow CompanyId to be nullable
        public Company? Company { get; set; }
        public ICollection<DriverApplications> DriverApplications { get; set; }
    }
}