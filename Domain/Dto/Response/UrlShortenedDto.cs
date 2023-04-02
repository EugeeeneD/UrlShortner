using Domain.Interfaces;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Dto.Response
{
    public class UrlShortenedDto
    {
        public Guid Id { get; set; }
        public string Shortened { get; set; }
        public string LongUrl { get; set; }
        public DateTime DateOfCreation { get; set; }
        public Guid UserId { get; set; }
    }
}
