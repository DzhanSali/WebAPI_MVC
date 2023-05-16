using Microsoft.EntityFrameworkCore;
using WebAPI.Model;

namespace WebAPI.Repository
{
    public class DBConnection : DbContext
    {
        public DBConnection(DbContextOptions<DBConnection> context) : base(context)
        {
        }
        public DbSet<Person> People { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<ReadBooks> ReadBooks { get; set; }
    }
}
