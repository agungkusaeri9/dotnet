using Microsoft.AspNetCore.Mvc;

namespace crudmysql.Helpers
{
    public static class ResponseFormatter
    {
        public static IActionResult Success(object? data = null, string? message = "Success", int code = 200)
        {
            var response = new
            {
                success = true,
                message,
                data
            };

            return code switch
            {
                200 => new OkObjectResult(response),
                201 => new ObjectResult(response) { StatusCode = 201 },
                _ => new ObjectResult(response) { StatusCode = code }
            };
        }

        public static IActionResult Error(string? message = "Something went wrong", object? errors = null, int code = 400)
        {
            var response = new
            {
                success = false,
                message,
                data = (object?)null,
                errors
            };

            return new ObjectResult(response)
            {
                StatusCode = code
            };
        }

        public static IActionResult ValidationError(object errors)
        {
            return Error("Validation error", errors, 422);
        }

        public static IActionResult BadRequest(string? message = "Bad Request", object? errors = null)
        {
            return Error(message, errors, 400);
        }

        public static IActionResult NotFound(string? message = "Resource not found")
        {
            var response = new
            {
                success = false,
                message,
                data = (object?)null,
            };

            return new ObjectResult(response)
            {
                StatusCode = 404
            };
        }
    }
}
