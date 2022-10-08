
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OpahQueue.Data;
using OpahQueue.Models;

namespace OpahQueue.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProtocolsController : ControllerBase
    {
        private readonly DBContext _context;

        public ProtocolsController(DBContext context)
        {
            _context = context;
        }
        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Exceller>>> GetExceller()
        {
            return await _context.Exceller.ToListAsync();
        }
        
        [HttpGet("{id}")]
        public async Task<ActionResult<Exceller>> GetExceller(int id)
        {
            var exceller = await _context.Exceller.FindAsync(id);
            if (exceller == null)
            {
                return NotFound();
            }
            return exceller;
        }
        
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutExceller(int id, Exceller exceller)
        {
            if (id != exceller.Id)
            {
                return BadRequest();
            }
            _context.Entry(exceller).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ExcellerExists(id))
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
        
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Exceller>> PostExceller(Exceller exceller)
        {
            _context.Exceller.Add(exceller);
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetExceller", new { id = exceller.Id }, exceller);
        }
        
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteExceller(int id)
        {
            var exceller = await _context.Exceller.FindAsync(id);
            if (exceller == null)
            {
                return NotFound();
            }
            _context.Exceller.Remove(exceller);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        private bool ExcellerExists(int id)
        {
            return _context.Exceller.Any(e => e.Id == id);
        }
    }
}
