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
    public class StatusOrdersController : ControllerBase
    {
        private readonly Book_ShopContext _context;

        public StatusOrdersController(Book_ShopContext context)
        {
            _context = context;
        }

        // GET: api/StatusOrders
        [HttpGet]
        public async Task<ActionResult<IEnumerable<StatusOrder>>> GetStatusOrders()
        {
          if (_context.StatusOrders == null)
          {
              return NotFound();
          }
            return await _context.StatusOrders.ToListAsync();
        }

        // GET: api/StatusOrders/5
        [HttpGet("{id}")]
        public async Task<ActionResult<StatusOrder>> GetStatusOrder(int? id)
        {
          if (_context.StatusOrders == null)
          {
              return NotFound();
          }
            var statusOrder = await _context.StatusOrders.FindAsync(id);

            if (statusOrder == null)
            {
                return NotFound();
            }

            return statusOrder;
        }

        // PUT: api/StatusOrders/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutStatusOrder(int? id, StatusOrder statusOrder)
        {
            if (id != statusOrder.IdStatusOrder)
            {
                return BadRequest();
            }

            _context.Entry(statusOrder).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StatusOrderExists(id))
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

        // POST: api/StatusOrders
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<StatusOrder>> PostStatusOrder(StatusOrder statusOrder)
        {
          if (_context.StatusOrders == null)
          {
              return Problem("Entity set 'Book_ShopContext.StatusOrders'  is null.");
          }
            _context.StatusOrders.Add(statusOrder);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetStatusOrder", new { id = statusOrder.IdStatusOrder }, statusOrder);
        }

        // DELETE: api/StatusOrders/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStatusOrder(int? id)
        {
            if (_context.StatusOrders == null)
            {
                return NotFound();
            }
            var statusOrder = await _context.StatusOrders.FindAsync(id);
            if (statusOrder == null)
            {
                return NotFound();
            }

            _context.StatusOrders.Remove(statusOrder);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool StatusOrderExists(int? id)
        {
            return (_context.StatusOrders?.Any(e => e.IdStatusOrder == id)).GetValueOrDefault();
        }
    }
}
