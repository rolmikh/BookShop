using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API_Book_Shop.Models;

namespace API_Book_Shop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SuppliesController : ControllerBase
    {
        private readonly Book_ShopContext _context;

        public SuppliesController(Book_ShopContext context)
        {
            _context = context;
        }

        // GET: api/Supplies
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Supply>>> GetSupplies()
        {
          if (_context.Supplies == null)
          {
              return NotFound();
          }
            return await _context.Supplies.ToListAsync();
        }

        // GET: api/Supplies/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Supply>> GetSupply(int? id)
        {
          if (_context.Supplies == null)
          {
              return NotFound();
          }
            var supply = await _context.Supplies.FindAsync(id);

            if (supply == null)
            {
                return NotFound();
            }

            return supply;
        }

        // PUT: api/Supplies/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSupply(int? id, Supply supply)
        {
            if (id != supply.IdSupply)
            {
                return BadRequest();
            }

            _context.Entry(supply).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SupplyExists(id))
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

        // POST: api/Supplies
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Supply>> PostSupply(Supply supply)
        {
          if (_context.Supplies == null)
          {
              return Problem("Entity set 'Book_ShopContext.Supplies'  is null.");
          }
            _context.Supplies.Add(supply);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetSupply", new { id = supply.IdSupply }, supply);
        }

        // DELETE: api/Supplies/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSupply(int? id)
        {
            if (_context.Supplies == null)
            {
                return NotFound();
            }
            var supply = await _context.Supplies.FindAsync(id);
            if (supply == null)
            {
                return NotFound();
            }

            _context.Supplies.Remove(supply);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool SupplyExists(int? id)
        {
            return (_context.Supplies?.Any(e => e.IdSupply == id)).GetValueOrDefault();
        }
    }
}
