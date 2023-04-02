using Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrasructure.Data
{
    public class UrlDbContext : IdentityDbContext<IdentityUser<Guid>, IdentityRole<Guid>, Guid>
    {
        public UrlDbContext(DbContextOptions<UrlDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ShortenedUrl>()
                .HasOne(s => s.User)
                .WithMany(u => u.ShortnedUrls)
                .HasForeignKey(s => s.UserId);

            modelBuilder.Entity<User>()
                .HasMany(s => s.ShortnedUrls)
                .WithOne(s => s.User);
        }

        public DbSet<User> Users { get; set; }
        public DbSet<ShortenedUrl> ShortenedUrls { get; set; }
    }
}
