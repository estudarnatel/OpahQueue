namespace OpahQueue.Helpers
{
    public class AppSettings
    {
        // public string Secret { get; set; }
        // correção de warning (https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/compiler-messages/nullable-warnings)
        public string? Password { get; set; }
    }
}