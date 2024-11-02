using Microsoft.AspNetCore.Identity;
using OnlineLearning.Entity.Entities;
using OnlineLearning.Service.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OnlineLearning.Service.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UserService(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IEnumerable<ApplicationUser>> GetAllInstructorsAsync()
        {
            var allUsers = _userManager.Users;
            var instructors = new List<ApplicationUser>();

            foreach (var user in allUsers)
            {
                if (await _userManager.IsInRoleAsync(user, "Instructor"))
                {
                    instructors.Add(user);
                }
            }

            return instructors;
        }

        public async Task<ApplicationUser> GetUserByIdAsync(string userId)
        {
            return await _userManager.FindByIdAsync(userId);
        }
    }
}
