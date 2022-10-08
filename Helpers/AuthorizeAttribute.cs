using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using OpahQueue.Models;

namespace OpahQueue.Helpers
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            // var todoitem = (TodoItem) context.HttpContext.Items["TodoItem"];
            // correção de warning (https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/compiler-messages/nullable-warnings)
            Console.WriteLine("CONTEXTO H   T   T   P");
            Console.WriteLine(context.HttpContext.Items);
            Console.WriteLine("CONTEXTO H   T   T   P");
            var user = (User?) context.HttpContext.Items["User"];
            if (user == null)
            {
                // not logged in
                context.Result = new JsonResult(new { message = "Unauthorized" }) { StatusCode = StatusCodes.Status401Unauthorized };
            }
        }
    }
}