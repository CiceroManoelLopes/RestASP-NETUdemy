using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestASP_NETUdemy.Business;
using RestASP_NETUdemy.Model;
using RestASP_NETUdemy.Token.Data;
using RestASP_NETUdemy.VO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestASP_NETUdemy.Controllers
{
    [ApiVersion("1")]
    [ApiController]
    //[Authorize("Bearer")]
    [Route("api/[controller]/v{version:apiVersion}")]
    public class AuthController : ControllerBase
    {
        private ILoginBusiness _loginBusiness;
        public AuthController(ILoginBusiness loginBusiness)
        {
            this._loginBusiness = loginBusiness;
        }

        [HttpPost]
        [Route("obterToken")]
        public IActionResult ObterToken([FromBody] UserVO user)
        {
            if (user == null)
            {
                return BadRequest("Requisão de Token Inválida");
            }

            var _token = _loginBusiness.ValidarCredenciais(user);
            if (_token == null)
            {
                return Unauthorized();
            }
            return Ok(_token);
        }


        [HttpPost]
        [Route("refreshToken")]
        public IActionResult RefreshToken([FromBody] TokenVO tokenVO)
        {
            if (tokenVO == null)
            {
                return BadRequest("1-Requisão de Refresh Token Inválida");
            }

            var _token = _loginBusiness.ValidarCredenciais(tokenVO);
            if (_token == null)
            {
                return BadRequest("2-Requisão de Refresh Token Inválida");
            }
            return Ok(_token);
        }

        [HttpGet]
        [Authorize("Bearer")]
        [Route("revokeToken")]
        public IActionResult RevokeToken()
        {
            var _username = User.Identity.Name;
            var _result = _loginBusiness.RevokeToken(_username);

            if (!_result)
            {
                return BadRequest("Requisão de Revoke Inválida");
            }           
            return NoContent();
        }
    }
}
