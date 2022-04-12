using RestASP_NETUdemy.Model;
using RestASP_NETUdemy.Model.Context;
using RestASP_NETUdemy.Token.Data;
using RestASP_NETUdemy.VO;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace RestASP_NETUdemy.Repository.UserApi
{
    public class UserRepositoryImpl : IUserRepository
    {
        private readonly MySQLContext _context;

        public UserRepositoryImpl(MySQLContext context)
        {
            _context = context;
        }
        public Users ValidarCredencial(UserVO userVO)
        {
            Users usuas = null;

            var pass = GerarHash(userVO.Password, new SHA256CryptoServiceProvider());
            try
            {
                usuas = _context.Users.FirstOrDefault(u => (u.user_name == userVO.UserName) && (u.password == pass));                  
            }
            catch (Exception e)
            {
                //
            }
            return usuas;
        }

        //Para o refesh token
        public Users ValidarCredencial(string userName)
        {
            return _context.Users.SingleOrDefault(u => (u.user_name == userName));           
        }

        public bool RevokeToken(string userName)
        {
            var _user = _context.Users.SingleOrDefault(u => (u.user_name == userName));
            if (_user == null)
            {
                return false;
            }
            _user.refresh_token = null;

            _context.SaveChanges();

            return true;
        }

        public Users RefreshUserInfo(Users user)
        {
            if (!_context.Users.Any(p => p.id.Equals(user.id)))
            {
                return null;
            }

            var result = _context.Users.SingleOrDefault(p => p.id.Equals(user.id));
            if (result != null)
            {
                try
                {
                    _context.Entry(result).CurrentValues.SetValues(user);
                    _context.SaveChanges();
                    return result;
                }
                catch (Exception ex)
                {
                    throw ex;
                }                
            }
            return result;

        }

        //Encriptar a senha
        private object GerarHash(string input, SHA256CryptoServiceProvider algorithm)
        {
            Byte[] _inputBytes = Encoding.UTF8.GetBytes(input);
            Byte[] _hashedBytes = algorithm.ComputeHash(_inputBytes);
            return BitConverter.ToString(_hashedBytes);
        }
    }
}
