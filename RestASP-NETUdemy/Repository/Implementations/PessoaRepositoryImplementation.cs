using RestASP_NETUdemy.Model;
using RestASP_NETUdemy.Model.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace RestASP_NETUdemy.Repository.Implementations
{
    public class PessoaRepositoryImplementation : IPessoaRepository
    {
        //Retirada essa variavel, servia para usar antes de ter banco de dados
        //private volatile int count; 

        private MySQLContext _context; //Declarar o Context para a classe

        public PessoaRepositoryImplementation(MySQLContext context) //Criar um Construtor que não existia antes de precisar da conexão do banco de dados, com parametrosde DBContext
        {
            _context = context; //Atribiu ao Context da Classe o Context que veio como Paramtro
        }

        public List<Pessoa> FindAll()
        {
            return _context.Pessoas.ToList();
        }

        public Pessoa FindByID(long id)
        {
            return _context.Pessoas.SingleOrDefault(p => p.Id.Equals(id));
        }

        public Pessoa Update(Pessoa pessoa)
        {
            if (!Exists(pessoa.Id)) return new Pessoa();

            var result = _context.Pessoas.SingleOrDefault(p => p.Id.Equals(pessoa.Id));

            if (result != null)
            {
                try
                {
                    _context.Entry(result).CurrentValues.SetValues(pessoa);
                    _context.SaveChanges();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            return pessoa;
        }

        public Pessoa Create(Pessoa pessoa)
        {
            try
            {
                _context.Add(pessoa);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return pessoa;
        }

        public void Delete(long id)
        {
            var result = _context.Pessoas.SingleOrDefault(p => p.Id.Equals(id));

            if (result != null)
            {
                try
                {
                    _context.Pessoas.Remove(result);
                    _context.SaveChanges();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        //Metodo comentando porque vamos pegar do banco, usamos esse metodo abaixo por conta do inicio do curso para aprendizagem, agora vamos pegar do banco
        //public List<Pessoa> FindAll()
        //{
        //    List<Pessoa> pessoas = new List<Pessoa>();
        //    for(int i =0; i < 8; i++)
        //    {
        //        Pessoa pessoa = GerarPessoa(i);
        //        pessoas.Add(pessoa);
        //    };
        //    return pessoas;
        //}
        

        public bool Exists(long id)
        {
            return _context.Pessoas.Any(p => p.Id.Equals(id));

        }


        //Metodo comentando porque vamos pegar do banco, usamos esse metodo abaixo por conta do inicio do curso para aprendizagem, agora vamos pegar do banco
        //private Pessoa GerarPessoa(int i)
        //{
        //    return new Pessoa
        //    {
        //        Id = GerarIdPessoa(),
        //        primeiroNome = "Primeiro Nome " + i,
        //        ultimoNome = "Ultimo Nome " + 1,
        //        Endereco = "Itapua, Vila Velha " + 1,
        //        Genero = "Masculino"
        //    };
        //}
        //Metodo comentando porque vamos pegar do banco, usamos esse metodo abaixo por conta do inicio do curso para aprendizagem, agora vamos pegar do banco
        //private long GerarIdPessoa()
        //{
        //    return Interlocked.Increment(ref count);
        //} 
    }
}
