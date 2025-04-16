using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using crudmysql.Data;
using crudmysql.DTOs.Auth;
using crudmysql.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using static Org.BouncyCastle.Math.EC.ECCurve;

namespace crudmysql.Services
{
    public class AuthService
    {
        private readonly ApplicationDbContext _context;
        private readonly PasswordHasher<User> _passwordHasher;
        private readonly IConfiguration _config;


        public AuthService(ApplicationDbContext context, PasswordHasher<User> passwordHasher, IConfiguration config)
        {
            _context = context;
            _passwordHasher = passwordHasher;
            _config = config;
        }

        public User Login(LoginDTO dto)
        {
            try
            {
                var user = _context.Users.FirstOrDefault(u => u.Email == dto.Email);
                if (user == null) return null;

                var verifyResult = _passwordHasher.VerifyHashedPassword(user, user.Password, dto.Password);
                if (verifyResult == PasswordVerificationResult.Failed)
                    return null;

                return user; // return full user object
            }
            catch (Exception ex)
            {
                return null;
            }
        }

    }
}
