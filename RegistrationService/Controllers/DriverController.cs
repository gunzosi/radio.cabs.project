using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RegistrationService.DTOs;
using RegistrationService.Models;
using RegistrationService.Services.IServices;

namespace RegistrationService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DriverController : ControllerBase
    {
        private readonly RegistrationDbContext _context;
        private readonly IBlobServices _blobServices;

        public DriverController(RegistrationDbContext context, IBlobServices blobServices)
        {
            _context = context;
            _blobServices = blobServices;
        }

        [HttpGet("getProfile/{id}")]
        public async Task<IActionResult> GetProfile(int id)
        {
            var driver = await _context.Drivers.FindAsync(id);
            if (driver == null)
            {
                return NotFound("Driver not found");
            }

            return Ok(new
            {
                statusCode = 200,
                data = driver
            });
        }

        [HttpPut("updateProfile/{id}")]
        public async Task<IActionResult> UpdateProfile(int id, [FromForm] DriverUpdateDto driverDto)
        {
            var driver = await _context.Drivers.FindAsync(id);
            if (driver == null)
            {
                return NotFound("Driver not found");
            }

            driver.DriverName = driverDto.DriverName;
            driver.DriverMobile = driverDto.DriverMobile;
            driver.DriverEmail = driverDto.DriverEmail;
            driver.DriverLicense = driverDto.DriverLicense;
            driver.Address = driverDto.Address;
            driver.Street = driverDto.Street;
            driver.Ward = driverDto.Ward;
            driver.District = driverDto.District;
            driver.City = driverDto.City;
            
            if (driverDto.DriverPersonalImage != null)
            {
                var personalImage = await _blobServices.UploadBlobAsync(driverDto.DriverPersonalImage);
                driver.DriverPersonalImage = personalImage;
            }
            
            if (driverDto.DriverLicenseImage != null)
            {
                var licenseImage = await _blobServices.UploadBlobAsync(driverDto.DriverLicenseImage);
                driver.DriverLicenseImage = licenseImage;
            }

            _context.Entry(driver).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Drivers.Any(e => e.Id == id))
                {
                    return NotFound("Driver not found for UPDATE");
                }
                else
                {
                    throw;
                }
            }
            
            return Ok(new
            {
                statusCode = 200,
                data = driver,
                message = "Driver updated successfully"
            });
        }
        
        
        // 2. Driver can apply to company
        [HttpPost("applyToCompany")]
        public async Task<IActionResult> ApplyToCompany([FromBody] ApplyDto applyDto)
        {
            var driver = await _context.Drivers.FindAsync(applyDto.DriverId);
            if (driver == null)
            {
                return NotFound("Driver not found");
            }
            
            var company = await _context.Companies.FindAsync(applyDto.CompanyId);
            if (company == null)
            {
                return NotFound("Company not found");
            }

            var existingApplication = await _context.DriverApplications
                .Where(app => app.DriverId == applyDto.DriverId && app.Status == "Pending")
                .FirstOrDefaultAsync();

            if (existingApplication != null)
            {
                return BadRequest("You already have a pending application.");
            }

            var application = new DriverApplications
            {
                DriverId = applyDto.DriverId,
                CompanyId = applyDto.CompanyId,
                ApplicationDate = DateTime.UtcNow,
                Status = "Pending"
            };
            
            await _context.DriverApplications.AddAsync(application);
            await _context.SaveChangesAsync();
            
            return Ok(new
            {
                statusCode = 200,
                Application  = application,
                message = "Application submitted successfully"
            });

        }
    }
}
