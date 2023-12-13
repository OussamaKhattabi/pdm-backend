using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using pdm.Data;
using pdm.Models;

namespace pdm.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommandesController : ControllerBase
    {
        private readonly PDMContext _context;

        public CommandesController(PDMContext context)
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
                    Id = c.Id,
                    Date= c.Date,
                    CustomId = c.CustomId,
                    Custom = c.Custom,
                    VehiculeId = c.VehiculeId,
                    Vehicule = c.Vehicule,
                    UserId = c.UserId,
                    User = c.User != null ? new User
                    {
                        Id = c.User.Id,
                        Firstname = c.User.Firstname,
                        Lastname = c.User.Lastname,
                        Email = c.User.Email,
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
            if (id != commande.Id)
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
            
           if (_context.Commande == null)
           {
              return Problem("Entity set 'PremiumDeluxeMotorSports_v1Context.Commande'  is null.");
           }

           _context.Commande.Add(commande);
           await _context.SaveChangesAsync();

            return CreatedAtAction("GetCommande", new { id = commande.Id }, commande);
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
            return (_context.Commande?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
