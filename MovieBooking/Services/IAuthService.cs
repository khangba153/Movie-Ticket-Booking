using MovieBooking.Models;

namespace MovieBooking.Services
{
    public interface IAuthService
    {
        Task<User?> LoginAsync(string username, string password);
        Task<User> RegisterAsync(string username, string password);
        Task<bool> UsernameExistsAsync(string username);
    }
}
