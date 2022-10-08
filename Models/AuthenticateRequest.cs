using System.ComponentModel.DataAnnotations;

namespace OpahQueue.Models
{
    public class AuthenticateRequest
    {
        [Required]
        // public string Username { get; set; }
        // correção de warning (https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/compiler-messages/nullable-warnings)
        public string? Username { get; set; }

        [Required]
        // public string Password { get; set; }
        // correção de warning (https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/compiler-messages/nullable-warnings)
        public string? Password { get; set; }
    }
}