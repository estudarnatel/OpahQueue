using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using OpahQueue.Data;
using OpahQueue.Helpers;
using OpahQueue.Models;

namespace OpahQueue.Services
{

    public interface IUserService
    {
        AuthenticateResponse Authenticate(AuthenticateRequest model);
        IEnumerable<User> GetAll();
        User GetById(int id);
    }


    public class UserService : IUserService
    {
        // users hardcoded for simplicity, store in a db with hashed passwords in production applications
        // private List<User> _users = new List<User>
        // {
        //     // new User { Id = 1, FirstName = "Test", LastName = "User", Username = "test", Password = "test" }
        //     new User { Id = 1, Name = "Test", IsComplete = false, Secret = "test", Position = 0 }
        // };

        private readonly DBContext _context;

        private readonly AppSettings _appSettings;

        public UserService(IOptions<AppSettings> appSettings, DBContext context)
        {
            _appSettings = appSettings.Value;
            _context = context;
        }

        public AuthenticateResponse Authenticate(AuthenticateRequest model)
        {
            // var user = _users.SingleOrDefault(x => x.Username == model.Username && x.Password == model.Password);
            // var user = _users.SingleOrDefault(x => x.Name == model.Username && x.Secret == model.Password);
            // var user = _users.SingleOrDefault(x => x.Name == model.Username);
            var user = _context.Users.SingleOrDefault(x => x.Username == model.Username && x.Password == model.Password);

            // return null if user not found
            // if (user == null) return null;
            // correção de warning // correção de warning (https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/compiler-messages/nullable-warnings)
            if (user == null) return null!;

            // authentication successful so generate jwt token
            var token = generateJwtToken(user);

            return new AuthenticateResponse(user, token);
        }

        public IEnumerable<User> GetAll()
        {
            // return _users;
            return _context.Users.ToList();
        }

        public User GetById(int id)
        {
            // return _users.FirstOrDefault(x => x.Id == id);
            // return _context.Users.FirstOrDefault(x => x.Id == id);
            // correção de warning (https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/compiler-messages/nullable-warnings)
            return _context.Users.FirstOrDefault(x => x.Id == id)!;
        }

        // helper methods

        private string generateJwtToken(User user)
        {
            // generate token that is valid for 7 days
            var tokenHandler = new JwtSecurityTokenHandler();
            // var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            // correção de warning (https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/compiler-messages/nullable-warnings)
            var key = Encoding.ASCII.GetBytes(_appSettings.Password!);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("id", user.Id.ToString()) }),
                // Expires = DateTime.UtcNow.AddDays(7),
                // Expires = DateTime.UtcNow.AddMinutes(20).ToLongTimeString(),
                // Expires = DateTime.UtcNow.AddMinutes(2),
                Expires = DateTime.UtcNow.AddHours(8),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}