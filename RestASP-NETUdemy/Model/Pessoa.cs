using RestASP_NETUdemy.Model.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace RestASP_NETUdemy.Model
{
    [Table("pessoa")]
    public class Pessoa : BaseEntity
    {
        [Column("primeiroNome")]
        public string primeiroNome { get; set; }

        [Column("ultimoNome")]
        public string ultimoNome { get; set; }

        [Column("Endereco")]
        public string Endereco { get; set; }

        [Column("Genero")]
        public string Genero { get; set; }
    }
}
