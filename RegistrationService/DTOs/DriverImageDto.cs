﻿namespace RegistrationService.DTOs
{
    public class DriverUpdateDto
    {
        public string DriverName { get; set; }
        public long DriverMobile { get; set; }
        public string DriverEmail { get; set; }
        public string DriverLicense { get; set; }
        public string Address { get; set; }
        public string Street { get; set; }
        public string Ward { get; set; }
        public string District { get; set; }
        public string City { get; set; }
        public IFormFile DriverPersonalImage { get; set; }
        public IFormFile DriverLicenseImage { get; set; }
    }
}

