using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<Car> Cars { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Rental> Rentals { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Car>().HasMany(x => x.Rentals).WithOne(x => x.Car).HasForeignKey(x => x.CarId);
        modelBuilder.Entity<User>().HasMany(x => x.Rentals).WithOne(x => x.User).HasForeignKey(x => x.UserId);
    }
}