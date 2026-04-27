using System.ComponentModel.DataAnnotations;

namespace HomeMaintenance.Models;

// Service model represents a home maintenance service
public class Service
{
    // Primary key unique ID for each service
    [Key]
    public int Id { get; set; }
    
    // Service name required field, max 100 characters
    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;
    
    // Service description max 500 characters
    [MaxLength(500)]
    public string Description { get; set; } = string.Empty;
    
    // Service price in dollars
    public decimal Price { get; set; }
    
    // Service category (Electrical, Plumbing) max 50 characters
    [MaxLength(50)]
    public string Category { get; set; } = string.Empty;
    
    // Whether the service is currently available - default is true
    public bool Available { get; set; } = true;
}