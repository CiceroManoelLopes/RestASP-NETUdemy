using RestASP_NETUdemy.Model;
using RestASP_NETUdemy.Model.Context;
using RestASP_NETUdemy.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace RestASP_NETUdemy.Business.Implementations
{
    public class PessoaBusinessImplementation : IPessoaBusiness
    {
        //Retirada essa variavel, servia para usar antes de ter banco de dados
        //private volatile int count; 

        private readonly IRepository<Pessoa> _repository; 

        public PessoaBusinessImplementation(IRepository<Pessoa> repository) //Criar um Construtor que não existia antes de precisar da conexão do banco de dados, com parametrosde DBContext
        {
            _repository = repository; //Atribiu ao Context da Classe o Context que veio como Paramtro
        }

        public List<Pessoa> FindAll()
        {
            return _repository.FindAll();
        }

        public Pessoa FindByID(long id)
        {
            return _repository.FindByID(id);
        }

        public Pessoa Create(Pessoa pessoa)
        {
            return _repository.Create(pessoa);
        }

        public Pessoa Update(Pessoa pessoa)
        {
            return _repository.Update(pessoa);
        }       

        public void Delete(long id)
        {
           _repository.Delete(id);
        }   
    }
}
