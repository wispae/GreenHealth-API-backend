using GreenHealth_API_backend.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GreenHealth_API_backend.Services
{
    public interface IUserService
    {
        Task<User> DeleteUser(int id);
        Task<User> GetUser(int id);
        Task<IEnumerable<User>> GetUsers();
        Task<User> PostUser(User user);
        Task<User> PutUser(int id, User user);
    }
}