using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RestASP_NETUdemy.Model;
using RestASP_NETUdemy.Token.Data;
using RestASP_NETUdemy.VO;

namespace RestASP_NETUdemy.Business
{
    public interface ILoginBusiness
    {        
        TokenVO ValidarCredenciais(UserVO user);

        TokenVO ValidarCredenciais(TokenVO tokenVO);
        bool RevokeToken(string userName);
    }
}
