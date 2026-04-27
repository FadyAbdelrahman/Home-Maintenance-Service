using Microsoft.EntityFrameworkCore;
using HomeMaintenance.Models;

namespace HomeMaintenance.Data;

// Database context class handles all database operations
public class AppDbContext : DbContext
{
    // Constructor to initialize the database context
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
    
    // Database table for services
    public DbSet<Service> Services { get; set; }
    
    // Database table for service requests
    public DbSet<ServiceRequest> ServiceRequests { get; set; }
    
    // Method to configure the database model and seed initial data
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        // Add sample services to the database when its created
        modelBuilder.Entity<Service>().HasData(
            new Service { Id = 1, Name = "Electrical Repair", Description = "Fixing lights, outlets, wiring", Price = 50.00m, Category = "Electrical", Available = true },
            new Service { Id = 2, Name = "Plumbing Service", Description = "Fixing pipes, sinks, toilets", Price = 45.00m, Category = "Plumbing", Available = true },
            new Service { Id = 3, Name = "AC Maintenance", Description = "Air conditioning repair and cleaning", Price = 60.00m, Category = "HVAC", Available = true },
            new Service { Id = 4, Name = "Carpentry Work", Description = "Furniture repair, cabinet installation", Price = 40.00m, Category = "Carpentry", Available = true },
            new Service { Id = 5, Name = "Painting Service", Description = "Interior and exterior painting", Price = 35.00m, Category = "Painting", Available = true },
            new Service { Id = 6, Name = "Appliance Repair", Description = "Washing machines, refrigerators, etc.", Price = 55.00m, Category = "Appliance", Available = true }
        );
    }
}