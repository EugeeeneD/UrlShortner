using Domain.Interfaces;
using Domain.Models;
using Infrasructure.Data;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Repository
{
    public class ShortenedUrlRepository : IShortenedUrlRepository
    {
        // maybe should just create base repository with save method
        private readonly UrlDbContext _context;

        public ShortenedUrlRepository(UrlDbContext context)
        {
            _context = context;
        }

        public async Task<bool> AddShortenedUrlAsync(string url, Guid userId)
        {
            Uri validatedUri;
            if (!Uri.TryCreate(url, UriKind.Absolute, out validatedUri))
                return false;

            var shortenedUrl = GenerateShortenedUrlAsync(url, userId);
            await _context.ShortenedUrls.AddAsync(shortenedUrl);
            return await SaveAsync();
        }

        public async Task<bool> DeleteShortnedUrlAsync(ShortenedUrl url)
        {
            _context.ShortenedUrls.Remove(url);
            return await SaveAsync();
        }

        public async Task<bool> DoesLongUrlExists(string url)
        {
            return await _context.ShortenedUrls.AnyAsync(u => u.LongUrl == url);
        }

        public ShortenedUrl GenerateShortenedUrlAsync(string url, Guid userId)
        {
            ShortenedUrl res = new ShortenedUrl()
            {
                Id = Guid.NewGuid(),
                LongUrl = url,
                UserId = userId,
                DateOfCreation = DateTime.Now
            };

            return res;
        }

        public async Task<ShortenedUrl> GetShortenedUrlAsync(Guid id)
        {
            return await _context.ShortenedUrls.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<ICollection<ShortenedUrl>> GetShortenedUrlsAsync()
        {
            return await _context.ShortenedUrls.ToListAsync();
        }

        public async Task<bool> SaveAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
