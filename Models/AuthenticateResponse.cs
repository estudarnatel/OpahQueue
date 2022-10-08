namespace OpahQueue.Models
{
    public class AuthenticateResponse
    {
        // public int Id { get; set; }
        public long Id { get; set; }
        // public string FirstName { get; set; }
        // public string LastName { get; set; }
        // public string Username { get; set; }
        // correção de warning (https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/compiler-messages/nullable-warnings)
        public string? Name { get; set; }
        public string Token { get; set; }

        public int Position {get; set;}


        public AuthenticateResponse(User user, string token)
        {
            Id = user.Id;
            // FirstName = user.FirstName;
            // LastName = user.LastName;
            Name = user.Name;
            Token = token;
            Position = user.Position;
        }
    }
}