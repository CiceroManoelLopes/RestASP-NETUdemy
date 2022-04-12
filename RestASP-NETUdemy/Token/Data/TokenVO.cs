using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestASP_NETUdemy.Token.Data
{
    public class TokenVO
    {
        public TokenVO(bool authenticated, string create, string expiration, string acessToken, string refreshToken)
        {
            Authenticated = authenticated;
            Create = create;
            Expiration = expiration;
            AcessToken = acessToken;
            RefreshToken = refreshToken;
        }

        public bool Authenticated { get; set; }
        public string Create { get; set; }
        public string Expiration { get; set; }
        public string AcessToken {get; set;}
        public string RefreshToken { get; set; }

    }
}
