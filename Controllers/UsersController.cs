
using System;
using Newtonsoft.Json.Linq;
using System.Text.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OpahQueue.Data;
using OpahQueue.Models;
using OpahQueue.Services;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authorization;

using System.IO;
using System.Data;
using ClosedXML.Excel;

namespace OpahQueue.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly DBContext _context;
        private IUserService _userService;

        private static DateTime staticTimeNewPics = DateTime.Now;
        public UsersController(DBContext context, IUserService userService)
        {
            _context = context;
            _userService = userService;
        }

        [HttpPost("authenticate")]
        public IActionResult Authenticate(AuthenticateRequest model)
        {
            var response = _userService.Authenticate(model);
            if (response == null)
                return BadRequest(new { message = "Username or password is incorrect" });
            var user = _context.Users.Find(response.Id);
            AddToQueue(user!);
            return Ok(response);
        }

        [Helpers.Authorize]
        [HttpGet("authorize")]
        public IActionResult GetAll()
        {
            var users = _userService.GetAll();
            return Ok(users);
        }

        [HttpGet("byposition")]
        public async Task<ActionResult<IEnumerable<User>>> GetOrderByPosition()
        {
            return await _context.Users
                .OrderBy(x => x.Position)
                .ToListAsync();
        }

        [HttpGet("size")]
        public async Task<ActionResult<IEnumerable<User>>> GetSizeQueue()
        {
            List<User> meus = await _context.Users.Where(x => x.Position > 0).ToListAsync();
            if (meus == null)
                return NotFound(new { queueSize = 0, message = "fila não existe"});
            return Ok(new { queueSize = meus.Count});
        }

        [HttpGet()]
        public async Task<ActionResult<IEnumerable<User>>> GetUsersLoggedByPosition()
        {
            return await _context.Users
                .Where(x => x.Position > 0)
                .OrderBy(x => x.Position)
                .ToListAsync();
        }
        
        [HttpGet("changes")]
        public async Task<ActionResult<IEnumerable<User>>> GetTest(bool up, int id)
        {
            await _context.SaveChangesAsync();
            return Ok(new {up = up, id = id });
        }
        
        [HttpGet("dtnow/changes")]
        public async Task<ActionResult<IEnumerable<User>>> GetUsersLoggedByPositionDTnow(bool up, int id)
        {
            DTnow(id);
            List<User> meus = await _context.Users.Where(x => x.Position > 0).OrderBy(x => x.Position).ToListAsync();
            if (up)
            {
                staticTimeNewPics = DateTime.Now;
                return Ok(new {list = meus, timeNewPics = staticTimeNewPics.ToString()});
            }
            else
            {
                return Ok(new {list = meus, timeNewPics = staticTimeNewPics.ToString()});
            }
        }

        [HttpGet("dtnow/changes/single")]
        public async Task<ActionResult<IEnumerable<User>>> GetUserPositionDTnowSingle(bool up, long id)
        {
            var userObj = await _context.Users.FindAsync(id);
            if (up)
            {
                staticTimeNewPics = DateTime.Now;
                return Ok(userObj);
            }
            else
            {
                return Ok(userObj);
            }
        }
        
        [HttpGet("excel")]
        public async Task<ActionResult<IEnumerable<User>>> GetUsersTOspreadsheet()
        {
            DataTable dt = new DataTable("Grid");
            dt.Columns.AddRange(new DataColumn[2] {
                                            new DataColumn("Username"),
                                            new DataColumn("Position")
                                            });
            List<User> customers = await _context.Users.Take(10).ToListAsync();
            foreach (var customer in customers)
            {
                dt.Rows.Add(customer.Username, customer.Position);
            }
            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt);
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    Console.WriteLine("TESTE CHEGOU ATÉ AQUI");
                    Console.WriteLine("TESTE CHEGOU ATÉ AQUI");
                    Console.WriteLine("TESTE CHEGOU ATÉ AQUI");
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Grid.xlsx");
                }
            }
        }
        
        [HttpGet("{id}")]
        public async Task<ActionResult<UserDTO>> GetTodoItem(long id)
        {
            if (_context.Users == null)
                return NotFound();
            var todoItem = await _context.Users.FindAsync(id);
            if (todoItem == null)
                return NotFound();
            return Ok(todoItem);
        }
        
        [HttpGet("by/{name}")]
        public async Task<ActionResult<User>> GetTodoItemByName(string name)
        {
            if (_context.Users == null)
                return NotFound();
            var todoItem = await _context.Users.FirstOrDefaultAsync(linq => linq.Name == name);
            if (todoItem == null)
                return NotFound();
            return Ok(todoItem);
        }

        [HttpPost]
        public async Task<ActionResult<User>> CreateTodoItem(User u)
        {
            _context.Users.Add(u);
            await _context.SaveChangesAsync();
            return Ok(ItemToDTO(u));
        }

        private async Task<IActionResult> SaveContext(User u)
        {
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) when (!UserExists(u.Id))
            {
                Console.WriteLine("OCORRÊU UM ERRO. NÃO FOI POSSÍVEL SALVAR");
                return NotFound();
            }
            return NoContent();
        }

        private void Check(int id){
            Console.WriteLine("");
        }

        private void DTnow(int id){
            Console.WriteLine("");
        }
        private User AddToQueue(User u)
        {
            List<User> meus = _context.Users.Where(x => x.Position > 0).ToList();
            var find = _context.Users.Find(u.Id);
            if(find!.Position == 0)
            {
                find!.Position = meus.Count + 1;
            }
            _context.SaveChanges();
            return find;
        }

        private User SetPosition(CustomRequest cr)
        {
            List<User> meus = _context.Users.Where(x => x.Position > 0).OrderBy(x => x.Position).ToList();
            User td = new User();
            int thisPosition = 0;
            bool findPosition = false;
            foreach (User one in meus)
            {
                if(cr.FurarFila == false && cr.Logout == false)
                {
                    if(one.Id == cr.Id)
                    {
                        one.Position = meus.Count;
                        td = one;
                    }
                    else
                        one.Position--;
                }
                if(cr.FurarFila == true && cr.Logout == false)
                {
                    if(findPosition && one.Position > thisPosition)
                        one.Position--;
                    if(one.Id == cr.Id)
                    {
                        findPosition = true;
                        thisPosition = one.Position;
                        one.Position = meus.Count;
                        td = one;
                    }
                }
                if(cr.FurarFila == false && cr.Logout == true)
                {
                    if(findPosition && one.Position > thisPosition)
                        one.Position--;
                    if(one.Id == cr.Id)
                    {
                        findPosition = true;
                        thisPosition = one.Position;
                        one.Position = 0;
                        td = one;
                    }
                }
            }
            return td;
        }
        
        [HttpPut]
        public async Task<IActionResult> ChangePosition(CustomRequest cr)
        {
            var find = await _context.Users.FindAsync(cr.Id);
            User tdR = new User();
            if(find?.Position == 1 || cr.FurarFila == true || cr.Logout == true)
            {
                tdR = SetPosition(cr);
                await SaveContext(tdR);
                return Ok(tdR);
            }
            else
                return Ok(new CustomResponse(find!.Position));
        }

        private void Logout(int id)
        {
        }

        [HttpPut("logout")]
        public async Task<IActionResult> LogoutTask(CustomRequest cr)
        {
            var find = await _context.Users.FindAsync(cr.Id);
            User tdR = new User();
            tdR = SetPosition(cr);
            await SaveContext(find!);
            return Ok(new { position = 0, logout = true});
        }

        [HttpPut("position")]
        public async Task<IActionResult> UpdatePosition(User u)
        {
            var find = await _context.Users.FindAsync(u.Id);
            if (find == null)
            {
                return NotFound("USUÁRIO NÃO ENCONTRADO");
            }
            find!.Position = u.Position;
            await SaveContext(find);
            return Ok(find);
        }

        [Helpers.Authorize]
        [HttpPut("updateUser")]
        public async Task<IActionResult> UpdateUser(User u)
        {
            Console.WriteLine("RECEBIDO");
            Console.WriteLine(u.Id);
            Console.WriteLine(u.Name);
            Console.WriteLine(u.Password);
            var find = await _context.Users.FindAsync(u.Id);
            if (find == null)
                return NotFound();
            if (u.Name != null && u.Name?.Length > 0)
            {
                Console.WriteLine("UP USER NAME");
                find.Name = u.Name;
            }
            if (u.Password != null && u.Password?.Length > 0)
            {
                Console.WriteLine("UP PASS WORD");
                find.Password = u.Password;
            }
            await SaveContext(find);
            if (u.Name == null || u.Password == null)
                Console.WriteLine("N U L L");
            return Ok(find);
        }
        
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTodoItem(long id)
        {
            if (_context.Users == null)
                return NotFound();
            var todoItem = await _context.Users.FindAsync(id);
            if (todoItem == null)
                return NotFound();
            _context.Users.Remove(todoItem);
            await _context.SaveChangesAsync();
            return Ok(todoItem);
        }

        private bool UserExists(long id)
        {
            return (_context.Users?.Any(e => e.Id == id)).GetValueOrDefault();
        }
        
        private static UserDTO ItemToDTO(User todoItem) =>
            new UserDTO
            {
                Id = todoItem.Id,
                Name = todoItem.Name,
                WorkStatus = todoItem.WorkStatus,
                Position = todoItem.Position
            };
        
        private void ConsolePrintOrderBy()
        {
            List<User> meus = _context.Users.ToList();
            var size = meus.Count;
            IEnumerable<User> query = meus.OrderBy(l => l.Position);
            Console.WriteLine("VENDOOOOOOOOOOOOOOOOOOOO");
            foreach (User ql in query)
            {
                Console.WriteLine("{0} - {1}", ql.Name, ql.Position);
            }
            Console.WriteLine("VENDOOOOOOOOOOOOOOOOOOOO");
        }
    }
}
