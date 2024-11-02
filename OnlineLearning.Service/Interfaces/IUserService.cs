using OnlineLearning.Entity.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OnlineLearning.Service.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<ApplicationUser>> GetAllInstructorsAsync();
        Task<ApplicationUser> GetUserByIdAsync(string userId);
        // Add any other user-related methods if needed
    }
}
