using RestASP_NETUdemy.Configurations;
using RestASP_NETUdemy.Model;
using RestASP_NETUdemy.Repository.UserApi;
using RestASP_NETUdemy.Token.Data;
using RestASP_NETUdemy.Token.Service;
using RestASP_NETUdemy.VO;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace RestASP_NETUdemy.Business.Implementations
{
    public class LoginBusinessImpl : ILoginBusiness
    {
        private const string DATE_FORMAT = "yyyy-MM-dd HH:mm:ss";
        private TokenConfigurations _configurarion;

        private IUserRepository _repository;
        private readonly ITokenService _tokenService;

        public LoginBusinessImpl(TokenConfigurations configurarion, IUserRepository repository, ITokenService tokenService)
        {
            _configurarion = configurarion;
            _repository = repository;
            _tokenService = tokenService;
        }

        public TokenVO ValidarCredenciais(UserVO credencialUser)
        {
            var _user = _repository.ValidarCredencial(credencialUser);

            if (_user == null) return null;

            var _claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N")),
                new Claim(JwtRegisteredClaimNames.UniqueName, _user.user_name)
            };

            var _acessoToken = _tokenService.GerarTokenAcesso(_claims);
            var _refreshToken = _tokenService.GerarRefreshToken();
           

            _user.refresh_token = _refreshToken;
            _user.refresh_token_expiry_time = DateTime.Now.AddDays(_configurarion.DaysToExpiry);

            _repository.RefreshUserInfo(_user);

            var _createDate = DateTime.Now;
            var _expirationDate = _createDate.AddMinutes(_configurarion.Minutes);


            return new TokenVO(true,
                                 _createDate.ToString(DATE_FORMAT),
                                 _expirationDate.ToString(DATE_FORMAT),
                                 _acessoToken,
                                 _refreshToken
                                 );

        }

        //Para Refresh Token
        public TokenVO ValidarCredenciais(TokenVO token)
        {
            var _acessoToken = token.AcessToken;
            var _refreshToken = token.RefreshToken;

            var _principal = _tokenService.ObterClaimsPrincipalFromExpiredToken(_acessoToken);

            var _username = _principal.Identity.Name;

            var _user = _repository.ValidarCredencial(_username);
            if(_user == null || _user.refresh_token != _refreshToken || _user.refresh_token_expiry_time <= DateTime.Now)
            {
                return null;
            }

            _acessoToken = _tokenService.GerarTokenAcesso(_principal.Claims);
            _refreshToken = _tokenService.GerarRefreshToken();

            _user.refresh_token = _refreshToken;

            _repository.RefreshUserInfo(_user);


            //Gravar no banco o novo refresh token
            var _createDate = DateTime.Now;
            var _expirationDate = _createDate.AddMinutes(_configurarion.Minutes);
            return new TokenVO(true,
                                 _createDate.ToString(DATE_FORMAT),
                                 _expirationDate.ToString(DATE_FORMAT),
                                 _acessoToken,
                                 _refreshToken
                                 );
        }
        public bool RevokeToken(string userName)
        {
            return _repository.RevokeToken(userName);
        }
    }
}
