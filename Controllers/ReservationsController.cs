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
    public class ReservationsController : ControllerBase
    {
        private readonly PremiumDeluxeMotorSports_v1Context _context;

        public ReservationsController(PremiumDeluxeMotorSports_v1Context context)
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
                    IdReservation = r.IdReservation,
                    DateReservation = r.DateReservation,
                    VehiculeId = r.VehiculeId,
                    Vehicule = r.Vehicule,
                    UserId = r.UserId,
                    User = r.User != null ? new User
                    {
                        UserID = r.User.UserID,
                        UserFirstName = r.User.UserFirstName,
                        UserLastName = r.User.UserLastName,
                        UserEmail = r.User.UserEmail,
                    } : null
                })
                .ToListAsync();
        }

        // GET: api/Reservations/5
        [HttpGet("{id}"), Authorize(Roles = "Admin,Membre")]
        public async Task<ActionResult<Reservation>> GetReservation(long id)
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

        // PUT: api/Reservations/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}"), Authorize(Roles = "Admin,Membre")]
        public async Task<IActionResult> PutReservation(long id, Reservation reservation)
        {
            if (id != reservation.IdReservation)
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

            return CreatedAtAction("GetReservation", new { id = reservation.IdReservation }, reservation);
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
            return (_context.Reservation?.Any(e => e.IdReservation == id)).GetValueOrDefault();
        }
    }
}
