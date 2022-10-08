
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OpahQueue.Data;
using OpahQueue.Models;

using System.IO;
using System.Data;
// using System.Linq;
using ClosedXML.Excel;

namespace OpahQueue.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CallsController : ControllerBase
    {
        private readonly DBContext _context;
        public CallsController(DBContext context)
        {
            _context = context;
        }
        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TIatende>>> GetTIatende()
        {
            return await _context.TIatende.ToListAsync();
        }
        
        [HttpGet("{id}")]
        public async Task<ActionResult<TIatende>> GetTIatende(int id)
        {
            var tIatende = await _context.TIatende.FindAsync(id);
            if (tIatende == null)
            {
                return NotFound();
            }
            return tIatende;
        }
        
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTIatende(int id, TIatende tIatende)
        {
            if (id != tIatende.Id)
            {
                return BadRequest();
            }
            _context.Entry(tIatende).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TIatendeExists(id))
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
        public async Task<ActionResult<TIatende>> PostTIatende(TIatende tIatende)
        {
            _context.TIatende.Add(tIatende);
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetTIatende", new { id = tIatende.Id }, tIatende);
        }

        [HttpGet("ti")]
        public async Task<ActionResult<IEnumerable<User>>> GetCallsTOspreadsheet()
        {
            DataTable dt = new DataTable("Grid");
            dt.Columns.AddRange(new DataColumn[3] {
                                            new DataColumn("Called"),
                                            new DataColumn("Datetime"),
                                            new DataColumn("UserId")
                                            });
            // var customers = from customer in this._context.TIatende.Take(10)
            //                 select customer;
            // var customers = from customer in this._context.TIatende.Where(x => x.Datetime.Date == DateTime.Now.Date).ToList()
            //                 select customer;
            List<TIatende> customers = await _context.TIatende.Where(x => x.Datetime.Date == DateTime.Now.Date).ToListAsync();
            foreach (var customer in customers)
            {
                dt.Rows.Add(customer.Called, customer.Datetime, customer.UserId);
                Console.WriteLine(""+ customer.Called + "   " + customer.UserId);
            }
            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt);
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Grid.xlsx");
                }
            }            
        }
        
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTIatende(int id)
        {
            var tIatende = await _context.TIatende.FindAsync(id);
            if (tIatende == null)
            {
                return NotFound();
            }
            _context.TIatende.Remove(tIatende);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        private bool TIatendeExists(int id)
        {
            return _context.TIatende.Any(e => e.Id == id);
        }
    }
}
