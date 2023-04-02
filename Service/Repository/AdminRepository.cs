using AutoMapper;
using Domain.Dto.Response;
using Domain.Interfaces;
using Domain.Models;
using Infrasructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Repository
{
    public class AdminRepository : IAdminRepository
    {
        private readonly UrlDbContext _context;
        private readonly IMapper _mapper;

        public AdminRepository(UrlDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<bool> DeleteShortnedUrlAsync(Guid urlId)
        {
            var urlToRemove = await _context.ShortenedUrls.FirstOrDefaultAsync(u => u.Id == urlId);
            _context.ShortenedUrls.Remove(urlToRemove);
            return await SaveAsync();        
        }

        public async Task<bool> DeleteUserAsync(Guid userId)
        {
            var userToRemove = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);

            _context.Users.Remove(userToRemove);

            return await SaveAsync();
        }

        public async Task<User> GetUserByIdAsync(Guid userId)
        {
            User user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);

            return user;
        }

        public async Task<ICollection<UserDto>> GetUsersAsync()
        {
            var users = await _context.Users.ToListAsync();
            return _mapper.Map<List<UserDto>>(users);
        }

        public async Task<bool> SaveAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteAllUrlsAsync()
        {
            foreach (var url in _context.ShortenedUrls)
            {
                _context.ShortenedUrls.Remove(url);
            }

            return await SaveAsync();
        }

        public async Task<ICollection<UserDto>> GetAllUsers()
        {
            var users = await _context.Users.ToListAsync();
            return _mapper.Map<ICollection<UserDto>>(users);
        }
    }
}
