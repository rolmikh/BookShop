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
    public class CustomerCardsController : ControllerBase
    {
        private readonly Book_ShopContext _context;

        public CustomerCardsController(Book_ShopContext context)
        {
            _context = context;
        }

        // GET: api/CustomerCards
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CustomerCard>>> GetCustomerCards()
        {
          if (_context.CustomerCards == null)
          {
              return NotFound();
          }
            return await _context.CustomerCards.ToListAsync();
        }

        // GET: api/CustomerCards/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CustomerCard>> GetCustomerCard(int? id)
        {
          if (_context.CustomerCards == null)
          {
              return NotFound();
          }
            var customerCard = await _context.CustomerCards.FindAsync(id);

            if (customerCard == null)
            {
                return NotFound();
            }

            return customerCard;
        }

        // PUT: api/CustomerCards/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCustomerCard(int? id, CustomerCard customerCard)
        {
            if (id != customerCard.IdCustomerCard)
            {
                return BadRequest();
            }

            _context.Entry(customerCard).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CustomerCardExists(id))
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

        // POST: api/CustomerCards
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<CustomerCard>> PostCustomerCard(CustomerCard customerCard)
        {
          if (_context.CustomerCards == null)
          {
              return Problem("Entity set 'Book_ShopContext.CustomerCards'  is null.");
          }
            _context.CustomerCards.Add(customerCard);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCustomerCard", new { id = customerCard.IdCustomerCard }, customerCard);
        }

        // DELETE: api/CustomerCards/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomerCard(int? id)
        {
            if (_context.CustomerCards == null)
            {
                return NotFound();
            }
            var customerCard = await _context.CustomerCards.FindAsync(id);
            if (customerCard == null)
            {
                return NotFound();
            }

            _context.CustomerCards.Remove(customerCard);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CustomerCardExists(int? id)
        {
            return (_context.CustomerCards?.Any(e => e.IdCustomerCard == id)).GetValueOrDefault();
        }
    }
}
