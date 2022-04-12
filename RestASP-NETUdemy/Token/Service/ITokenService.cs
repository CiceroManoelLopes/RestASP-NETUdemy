using System.Collections.Generic;
using System.Security.Claims;

namespace RestASP_NETUdemy.Token.Service
{
    public interface ITokenService
    {
        string GerarTokenAcesso(IEnumerable<Claim> claims);

        string GerarRefreshToken();

        ClaimsPrincipal ObterClaimsPrincipalFromExpiredToken(string token);
    }
}
