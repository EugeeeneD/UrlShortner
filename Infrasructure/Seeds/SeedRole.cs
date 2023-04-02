using Domain.Dto.Request;
using Domain.Interfaces;
using Domain.Models;
using Infrasructure.Data;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Seeds
{
    public class SeedRole : IDbContextSeed
    {
        private readonly RoleManager<IdentityRole<Guid>> _roleManager;
        private readonly IUserAuthenticationRepository _userAuthenticationRepository;

        public SeedRole(RoleManager<IdentityRole<Guid>> roleManager, IUserAuthenticationRepository userAuthenticationRepository)
        {
            _roleManager = roleManager;
            _userAuthenticationRepository = userAuthenticationRepository;
        }

        public async Task SeedAsync(UrlDbContext context)
        {
            if (!await _roleManager.RoleExistsAsync("admin"))
            {
                var admin = new IdentityRole<Guid>("admin");
                await _roleManager.CreateAsync(admin);
            }

            if (!await _roleManager.RoleExistsAsync("user"))
            {
                var role = new IdentityRole<Guid>("user");
                await _roleManager.CreateAsync(role);
            }
        }
    }
}
