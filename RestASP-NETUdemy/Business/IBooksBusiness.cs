using RestASP_NETUdemy.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestASP_NETUdemy.Business
{
    public interface IBooksBusiness
    {
        Books Create(Books book);
        Books FindByID(long id);
        List<Books> FindAll();
        Books Update(Books book);
        void Delete(long id);
    }
}
