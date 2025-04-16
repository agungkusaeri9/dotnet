
using crudmysql.Models;
using crudmysql.Services;
using Microsoft.AspNetCore.Mvc;
using crudmysql.Helpers;

namespace crudmysql.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;

        public UserController(UserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(int page = 1, int limit = 10)
        {
            try
            {
                var (data, pagination) = await _userService.GetAllUsersAsync(page, limit);
                return ResponseFormatter.Success(data, "Success mengambil data",200,pagination);
            }
            catch (Exception ex)
            {
                return ResponseFormatter.Error(ex.Message, "Gagal mengambil data", 500);
            }
        }



        [HttpPost]
        public async Task<IActionResult> Create([FromBody] User user)
        {
            try
            {
                var createdUser = await _userService.CreateUserAsync(user);
                return ResponseFormatter.Success(createdUser);
            }
            catch (ArgumentException ex) 
            {
                return BadRequest(new
                {
                    success = false,
                    message = ex.Message
                });
            }
            catch (Exception ex)
            {
                // Internal server error
                return StatusCode(500, new
                {
                    success = false,
                    message = "An unexpected error occurred.",
                    error = ex.Message // Optional: hilangkan di production
                });
            }
        }



        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var user = await _userService.GetUserByIdAsync(id);
                if (user == null)
                {
                    return ResponseFormatter.NotFound("User not found");
                }

                return ResponseFormatter.Success(user, "User retrieved successfully");
            }
            catch (Exception ex)
            {
                // Log error kalau perlu
                return ResponseFormatter.Error($"Internal server error: {ex.Message}");
            }
        }


        [HttpPatch("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] User user)
        {
            try
            {
                var userCheck = await _userService.GetUserByIdAsync(id);
                if (userCheck == null)
                    return ResponseFormatter.NotFound("User not found");

                var updatedUser = await _userService.UpdateUserAsync(id, user);
                return ResponseFormatter.Success(updatedUser);
            }
            catch (ArgumentException ex)
            {
                return ResponseFormatter.ValidationError(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "An error occurred while updating the user",
                    error = ex.Message
                });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Remove(int id, [FromBody] User user)
        {
            try
            {
                var userCheck = await _userService.GetUserByIdAsync(id);
                if (userCheck == null)
                    return ResponseFormatter.NotFound("User not found");

                var updatedUser = await _userService.DeleteUserAsync(id);
                return ResponseFormatter.Success(updatedUser);
            }
            catch (ArgumentException ex)
            {
                return ResponseFormatter.ValidationError(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "An error occurred while updating the user",
                    error = ex.Message
                });
            }
        }
    }
}
