using Domain.Interfaces.TokenHandler;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Service.TokenHandler
{
    public class JwtTokenHandler : ITokenHandler
    {
        public bool TryGetIdFromJwtToken(string token, ref Guid id)
        {
            var jwtHandler = new JwtSecurityTokenHandler();

            var handledToken = jwtHandler.ReadJwtToken(token);

            Claim userId = handledToken.Claims.FirstOrDefault(x => x.Type == "id");

            if (userId == null)
                return false;

            string idAsString = userId.Value;

            id = new Guid(idAsString);
            return true;
        }

        public bool TryGetRoleFromJwtToken(string token, ref string role)
        {
            var jwtHandler = new JwtSecurityTokenHandler();

            var handledToken = jwtHandler.ReadJwtToken(token);

            Claim userRole = handledToken.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Role);

            if (userRole == null)
                return false;

            role = userRole.Value;
            return true;
        }
    }
}
