using crudmysql.Data;
using crudmysql.DTOs.Pagination;
using crudmysql.DTOs.Product;
using crudmysql.DTOs.User;
using crudmysql.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace crudmysql.Services
{
    public class UserService
    {
        private readonly ApplicationDbContext _context;
        private readonly PasswordHasher<User> _passwordHasher;
        public UserService(ApplicationDbContext context, PasswordHasher<User> passwordHasher)
        {
            _context = context;
            _passwordHasher = passwordHasher;
        }

        public async Task<(List<UserDTO> Data, PaginationInfo Pagination)> GetAllUsersAsync(int page, int perPage)
        {
            var total = await _context.Users.CountAsync();
            var items = await _context.Users
                .Select(u => new UserDTO
                {
                    Id = u.Id,
                    Name = u.Name,
                    Email = u.Email
                })
                .Skip((page - 1) * perPage)
                .Take(perPage)
                .ToListAsync();

            var pagination = new PaginationInfo
            {
                Page = page,
                PerPage = perPage,
                Total = total,
                TotalPages = (int)Math.Ceiling(total / (double)perPage),
                From = ((page - 1) * perPage) + 1,
                To = Math.Min(page * perPage, total)
            };

            return (items, pagination);
        }

        public async Task<UserDTO?> GetUserByIdAsync(int id)
        {
            try
            {
                var user = await _context.Users
                    .Where(u => u.Id == id)
                    .Select(u => new UserDTO
                    {
                        Id = u.Id,
                        Name = u.Name,
                        Email = u.Email
                    })
                    .FirstOrDefaultAsync();

                return user;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving the user.", ex);
            }
        }


        public async Task<UserDTO> CreateUserAsync(User user)
        {
            // Cek email duplikat
            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == user.Email);
            if (existingUser != null)
            {
                throw new ArgumentException("Email already exists.");
            }

            try
            {
                user.Password = _passwordHasher.HashPassword(user, user.Password);
                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                var userUpdate = await GetUserByIdAsync(user.Id); 
                return userUpdate!;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while creating the user.", ex);
            }
        }

        public async Task<User> UpdateUserAsync(int id, User user)
        {
            try
            {
                var existingUser = await _context.Users.FindAsync(id);
                if (existingUser == null)
                {
                    throw new ArgumentException("User not found");
                }

                existingUser.Name = user.Name;
                existingUser.Email = user.Email;
                existingUser.Password = user.Password;

                await _context.SaveChangesAsync();
                return existingUser;
            }
            catch (ArgumentException)
            {

                throw;
            }
            catch (Exception ex)
            {
                // kalau unexpected error, lempar sebagai 500
                throw new Exception("An unexpected error occurred during update.", ex);
            }
        }

        public async Task<User> DeleteUserAsync(int id)
        {
            try
            {
                var existingUser = await _context.Users.FindAsync(id);
                if (existingUser == null)
                {
                    throw new ArgumentException("User not found");
                }

                _context.Users.Remove(existingUser);
                await _context.SaveChangesAsync();

                return existingUser;
            }
            catch (ArgumentException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new Exception("An unexpected error occurred during deletion.", ex);
            }
        }



    }
}
