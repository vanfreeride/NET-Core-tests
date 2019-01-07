using Microsoft.EntityFrameworkCore;
using webapi.infrastructure.DbObjects;

namespace webapi.infrastructure.Db
{
    public class MyDb : DbContext
    {
        public DbSet<BookRow> Books { get; set; }
 
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySQL("server=127.0.0.1;port=3306;UserId=root;Password=mypassword;database=webapidb;");
        }
    }
}