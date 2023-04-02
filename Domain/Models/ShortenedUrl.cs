using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class ShortenedUrl
    {
        // maybe it should be a record
        public Guid Id { get; set; }
        public string LongUrl { get; set; }
        public DateTime DateOfCreation { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; }
    }
}
