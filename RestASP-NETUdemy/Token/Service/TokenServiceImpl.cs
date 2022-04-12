using Microsoft.IdentityModel.Tokens;
using RestASP_NETUdemy.Configurations;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace RestASP_NETUdemy.Token.Service
{
    public class TokenServiceImpl : ITokenService
    {
        private TokenConfigurations _tokenConfiguration;        

        public TokenServiceImpl(TokenConfigurations tokenConfiguration)
        {
            _tokenConfiguration = tokenConfiguration;
        }

        public string GerarTokenAcesso(IEnumerable<Claim> claims)
        {
            var _chave = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_tokenConfiguration.Secret));
            
            var _loginCredencial = new SigningCredentials(_chave, SecurityAlgorithms.HmacSha256);

            var tokenOption = new JwtSecurityToken(
                issuer: _tokenConfiguration.Issuer,
                audience: _tokenConfiguration.Audience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(_tokenConfiguration.Minutes),
                signingCredentials: _loginCredencial
            );

            string tokenString = new JwtSecurityTokenHandler().WriteToken(tokenOption);

            return tokenString;
        }

        public string GerarRefreshToken()
        {
            var numeroRandomico = new byte[32];
            using (var rng = RandomNumberGenerator.Create()){
                rng.GetBytes(numeroRandomico);
                return Convert.ToBase64String(numeroRandomico);
            };
        }        

        public ClaimsPrincipal ObterClaimsPrincipalFromExpiredToken(string token)
        {            
            var _parametrosValidacaoToken = new TokenValidationParameters {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_tokenConfiguration.Secret)),
                ValidateLifetime = false
            };

            var _tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken _securityToken;

            var _principal = _tokenHandler.ValidateToken(token, _parametrosValidacaoToken, out _securityToken);            
            var _jwtSecurityToken = _securityToken as JwtSecurityToken;
            if (_jwtSecurityToken == null ||
                !_jwtSecurityToken.Header.Alg.Equals(
                    SecurityAlgorithms.HmacSha256,
                    StringComparison.InvariantCulture)) throw new SecurityTokenException("Token Inválido");
           
            return _principal;
        }
    }
}
