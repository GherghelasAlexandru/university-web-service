using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Text;

namespace StudentWebService
{
    public class BasicAuthentication : IAsyncActionFilter
    {
        private const string Username = "admin";
        private const string Password = "admin";

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (!IsValidCredentials(context))
            {
                context.HttpContext.Response.Headers["WWW-Authenticate"] = "Basic";
                context.Result = new UnauthorizedResult();
                return;
            }
            await next();
        }

        private bool IsValidCredentials(ActionExecutingContext context)
        {
            if (!context.HttpContext.Request.Headers.ContainsKey("Authorization"))
                return false;

            var authHeader = context.HttpContext.Request.Headers["Authorization"].ToString();
            var encodedCredentials = authHeader.Replace("Basic ", "");
            var decodedCredentials = Encoding.UTF8.GetString(Convert.FromBase64String(encodedCredentials));
            var credentials = decodedCredentials.Split(':');

            var username = credentials[0];
            var password = credentials[1];

            return username == Username && password == Password;
        }
    }
}
