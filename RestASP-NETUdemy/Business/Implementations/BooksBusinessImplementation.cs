using RestASP_NETUdemy.Model;
using RestASP_NETUdemy.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestASP_NETUdemy.Business.Implementations
{
    public class BooksBusinessImplementation : IBooksBusiness
    {
        private readonly IRepository<Books> _repository;

        public BooksBusinessImplementation(IRepository<Books> repository) //Criar um Construtor que não existia antes de precisar da conexão do banco de dados, com parametrosde DBContext
        {
            _repository = repository; //Atribiu ao Context da Classe o Context que veio como Paramtro
        }

        public List<Books> FindAll()
        {
            return _repository.FindAll();
        }

        public Books FindByID(long id)
        {
            return _repository.FindByID(id);
        }

        public Books Create(Books book)
        {
            return _repository.Create(book);
        }

        public Books Update(Books book)
        {
            return _repository.Update(book);
        }

        public void Delete(long id)
        {
            _repository.Delete(id);
        }
    }
}
