namespace Lab_1.CustomMiddeware
{
    using System.Net;
    using System.Text.Json;

    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;
      
        public GlobalExceptionMiddleware( RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                context.Response.ContentType = "application/json";

                var message = "Site is Off Now. Plz try again later.";

                var response = new
                {
                    success = false,
                    message,

                };

                await context.Response.WriteAsJsonAsync(response);
                
            }
        }

    }

  
}

