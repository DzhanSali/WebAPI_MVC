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
    }
}
