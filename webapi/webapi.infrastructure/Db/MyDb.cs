using Microsoft.EntityFrameworkCore;
using webapi.infrastructure.DbObjects;

namespace webapi.infrastructure.Db
{
    public class MyDb : DbContext
    {
        public MyDb(DbContextOptions<MyDb> options) : base(options)
        { }
        
        public DbSet<BookRow> Books { get; set; }
 
    }
}