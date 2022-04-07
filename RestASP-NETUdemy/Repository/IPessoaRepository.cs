using RestASP_NETUdemy.Model;
using System.Collections.Generic;

namespace RestASP_NETUdemy.Repository
{
    public interface IPessoaRepository
    {
        Pessoa Create(Pessoa pessoa);
        Pessoa FindByID(long id);
        List<Pessoa> FindAll();
        Pessoa Update(Pessoa pessoa);
        void Delete(long id);
        bool Exists(long id);
    }
}
