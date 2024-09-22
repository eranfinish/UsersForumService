using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UsersForumService.DAL.Entities;

namespace UsersForumService.DAL.Repositories.Users
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            if (_context.Users == null)
            {
                throw new ArgumentNullException(nameof(_context.Users));
            }
            var users = await _context.Users.ToListAsync();
            if (users == null || !users.Any())
            {
                throw new InvalidOperationException("No users found.");
            }
            return users;
        }

        public async Task<User?> GetUserByIdAsync(int id)
        {
            if (_context.Users == null)
            {
                throw new ArgumentNullException(nameof(_context.Users));
            }
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                throw new KeyNotFoundException($"User with ID {id} not found.");
            }
            return user;
        }

        public async Task AddUserAsync(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            try
            {
                await _context.Users.AddAsync(user);


                await _context.SaveChangesAsync();

            }
            catch (DbUpdateException ex)
            {
                // Log or return the detailed error message for debugging
                throw new Exception($"DB Update Error: {ex.InnerException?.Message ?? ex.Message}");
            }
            catch (Exception ex)
            {
                // Log or handle the exception
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public async Task UpdateUserAsync(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            if (_context.Users == null)
            {
                throw new ArgumentNullException(nameof(_context.Users));
            }
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteUserAsync(int id)
        {
            var user = await GetUserByIdAsync(id);
            if (user == null)
            {
                throw new KeyNotFoundException($"User with ID {id} not found.");
            }
            if (_context.Users == null)
            {
                throw new ArgumentNullException(nameof(_context.Users));
            }
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }

        public async Task<User?> GetUserWithPostsAsync(int id)
        {
            if(_context.Users == null)
            {
                return new User();
            }
            var user = await _context.Users
                .Include(u => u.Posts)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (user == null)
            {
                throw new KeyNotFoundException($"User with ID {id} not found.");
            }

            return user;
        }

        public async Task<User?> FindUserByEmailAsync(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                throw new ArgumentNullException(nameof(email), "Email cannot be null or empty.");
            }

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null)
            {
                return null;
            }

            return user;
        }

        public async Task<User?> GetUserByUserNameAsync(string username)
        {
            if(_context.Users == null)
            {
                return new User();
            }
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == username);
            //if (user == null)
            //{
            //    throw new KeyNotFoundException($"User with email {username} not found.");
            //}  
            return user;
        }
    }

}
