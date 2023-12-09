using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Common;
using pdm.Data;
using pdm.Models;
using pdm.Services;

namespace pdm.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommandesController : ControllerBase
    {
        private readonly PremiumDeluxeMotorSports_v1Context _context;

        public CommandesController(PremiumDeluxeMotorSports_v1Context context)
        {
            _context = context;
        }

        // GET: api/Commandes
        [HttpGet, Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<Commande>>> GetCommande()
        {
            if (_context.Commande == null)
          {
              return NotFound();
          }
            return await _context.Commande
                .Include(c => c.User)
                .Include(c => c.Vehicule)
                .Include(c => c.Custom)
                .Select(c => new Commande
                {
                    CmdId = c.CmdId,
                    Date_Cmd = c.Date_Cmd,
                    CustomId = c.CustomId,
                    Custom = c.Custom,
                    VehiculeId = c.VehiculeId,
                    Vehicule = c.Vehicule,
                    UserId = c.UserId,
                    User = c.User != null ? new User
                    {
                        UserID = c.User.UserID,
                        UserFirstName = c.User.UserFirstName,
                        UserLastName = c.User.UserLastName,
                        UserEmail = c.User.UserEmail,
                    } : null
                })
                .ToListAsync();
        }

        // GET: api/Commandes/5
        [HttpGet("{id}"), Authorize(Roles = "Admin,Membre")]
        public async Task<ActionResult<Commande>> GetCommande(int id)
        {
            if (_context.Commande == null)
          {
              return NotFound();
          }
            var commande = await _context.Commande.FindAsync(id);

            if (commande == null)
            {
                return NotFound();
            }

            return commande;
        }

        // PUT: api/Commandes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}"), Authorize(Roles = "Admin,Membre")]
        public async Task<IActionResult> PutCommande(int id, Commande commande)
        {
            if (id != commande.CmdId)
            {
                return BadRequest();
            }

            _context.Entry(commande).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CommandeExists(id))
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

        // POST: api/Commandes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost, Authorize(Roles = "Admin,Membre")]
        public async Task<ActionResult<Commande>> PostCommande(Commande commande)
        {
           if(commande.Custom != null)
            {
                if(commande.Custom.CustomId > 0)
                {
                    _context.Commande.Add(commande);
                }
                else
                {
                    commande.Custom = null;
                }
            }

          if (_context.Commande == null)
          {
              return Problem("Entity set 'PremiumDeluxeMotorSports_v1Context.Commande'  is null.");
          }

            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCommande", new { id = commande.CmdId }, commande);
        }

        // DELETE: api/Commandes/5
        [HttpDelete("{id}"), Authorize(Roles = "Admin,Membre")]
        public async Task<IActionResult> DeleteCommande(int id)
        {

            if (_context.Commande == null)
            {
                return NotFound();
            }
            var commande = await _context.Commande.FindAsync(id);
            if (commande == null)
            {
                return NotFound();
            }

            _context.Commande.Remove(commande);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CommandeExists(int id)
        {
            return (_context.Commande?.Any(e => e.CmdId == id)).GetValueOrDefault();
        }
    }
}
