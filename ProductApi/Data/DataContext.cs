using Microsoft.EntityFrameworkCore;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
        /*Database.EnsureDeleted();
        Database.EnsureCreated();*/
    }

    public DbSet<ProductEntity> Products { get; set; }
}