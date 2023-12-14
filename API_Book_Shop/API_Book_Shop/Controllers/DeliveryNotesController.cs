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
    public class DeliveryNotesController : ControllerBase
    {
        private readonly Book_ShopContext _context;

        public DeliveryNotesController(Book_ShopContext context)
        {
            _context = context;
        }

        // GET: api/DeliveryNotes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DeliveryNote>>> GetDeliveryNotes()
        {
          if (_context.DeliveryNotes == null)
          {
              return NotFound();
          }
            return await _context.DeliveryNotes.ToListAsync();
        }

        // GET: api/DeliveryNotes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<DeliveryNote>> GetDeliveryNote(int? id)
        {
          if (_context.DeliveryNotes == null)
          {
              return NotFound();
          }
            var deliveryNote = await _context.DeliveryNotes.FindAsync(id);

            if (deliveryNote == null)
            {
                return NotFound();
            }

            return deliveryNote;
        }

        // PUT: api/DeliveryNotes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDeliveryNote(int? id, DeliveryNote deliveryNote)
        {
            if (id != deliveryNote.IdDeliveryNote)
            {
                return BadRequest();
            }

            _context.Entry(deliveryNote).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DeliveryNoteExists(id))
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

        // POST: api/DeliveryNotes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<DeliveryNote>> PostDeliveryNote(DeliveryNote deliveryNote)
        {
          if (_context.DeliveryNotes == null)
          {
              return Problem("Entity set 'Book_ShopContext.DeliveryNotes'  is null.");
          }
            _context.DeliveryNotes.Add(deliveryNote);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDeliveryNote", new { id = deliveryNote.IdDeliveryNote }, deliveryNote);
        }

        // DELETE: api/DeliveryNotes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDeliveryNote(int? id)
        {
            if (_context.DeliveryNotes == null)
            {
                return NotFound();
            }
            var deliveryNote = await _context.DeliveryNotes.FindAsync(id);
            if (deliveryNote == null)
            {
                return NotFound();
            }

            _context.DeliveryNotes.Remove(deliveryNote);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool DeliveryNoteExists(int? id)
        {
            return (_context.DeliveryNotes?.Any(e => e.IdDeliveryNote == id)).GetValueOrDefault();
        }
    }
}
