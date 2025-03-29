using Microsoft.AspNetCore.Mvc.Filters;

namespace Lab_1.Filters
{
    public class AppNameFilter : IResultFilter
    {
      
            private readonly string _appName;

            public AppNameFilter(IConfiguration configuration)
            {
                _appName = configuration["AppName"] ;
            }

            public void OnResultExecuting(ResultExecutingContext context)
            {
                context.HttpContext.Response.Headers["AppName"] = _appName;
            }

            public void OnResultExecuted(ResultExecutedContext context)
            {
                // No action needed after execution
            }
        
    }
}
