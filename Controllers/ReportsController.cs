
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
using System.Collections;

namespace OpahQueue.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportsController : ControllerBase
    {
        private readonly DBContext _context;
        private static List<Report> slr = new List<Report>();
        private static DateTime dtReport;
        private static bool dtInit = false;
        public ReportsController(DBContext context)
        {
            _context = context;
        }
        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Report>>> GetReport()
        {            // return await _context.Report.ToListAsync();
            List<Report> reports = await _context.Report.ToListAsync();
            ArrayList objJson = new ArrayList(); // ASSIM NÃO PRECISA DECLARAR TIPO COMO NA LIST AQUI EM CIMA
            foreach (Report report in reports)
            {
                objJson.Add(new {name = report.Name, tIatendeDaily = report.TIatendeDaily, tIatendeMonthly = report.TIatendeMonthly, excellerDaily = report.ExcellerDaily, excellerMonthly = report.ExcellerMonthly });
            }
            return Ok(objJson);
        }

        [HttpGet("fullReport")]
        public async Task<ActionResult<Report>> GetReportMonthly()
        {
            List<Report> l = new List<Report>();
            var users = await _context.Users.Where(x => x.UserStatus == "ativo").ToListAsync();

            foreach (var item in users)
            {
                Report r = new Report();
                var monthlyTI = await _context.TIatende.Where(x => x.UserId == item.Id).ToListAsync();
                var dailyTI = await _context.TIatende.Where(x => x.UserId == item.Id && x.Datetime.Date == DateTime.Now.Date).ToListAsync();
                // se esta WEBAPI estiver hospedada em um servidor estrangeiro, pode ser que DateTime.Now.Date falhe ou tenha resposta errada por causa do fuso horário.
                var monthlyEX = await _context.Exceller.Where(x => x.UserId == item.Id).ToListAsync();
                var dailyEX = await _context.Exceller.Where(x => x.UserId == item.Id && x.Datetime.Date == DateTime.Now.Date).ToListAsync();
                // se esta WEBAPI estiver hospedada em um servidor estrangeiro, pode ser que DateTime.Now.Date falhe ou tenha resposta errada por causa do fuso horário.
                Console.WriteLine("TI ATENDE");
                Console.WriteLine("chamados de mensais " + item.Username + " = " + monthlyTI.Count);
                Console.WriteLine("chamados de diários " + item.Username + " = " + dailyTI.Count);
                Console.WriteLine("EXCELLER");
                Console.WriteLine("chamados de mensais " + item.Username + " = " + monthlyEX.Count);
                Console.WriteLine("chamados de diários " + item.Username + " = " + dailyEX.Count);
                r.UserId = item.Id;
                r.Name = item.Name;
                r.TIatendeDaily = dailyTI.Count;
                r.TIatendeMonthly = monthlyTI.Count;
                r.ExcellerDaily = dailyEX.Count;
                r.ExcellerMonthly = monthlyEX.Count;
                l.Add(r);
            }
            slr = l;
            dtInit = true;
            dtReport = DateTime.Now;
            return Ok(l);
        }

        [HttpGet("exportFullReport")]
        public async Task<ActionResult<Report>> GetExportReportMonthly()
        {
            if (dtInit)
            {
                Console.WriteLine("JÁ FEZ E VAI RETORNAR");
                Console.WriteLine(dtReport);    // RECUPERA LISTA STATICA E RETORNA NA RESPONSE
            }
            else
            {
                await GetReportMonthly();       // SE NÃO TIVER LISTA STATICA, CHAMA O OUTRO METODO
            }
            DataTable dt = new DataTable("Grid");
            dt.Columns
                .AddRange(new DataColumn[7] {
                    new DataColumn("UserId"),
                    new DataColumn("Name"),
                    new DataColumn("TIatendeDaily"),
                    new DataColumn("TIatendeMonthly"),
                    new DataColumn("ExcellerDaily"),
                    new DataColumn("ExcellerMonthly"),
                    new DataColumn("Datetime")
            });
            foreach (var item in slr)
            {
                dt.Rows.Add(item.UserId, item.Name, item.TIatendeDaily, item.TIatendeMonthly, item.ExcellerDaily, item.ExcellerMonthly, item.Datetime);
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
        
        [HttpGet("{id}")]
        public async Task<ActionResult<Report>> GetReport(int id)
        {
            var report = await _context.Report.FindAsync(id);
            if (report == null)
            {
                return NotFound();
            }
            return report;
        }
        
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("countReport")]
        public async Task<IActionResult> PutReport(long UserId, bool TI, bool ex)
        {
            var cr = await _context.Report.FirstOrDefaultAsync(x => x.UserId == UserId);
            Report nr = new Report();
            if (cr == null)
            {
                var user = await _context.Users.FindAsync(UserId);
                nr.UserId = user!.Id;
                nr.Name = user!.Name;
                nr.Datetime = DateTime.Now;
                if (TI)
                {
                    nr.TIatendeDaily++;
                    nr.TIatendeMonthly++;
                }
                if (ex)
                {
                    nr.ExcellerDaily++;
                    nr.ExcellerMonthly++;
                }
                await PostReport(nr);
                return Ok(nr);
            }
            else
            {
                DateTime agora = DateTime.Now.Date;
                DateTime rdt = cr.Datetime.Date;
                int ir = (agora - rdt).Days;
                if (ir > 0)
                {
                    cr.TIatendeDaily = 0;
                    cr.ExcellerDaily = 0;
                }
                if (ir > 30)
                {
                    cr.TIatendeMonthly = 0;
                    cr.ExcellerMonthly = 0;
                    cr.Datetime = DateTime.Now;
                }
                if (TI)
                {
                    cr.TIatendeDaily++;
                    cr.TIatendeMonthly++;
                }
                if (ex)
                {
                    cr.ExcellerDaily++;
                    cr.ExcellerMonthly++;
                }
                await _context.SaveChangesAsync();
                return Ok(cr);
            }            // return NoContent();
        }
        
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Report>> PostReport(Report report)
        {
            _context.Report.Add(report);
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetReport", new { id = report.Id }, report);
        }
        
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReport(int id)
        {
            var report = await _context.Report.FindAsync(id);
            if (report == null)
            {
                return NotFound();
            }
            _context.Report.Remove(report);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        private bool ReportExists(int id)
        {
            return _context.Report.Any(e => e.Id == id);
        }
    }
}
