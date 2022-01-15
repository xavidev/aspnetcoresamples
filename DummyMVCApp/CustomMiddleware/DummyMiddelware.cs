using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace DummyMVCApp.CustomMiddleware
{
    public class DummyMiddelware
    {
        private readonly RequestDelegate next;

        public DummyMiddelware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            context.Response.ContentType = "text/plaint";

            await context.Response.WriteAsync("pong");
        }
    }
}