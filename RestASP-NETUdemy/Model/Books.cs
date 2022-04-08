using RestASP_NETUdemy.Model.Base;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestASP_NETUdemy.Model
{
    [Table("books")]
    public class Books : BaseEntity
    {
        [Column("author")]
        public string Autor { get; set; }

        [Column("launch_date")]
        public DateTime dataLancamento { get; set; }

        [Column("price")]
        public decimal Preco { get; set; }

        [Column("title")]
        public string Titulo { get; set; }
    }
}
