using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace RestASP_NETUdemy.Model
{
    [Table("books")]
    public class Books
    {
        [Column("Id")]
        public long Id { get; set; }

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
