using RestASP_NETUdemy.Model;
using RestASP_NETUdemy.VO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestASP_NETUdemy.Repository.UserApi
{
    public interface IUserRepository
    {
        Users ValidarCredencial(UserVO user);

        Users ValidarCredencial(string username);

        bool RevokeToken(string username);

        Users RefreshUserInfo(Users user);
    }
}
