﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using pdm.Data;
using pdm.Models;

namespace pdm.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VehiculesController : ControllerBase
    {
        private readonly PDMContext _context;

        public VehiculesController(PDMContext context)
        {
            _context = context;
        }

        // GET: api/Vehicules
        [HttpGet] // Authorize(Roles = "Admin,Membre")]
        public async Task<ActionResult<IEnumerable<Vehicule>>> GetVehicule()
        {
          if (_context.Vehicule == null)
          {
              return NotFound();
          }
            return await _context.Vehicule.ToListAsync();
        }

        // GET: api/Vehicules/5
        [HttpGet("{id}"), Authorize(Roles = "Admin,Membre")]
        public async Task<ActionResult<Vehicule>> GetVehicule(int id)
        {
            if (_context.Vehicule == null)
          {
              return NotFound();
          }
            var vehicule = await _context.Vehicule.FindAsync(id);

            if (vehicule == null)
            {
                return NotFound();
            }

            return vehicule;
        }

        // PUT: api/Vehicules/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> PutVehicule(int id, Vehicule vehicule)
        {
            if (id != vehicule.VehiculeId)
            {
                return BadRequest();
            }

            _context.Entry(vehicule).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VehiculeExists(id))
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

        // POST: api/Vehicules
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost] //, Authorize(Roles = "Admin")]
        public async Task<ActionResult<Vehicule>> PostVehicule([FromForm] string Marque,[FromForm] string Model, [FromForm] int Prix, IFormFile? file) {
            var vehicule = new Vehicule
            {
                Marque = Marque,
                Model = Model,
                Prix = Prix
            };
            
            if (_context.Vehicule == null)
            {
                return NotFound();
            }
            if (file != null)
            {
                var fileName = $"{Guid.NewGuid().ToString() + System.IO.Path.GetExtension(file.FileName)}";
                var filePath = System.IO.Path.Combine("C:\\Users\\yassi\\Desktop\\projet-tei4-premiumdeluxemotorsport-frontend\\src\\assets\\images", fileName);
                using (var fileStream = new System.IO.FileStream(filePath, System.IO.FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }
                
                // Supprimer tout ce qui se trouve avant "assets"
                int srcIndex = filePath.IndexOf("assets");
                if (srcIndex != -1)
                {
                    filePath = $"{filePath.Substring(srcIndex)}";
                }
                
                vehicule.Image = filePath;
            } else {
                vehicule.Image = "default.jpg";
            }
            
            _context.Vehicule.Add(vehicule);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetVehicule", new { id = vehicule.VehiculeId }, vehicule);
        }

        // DELETE: api/Vehicules/5
        [HttpDelete("{id}"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteVehicule(int id)
        {
            if (_context.Vehicule == null)
            {
                return NotFound();
            }
            var vehicule = await _context.Vehicule.FindAsync(id);
            if (vehicule == null)
            {
                return NotFound();
            }

            _context.Vehicule.Remove(vehicule);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool VehiculeExists(int id)
        {
            return (_context.Vehicule?.Any(e => e.VehiculeId == id)).GetValueOrDefault();
        }
    }
}
