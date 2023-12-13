using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using pdm.Data;
using pdm.Models;

namespace pdm.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservationsController : ControllerBase
    {
        private readonly PDMContext _context;

        public ReservationsController(PDMContext context)
        {
            _context = context;
        }

        // GET: api/Reservations
        [HttpGet, Authorize(Roles = "Admin,Membre")]
        public async Task<ActionResult<IEnumerable<Reservation>>> GetReservation()
        {
            if (_context.Reservation == null)
            {
              return NotFound();
            }
            return await _context.Reservation
                .Include(r => r.User)
                .Include(r => r.Vehicule)
                .Select(r => new Reservation
                {
                    Id = r.Id,
                    Date = r.Date,
                    VehiculeId = r.VehiculeId,
                    Vehicule = r.Vehicule,
                    UserId = r.UserId,
                    User = r.User != null ? new User
                    {
                        Id = r.User.Id,
                        Firstname = r.User.Firstname,
                        Lastname = r.User.Lastname,
                        Email = r.User.Email,
                    } : null
                })
                .ToListAsync();
        }

        // GET: api/Reservations/5
        [HttpGet("{id}"), Authorize(Roles = "Admin,Membre")]
        public async Task<ActionResult<Reservation>> GetReservation(int id)
        {
            if (_context.Reservation == null)
            {
              return NotFound();
            }
            var reservation = await _context.Reservation.FindAsync(id);

            if (reservation == null)
            {
                return NotFound();
            }

            return reservation;
        }
        
        
        // GET: api/Reservations/5
        [HttpGet("user/{id}"), Authorize(Roles = "Admin,Membre")]
        public async Task<ActionResult<IEnumerable<Reservation>>> GetReservationsByUser(int id)
        {
            if (_context.Reservation == null)
            {
                return NotFound();
            }
            var reservations = await _context.Reservation.Where(r => r.User.Id == id).OrderBy(r => r.Date).ToListAsync();

            if (reservations == null)
            {
                return NotFound();
            }

            return reservations;
        }

        // PUT: api/Reservations/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}"), Authorize(Roles = "Admin,Membre")]
        public async Task<IActionResult> PutReservation(long id, Reservation reservation)
        {
            if (id != reservation.Id)
            {
                return BadRequest();
            }

            _context.Entry(reservation).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ReservationExists(id))
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

        // POST: api/Reservations
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost, Authorize(Roles = "Admin,Membre")]
        public async Task<ActionResult<Reservation>> PostReservation(Reservation reservation)
        {
            if (_context.Reservation == null)
            {
              return Problem("Entity set 'PremiumDeluxeMotorSports_v1Context.Reservation'  is null.");
            }
            _context.Reservation.Add(reservation);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetReservation", new { id = reservation.Id }, reservation);
        }

        // DELETE: api/Reservations/5
        [HttpDelete("{id}"), Authorize(Roles = "Admin,Membre")]
        public async Task<IActionResult> DeleteReservation(int id)
        {
            if (_context.Reservation == null)
            {
                return NotFound();
            }
            var reservation = await _context.Reservation.FindAsync(id);
            if (reservation == null)
            {
                return NotFound();
            }

            _context.Reservation.Remove(reservation);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ReservationExists(long id)
        {
            return (_context.Reservation?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
