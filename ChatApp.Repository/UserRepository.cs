using ChatApp.Domain.Entities;
using ChatApp.Repository.Interface;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatApp.Repository
{
    public class UserRepository : IUserRepository
    {
        private static readonly List<User> Users = InitialData.GenerateUsers();

        public Task<List<User>> GetAllUsersAsync()
        {
            return Task.FromResult(Users.ToList());
        }

        public Task<User?> GetUserByIdAsync(int id)
        {
            var user = Users.FirstOrDefault(u => u.Id == id);
            return Task.FromResult(user);
        }

        public Task<User?> GetUserByEmailAsync(string email)
        {
            var user = Users.FirstOrDefault(u => u.Email == email);
            return Task.FromResult(user);
        }

        public Task AddUserAsync(User user)
        {
            Users.Add(user);
            return Task.CompletedTask;
        }

        public Task UpdateUserAsync(User user)
        {
            var existingUser = Users.FirstOrDefault(u => u.Id == user.Id);
            if (existingUser != null)
            {
                existingUser.UserName = user.UserName;
                existingUser.Name = user.Name;
                existingUser.Email = user.Email;
                existingUser.IsActive = user.IsActive;
            }
            return Task.CompletedTask;
        }

        public Task DeleteUserAsync(int id)
        {
            var user = Users.FirstOrDefault(u => u.Id == id);
            if (user != null)
            {
                Users.Remove(user);
            }
            return Task.CompletedTask;
        }
    }
}
