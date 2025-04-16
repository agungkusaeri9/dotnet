using crudmysql.DTOs.Auth;
using crudmysql.Helpers;
using crudmysql.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace crudmysql.Controllers
{
    [Route("api/auth/")]
    [ApiController]
    public class AuthController : ControllerBase
    {

        private readonly AuthService _authService;
        private readonly JwtService _jwtService;

        public AuthController(AuthService authService, JwtService jwtService)
        {
            _authService = authService;
            _jwtService = jwtService;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginDTO dto)
        {
            try
            {
                var user = _authService.Login(dto);

                var token = _jwtService.GenerateToken(user);

                return ResponseFormatter.Success(new
                {
                    token,
                    user
                }, "Login berhasil");
            }
            catch (Exception ex)
            {
                return ResponseFormatter.ServerError(ex.Message);
            }
        }


    }
}
