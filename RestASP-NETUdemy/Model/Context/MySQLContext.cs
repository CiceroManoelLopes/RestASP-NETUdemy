using Microsoft.EntityFrameworkCore;

namespace RestASP_NETUdemy.Model.Context
{
    public class MySQL : DbContext
    {
        public MySQL()
        {

        }

        public MySQL(DbContextOptions<MySQL> options) : base(options) { }

        public DbSet<Pessoa> Pessoas { get; set; }
        public DbSet<Books> Books { get; set; }
    }
}
