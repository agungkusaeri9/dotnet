using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace crudmysql.Helpers
{
    public static class CustomValidationResponse
    {
        public static IActionResult FormatInvalidModelResponse(ActionContext context)
        {
            var errors = context.ModelState
                .Where(m => m.Value?.Errors.Count > 0)
                .ToDictionary(
                    kvp => kvp.Key,
                    kvp => kvp.Value!.Errors.Select(e => e.ErrorMessage).ToArray()
                );

            var response = new
            {
                success = false,
                message = "Validation failed",
                errors = errors
            };

            return new ObjectResult(response)
            {
                StatusCode = StatusCodes.Status422UnprocessableEntity
            };
        }
    }
}
