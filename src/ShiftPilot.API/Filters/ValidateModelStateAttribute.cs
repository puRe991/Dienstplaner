using ShiftPilot.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ShiftPilot.API.Filters;

public class ValidateModelStateAttribute : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        if (!context.ModelState.IsValid)
        {
            var errors = context.ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)).ToList();
            var response = new ApiResponse
            {
                Success = false,
                Message = "Validation failed",
                Errors = errors
            };

            context.Result = new BadRequestObjectResult(response);
        }
    }
}
