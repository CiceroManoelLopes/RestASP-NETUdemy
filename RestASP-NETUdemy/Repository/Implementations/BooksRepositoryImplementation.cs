using RestASP_NETUdemy.Model;
using RestASP_NETUdemy.Model.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestASP_NETUdemy.Repository.Implementations
{
    public class BooksRepositoryImplementation : IBooksRepository
    {

        private MySQL _context; //Declarar o Context para a classe

        public BooksRepositoryImplementation(MySQL context) //Criar um Construtor que não existia antes de precisar da conexão do banco de dados, com parametrosde DBContext
        {
            _context = context; //Atribiu ao Context da Classe o Context que veio como Paramtro
        }

        public List<Books> FindAll()
        {
            return _context.Books.ToList();
        }

        public Books FindByID(long id)
        {
            return _context.Books.SingleOrDefault(p => p.Id.Equals(id));
        }

        public Books Create(Books book)
        {
            try
            {
                _context.Add(book);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return book;
        }

        public Books Update(Books book)
        {
            if (!Exists(book.Id)) return null;

            var result = _context.Pessoas.SingleOrDefault(p => p.Id.Equals(book.Id));

            if (result != null)
            {
                try
                {
                    _context.Entry(result).CurrentValues.SetValues(book);
                    _context.SaveChanges();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            return book;
        }

       
        public void Delete(long id)
        {
            var result = _context.Books.SingleOrDefault(p => p.Id.Equals(id));

            if (result != null)
            {
                try
                {
                    _context.Books.Remove(result);
                    _context.SaveChanges();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }      

        public bool Exists(long id)
        {
            return _context.Books.Any(p => p.Id.Equals(id));
        }
    }
}
