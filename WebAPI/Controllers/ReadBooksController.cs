using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPI.Model;
using WebAPI.Repository;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReadBooksController : ControllerBase
    {
        private readonly DBConnection _context;

        public ReadBooksController(DBConnection context)
        {
            _context = context;
        }

        // GET: api/ReadBooks
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ReadBooks>>> GetReadBooks()
        {
            return await _context.ReadBooks.ToListAsync();
        }

        // GET: api/ReadBooks/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ReadBooks>> GetReadBooks(int id)
        {
            var readBooks = await _context.ReadBooks.FindAsync(id);

            if (readBooks == null)
            {
                return NotFound();
            }

            return readBooks;
        }

        // PUT: api/ReadBooks/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutReadBooks(int id, ReadBooks readBooks)
        {
            if (id != readBooks.Id)
            {
                return BadRequest();
            }

            _context.Entry(readBooks).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ReadBooksExists(id))
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

        // POST: api/ReadBooks
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ReadBooks>> PostReadBooks(ReadBooks readBooks)
        {
            _context.ReadBooks.Add(readBooks);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetReadBooks", new { id = readBooks.Id }, readBooks);
        }

        // DELETE: api/ReadBooks/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReadBooks(int id)
        {
            var readBooks = await _context.ReadBooks.FindAsync(id);
            if (readBooks == null)
            {
                return NotFound();
            }

            _context.ReadBooks.Remove(readBooks);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ReadBooksExists(int id)
        {
            return _context.ReadBooks.Any(e => e.Id == id);
        }
    }
}
