using Microsoft.EntityFrameworkCore;
using RestASP_NETUdemy.Model;
using RestASP_NETUdemy.Model.Base;
using RestASP_NETUdemy.Model.Context;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RestASP_NETUdemy.Repository.Generic
{
    public class GenericRepositoryImpl<T> : IRepository<T> where T : BaseEntity
    {
        private MySQLContext _context; //Declarar o Context para a classe

        private DbSet<T> _dataset;

        public GenericRepositoryImpl(MySQLContext context) //Criar um Construtor que não existia antes de precisar da conexão do banco de dados, com parametrosde DBContext
        {
            //Atribiu ao Context local o Context que veio como parametro
            _context = context;

            //Adicionou dinamicamente o DbSet(Generico) na classe genérica
            _dataset = _context.Set<T>(); 
        }
       
               
        public List<T> FindAll()
        {
            return _dataset.ToList();
        }

        public T FindByID(long id)
        {
            return _dataset.SingleOrDefault(p => p.Id.Equals(id));
        }

        public T Create(T item)
        {
            try
            {
                _dataset.Add(item);
                _context.SaveChanges();
                return item;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public T Update(T item)
        {
            var result = _dataset.SingleOrDefault(p => p.Id.Equals(item.Id));
            if (result != null)
            {
                try
                {
                    _context.Entry(result).CurrentValues.SetValues(item);
                    _context.SaveChanges();
                    return result;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            else
            {
                return null;
            }
            
        }

        public void Delete(long id)
        {
            var result = _dataset.SingleOrDefault(p => p.Id.Equals(id));
            if (result != null)
            {
                try
                {
                    _dataset.Remove(result);
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
            return _dataset.Any(p => p.Id.Equals(id));
        }
    }
}
