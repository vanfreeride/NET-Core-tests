
using Microsoft.EntityFrameworkCore;

public class MyContext : DbContext {

    public DbSet<Shit> Shits {get; set;}

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql("server=localhost;database=MyDb;UserId=root;Password=878787");

        }
}
