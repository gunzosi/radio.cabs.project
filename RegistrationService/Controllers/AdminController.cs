using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RegistrationService.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RegistrationService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly RegistrationDbContext _context;

        public AdminController(RegistrationDbContext context)
        {
            _context = context;
        }

        [HttpPost("addMembershipType")]
        public async Task<ActionResult<MembershipType>> AddMembershipTypeAsync(MembershipType membershipType)
        {
            _context.MembershipTypes.Add(membershipType);
            await _context.SaveChangesAsync();

            return CreatedAtAction(
                    nameof(GetMembershipTypeByIdAsync), 
                    new { id = membershipType.Id }
                , membershipType);
        }

        [HttpGet("getMembershipTypes")]
        public async Task<ActionResult<IEnumerable<MembershipType>>> GetMembershipTypesAsync()
        {
            return await _context.MembershipTypes.ToListAsync();
        }

        [HttpGet("getMembershipType/{id}")]
        public async Task<ActionResult<MembershipType>> GetMembershipTypeByIdAsync(int id)
        {
            var membershipType = await _context.MembershipTypes.FindAsync(id);

            if (membershipType == null)
            {
                return NotFound();
            }

            return membershipType;
        }

        [HttpPut("updateMembershipType/{id}")]
        public async Task<IActionResult> UpdateMembershipTypeAsync(int id, MembershipType membershipType)
        {
            if (id != membershipType.Id)
            {
                return BadRequest();
            }

            _context.Entry(membershipType).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MembershipTypeExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpDelete("deleteMembershipType/{id}")]
        public async Task<IActionResult> DeleteMembershipTypeAsync(int id)
        {
            var membershipType = await _context.MembershipTypes.FindAsync(id);
            if (membershipType == null)
            {
                return NotFound();
            }

            _context.MembershipTypes.Remove(membershipType);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        

        // CRUD for Users
        [HttpGet("getUsers")]
        public async Task<ActionResult<IEnumerable<User>>> GetAllUsersAsync()
        {
            return await _context.Users.ToListAsync();
        }

        [HttpDelete("deleteUser/{id}")]
        public async Task<IActionResult> DeleteUserAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPut("updateUser/{id}")]
        public async Task<IActionResult> UpdateUserAsync(int id, User user)
        {
            if (id != user.Id)
            {
                return BadRequest();
            }

            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                {
                    return NotFound(new { message = "User not found for UPDATE" });
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // CRUD for Drivers
        [HttpGet("getDrivers")]
        public async Task<ActionResult<IEnumerable<Driver>>> GetAllDriversAsync()
        {
            return await _context.Drivers.ToListAsync();
        }

        [HttpDelete("deleteDriver/{id}")]
        public async Task<IActionResult> DeleteDriverAsync(int id)
        {
            var driver = await _context.Drivers.FindAsync(id);
            if (driver == null)
            {
                return NotFound();
            }

            _context.Drivers.Remove(driver);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPut("updateDriver/{id}")]
        public async Task<IActionResult> UpdateDriverAsync(int id, Driver driver)
        {
            if (id != driver.Id)
            {
                return BadRequest();
            }

            _context.Entry(driver).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DriverExists(id))
                {
                    return NotFound(new { message = "Driver not found for UPDATE" });
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        

        // CRUD for Companies
        [HttpGet("getCompanies")]
        public async Task<ActionResult<IEnumerable<Company>>> GetAllCompaniesAsync()
        {
            return await _context.Companies.ToListAsync();
        }

        [HttpDelete("deleteCompany/{id}")]
        public async Task<IActionResult> DeleteCompanyAsync(int id)
        {
            var company = await _context.Companies.FindAsync(id);
            if (company == null)
            {
                return NotFound();
            }

            _context.Companies.Remove(company);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPut("updateCompany/{id}")]
        public async Task<IActionResult> UpdateCompanyAsync(int id, [FromForm] Company company)
        {
            if (id != company.Id)
            {
                return BadRequest();
            }

            _context.Entry(company).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CompanyExists(id))
                {
                    return NotFound(new { message = "Company not found for UPDATE" });
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }
        
        // HELPER 
        
        private bool MembershipTypeExists(int id)
        {
            return _context.MembershipTypes.Any(e => e.Id == id);
        }
        
        private bool DriverExists(int id)
        {
            return _context.Drivers.Any(e => e.Id == id);
        }

        private bool CompanyExists(int id)
        {
            return _context.Companies.Any(e => e.Id == id);
        }
        
        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
    }
}
