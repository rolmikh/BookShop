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
    public class OrderCompositionsController : ControllerBase
    {
        private readonly Book_ShopContext _context;

        public OrderCompositionsController(Book_ShopContext context)
        {
            _context = context;
        }

        // GET: api/OrderCompositions
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderComposition>>> GetOrderCompositions()
        {
          if (_context.OrderCompositions == null)
          {
              return NotFound();
          }
            return await _context.OrderCompositions.ToListAsync();
        }

        // GET: api/OrderCompositions/5
        [HttpGet("{id}")]
        public async Task<ActionResult<OrderComposition>> GetOrderComposition(int? id)
        {
          if (_context.OrderCompositions == null)
          {
              return NotFound();
          }
            var orderComposition = await _context.OrderCompositions.FindAsync(id);

            if (orderComposition == null)
            {
                return NotFound();
            }

            return orderComposition;
        }

        // PUT: api/OrderCompositions/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOrderComposition(int? id, OrderComposition orderComposition)
        {
            if (id != orderComposition.IdOrderComposition)
            {
                return BadRequest();
            }

            _context.Entry(orderComposition).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderCompositionExists(id))
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

        // POST: api/OrderCompositions
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<OrderComposition>> PostOrderComposition(OrderComposition orderComposition)
        {
          if (_context.OrderCompositions == null)
          {
              return Problem("Entity set 'Book_ShopContext.OrderCompositions'  is null.");
          }
            _context.OrderCompositions.Add(orderComposition);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetOrderComposition", new { id = orderComposition.IdOrderComposition }, orderComposition);
        }

        // DELETE: api/OrderCompositions/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrderComposition(int? id)
        {
            if (_context.OrderCompositions == null)
            {
                return NotFound();
            }
            var orderComposition = await _context.OrderCompositions.FindAsync(id);
            if (orderComposition == null)
            {
                return NotFound();
            }

            _context.OrderCompositions.Remove(orderComposition);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool OrderCompositionExists(int? id)
        {
            return (_context.OrderCompositions?.Any(e => e.IdOrderComposition == id)).GetValueOrDefault();
        }
    }
}
