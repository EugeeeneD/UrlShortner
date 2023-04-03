using Domain.Dto.Response;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IAdminRepository : IBaseRepository
    {
        public Task<User> GetUserByIdAsync(Guid userId);
        public Task<ICollection<UserDto>> GetUsersAsync();
        public Task<bool> DeleteUserAsync(Guid userId);
        public Task<bool> DeleteShortnedUrlAsync(Guid urlId);
        public Task<bool> DeleteAllUrlsAsync();
        public Task<ICollection<UserDto>> GetAllUsers();
    }
}
