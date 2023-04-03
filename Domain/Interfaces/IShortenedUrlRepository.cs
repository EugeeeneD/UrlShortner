using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IShortenedUrlRepository : IBaseRepository
    {
        public Task<ShortenedUrl> GetShortenedUrlAsync(Guid id);
        public Task<ICollection<ShortenedUrl>> GetShortenedUrlsAsync();
        public Task<bool> AddShortenedUrlAsync(string url, Guid userId);
        public ShortenedUrl GenerateShortenedUrlAsync(string url, Guid userId);
        public Task<bool> DeleteShortnedUrlAsync(ShortenedUrl url);
        public Task<bool> DoesLongUrlExists(string url);
    }
}
