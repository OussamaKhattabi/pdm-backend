using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Common;
using PremiumDeluxeMotorSports_v1.Data;
using PremiumDeluxeMotorSports_v1.Models;
using PremiumDeluxeMotorSports_v1.Services;

namespace PremiumDeluxeMotorSports_v1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomsController : ControllerBase
    {
        private readonly PremiumDeluxeMotorSports_v1Context _context;

        public CustomsController(PremiumDeluxeMotorSports_v1Context context)
        {
            _context = context;
        }

        // GET: api/Customs
        [HttpGet, Authorize(Roles = "Admin,Membre")]
        public async Task<ActionResult<IEnumerable<Custom>>> GetCustom()
        {

            if (_context.Custom == null)
              {
                  return NotFound();
              }
            return await _context.Custom.ToListAsync();
        }

        // GET: api/Customs/5
        [HttpGet("{id}"), Authorize(Roles = "Admin,Membre")]
        public async Task<ActionResult<Custom>> GetCustom(int id)
        {
          if (_context.Custom == null)
          {
              return NotFound();
          }
            var custom = await _context.Custom.FindAsync(id);

            if (custom == null)
            {
                return NotFound();
            }

            return custom;
        }

        // PUT: api/Customs/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}"), Authorize(Roles = "Admin,Membre")]
        public async Task<IActionResult> PutCustom(int id, Custom custom)
        {
            if (id != custom.CustomId)
            {
                return BadRequest();
            }

            _context.Entry(custom).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CustomExists(id))
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

        // POST: api/Customs
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost, Authorize(Roles = "Admin,Membre")]
        public async Task<ActionResult<Custom>> PostCustom(Custom custom)
        {
            if (_context.Custom == null)
          {
              return Problem("Entity set 'PremiumDeluxeMotorSports_v1Context.Custom'  is null.");
          }
            _context.Custom.Add(custom);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCustom", new { id = custom.CustomId }, custom);
        }

        // DELETE: api/Customs/5
        [HttpDelete("{id}"), Authorize(Roles = "Admin,Membre")]
        public async Task<IActionResult> DeleteCustom(int id)
        {
            if (_context.Custom == null)
            {
                return NotFound();
            }
            var custom = await _context.Custom.FindAsync(id);
            if (custom == null)
            {
                return NotFound();
            }

            _context.Custom.Remove(custom);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CustomExists(int id)
        {
            return (_context.Custom?.Any(e => e.CustomId == id)).GetValueOrDefault();
        }
    }
}
