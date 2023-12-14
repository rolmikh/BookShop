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
    public class SupplyCompositionsController : ControllerBase
    {
        private readonly Book_ShopContext _context;

        public SupplyCompositionsController(Book_ShopContext context)
        {
            _context = context;
        }

        // GET: api/SupplyCompositions
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SupplyComposition>>> GetSupplyCompositions()
        {
          if (_context.SupplyCompositions == null)
          {
              return NotFound();
          }
            return await _context.SupplyCompositions.ToListAsync();
        }

        // GET: api/SupplyCompositions/5
        [HttpGet("{id}")]
        public async Task<ActionResult<SupplyComposition>> GetSupplyComposition(int? id)
        {
          if (_context.SupplyCompositions == null)
          {
              return NotFound();
          }
            var supplyComposition = await _context.SupplyCompositions.FindAsync(id);

            if (supplyComposition == null)
            {
                return NotFound();
            }

            return supplyComposition;
        }

        // PUT: api/SupplyCompositions/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSupplyComposition(int? id, SupplyComposition supplyComposition)
        {
            if (id != supplyComposition.IdSupplyComposition)
            {
                return BadRequest();
            }

            _context.Entry(supplyComposition).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SupplyCompositionExists(id))
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

        // POST: api/SupplyCompositions
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<SupplyComposition>> PostSupplyComposition(SupplyComposition supplyComposition)
        {
          if (_context.SupplyCompositions == null)
          {
              return Problem("Entity set 'Book_ShopContext.SupplyCompositions'  is null.");
          }
            _context.SupplyCompositions.Add(supplyComposition);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetSupplyComposition", new { id = supplyComposition.IdSupplyComposition }, supplyComposition);
        }

        // DELETE: api/SupplyCompositions/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSupplyComposition(int? id)
        {
            if (_context.SupplyCompositions == null)
            {
                return NotFound();
            }
            var supplyComposition = await _context.SupplyCompositions.FindAsync(id);
            if (supplyComposition == null)
            {
                return NotFound();
            }

            _context.SupplyCompositions.Remove(supplyComposition);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool SupplyCompositionExists(int? id)
        {
            return (_context.SupplyCompositions?.Any(e => e.IdSupplyComposition == id)).GetValueOrDefault();
        }
    }
}
