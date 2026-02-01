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
        modelBuilder.Entity<User>().HasOne(u => u.Rental).WithMany(r => r.Users);
        modelBuilder.Entity<User>().HasOne(u => u.Car).WithOne(c => c.User);
        modelBuilder.Entity<Car>().HasOne(c => c.Rental).WithMany(r => r.Cars);
    }
}