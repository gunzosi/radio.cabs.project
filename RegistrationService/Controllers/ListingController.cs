using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RegistrationService.Models;

namespace RegistrationService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ListingController : ControllerBase
    {
        private readonly RegistrationDbContext _dbContext;
        public ListingController(RegistrationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        
        [AllowAnonymous]
        [HttpGet("/all/drivers")]
        public async  Task<IActionResult> Get()
        {
            var listings = await _dbContext.Drivers.ToListAsync();
            return Ok(listings);
        }
        
        [AllowAnonymous]
        [HttpGet("driver/{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var listing = await _dbContext.Drivers.FindAsync(id);
            if (listing == null)
            {
                return NotFound();
            }
            return Ok(listing);
        }
        
        [AllowAnonymous]
        [HttpGet("company")]
        public async Task<IActionResult> GetCompanies()
        {
            var companies = await _dbContext.Companies.ToListAsync();
            return Ok(companies);
        }
        
        [AllowAnonymous]
        [HttpGet("company/{id}")]
        public async Task<IActionResult> GetCompany(int id)
        {
            var company = await _dbContext.Companies.FindAsync(id);
            if (company == null)
            {
                return NotFound();
            }
            return Ok(company);
        }
        
        // Filter method for all DRIVER and COMPANY 
        
        // 1. USER FILTER BY NAME (DRIVER NAME, COMPANY NAME) 
        [HttpPost("filter/drivername")]
        public async Task<IActionResult> FilterByDriverName(string name)
        {
            var drivers = await _dbContext.Drivers.Where(d => d.DriverName.Contains(name)).ToListAsync();
            return Ok(new
            {
                Drivers = drivers,
            });
        }
        
        [HttpPost("filter/companyname")]
        public async Task<IActionResult> FilterByCompanyName(string name)
        {
            var companies = await _dbContext.Companies.Where(c => c.CompanyName.Contains(name)).ToListAsync();
            return Ok(new
            {
                Companies = companies,
            });
        }
        
        // 2. USER FILTER BY LOCATION SERVICE of DRIVER (LOCATION) and COMPANY (LOCATION SERVICE)
        [HttpPost("filter/driverlocation")]
        public async Task<IActionResult> FilterByDriverLocation(string location)
        {
            var drivers = await _dbContext.Drivers.Where(d => d.City.Contains(location)).ToListAsync();
            return Ok(new
            {
                Drivers = drivers,
            });
        }

        [HttpPost("filter/companylocation")]
        public async Task<IActionResult> FilterByCompanyLocation(string location)
        {
            var serviceCities = await _dbContext.ServiceCities
                .Where(s => s.City.Contains(location))
                .Include(s => s.Company) // Include the Company navigation property
                .ToListAsync();

            var companies = serviceCities.Select(s => s.Company).Distinct().ToList();

            return Ok(new
            {
                Companies = companies
            });
        }

        
        
        // 3. USER FILTER BY DRIVER CODE and COMPANY CODE
        [HttpPost("filter/drivercode")]
        public async Task<IActionResult> FilterByDriverCode(string code)
        {
            var drivers = await _dbContext.Drivers
                .Where(d => d.DriverCode.Contains(code)).ToListAsync();
            return Ok(new
            {
                Drivers = drivers,
            });
        }
        
        // 5. USER FILTER BY RATING
        
        
       
        
    }
}
