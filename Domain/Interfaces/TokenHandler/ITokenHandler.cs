using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces.TokenHandler
{
    public interface ITokenHandler
    {
        public bool TryGetIdFromJwtToken(string token, ref Guid id);
        public bool TryGetRoleFromJwtToken(string token, ref string role);
    }
}
