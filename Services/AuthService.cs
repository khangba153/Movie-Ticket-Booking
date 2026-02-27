using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;
using MovieBooking.Data;
using MovieBooking.Models;

namespace MovieBooking.Services
{
    public class AuthService : IAuthService
    {
        private readonly ApplicationDbContext _context;

        public AuthService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<User?> LoginAsync(string username, string password)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Username == username);

            if (user == null)
                return null;

            // Verify password
            if (!VerifyPassword(password, user.PasswordHash))
                return null;

            return user;
        }

        public async Task<User> RegisterAsync(string username, string password)
        {
            if (await UsernameExistsAsync(username))
                throw new InvalidOperationException("Tên người dùng đã tồn tại.");

            var user = new User
            {
                Username = username,
                Name = username,
                Email = "",
                PasswordHash = HashPassword(password)
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return user;
        }

        public async Task<bool> UsernameExistsAsync(string username)
        {
            return await _context.Users.AnyAsync(u => u.Username == username);
        }

        // Simple password hashing using SHA256 + salt
        private static string HashPassword(string password)
        {
            byte[] salt = RandomNumberGenerator.GetBytes(16);
            using var sha = SHA256.Create();
            byte[] hash = sha.ComputeHash(salt.Concat(System.Text.Encoding.UTF8.GetBytes(password)).ToArray());
            return Convert.ToBase64String(salt) + ":" + Convert.ToBase64String(hash);
        }

        private static bool VerifyPassword(string password, string storedHash)
        {
            // Handle legacy "dummy_hash" for seeded users
            if (storedHash == "dummy_hash")
                return false;

            var parts = storedHash.Split(':');
            if (parts.Length != 2)
                return false;

            byte[] salt = Convert.FromBase64String(parts[0]);
            byte[] expectedHash = Convert.FromBase64String(parts[1]);

            using var sha = SHA256.Create();
            byte[] actualHash = sha.ComputeHash(salt.Concat(System.Text.Encoding.UTF8.GetBytes(password)).ToArray());

            return actualHash.SequenceEqual(expectedHash);
        }
    }
}
