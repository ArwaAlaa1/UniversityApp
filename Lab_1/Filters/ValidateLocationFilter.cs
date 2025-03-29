using Lab_1.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;

namespace Lab_1.Filters
{
    public class ValidateLocationFilter : ActionFilterAttribute
    {
        private static readonly string[] AllowedLocations = { "LON", "PAR" };

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.ActionArguments.TryGetValue("department", out var departmentObj) && departmentObj is DepartmentSendDto department)
            {
                string location = department?.Location;

                if (string.IsNullOrEmpty(location) || !AllowedLocations.Contains(location.ToUpper()))
                {
                    context.Result = new BadRequestObjectResult(new
                    {
                        success = false,
                        message = "Invalid location. Allowed values: 'Lon' or 'Par'."
                    });
                }
            }
            else
            {
                context.Result = new BadRequestObjectResult(new
                {
                    success = false,
                    message = "Department Location is Valid."
                });
            }

            base.OnActionExecuting(context);
        }
    
}
}
