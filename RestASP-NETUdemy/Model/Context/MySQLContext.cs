using Microsoft.EntityFrameworkCore;

namespace RestASP_NETUdemy.Model.Context
{
    public class MySQLContext : DbContext
    {
        public MySQLContext()
        {

        }
        public MySQLContext(DbContextOptions<MySQLContext> options) : base(options) { }
        public DbSet<Pessoa> Pessoas { get; set; }
        public DbSet<Books> Books { get; set; }
        public DbSet<Users> Users { get; set; }
    }
}
