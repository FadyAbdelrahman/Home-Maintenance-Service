using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HomeMaintenance.Models;

// ServiceRequest model represents a customer's service request
public class ServiceRequest
{
    // Primary key unique ID for each request
    [Key]
    public int Id { get; set; }
    
    // Foreign key links to the Service table
    public int ServiceId { get; set; }
    
    // Navigation property allows access to the related Service object
    [ForeignKey(nameof(ServiceId))]
    public Service? Service { get; set; }
    
    // Customer's full name required, max 100 characters
    [Required]
    [MaxLength(100)]
    public string CustomerName { get; set; } = string.Empty;
    
    // Customer's email required, max 100 characters
    [Required]
    [MaxLength(100)]
    public string Email { get; set; } = string.Empty;
    
    // Customer's phone number max 20 characters
    [MaxLength(20)]
    public string Phone { get; set; } = string.Empty;
    
    // Service address max 200 characters
    [MaxLength(200)]
    public string Address { get; set; } = string.Empty;
    
    // Customers preferred date and time for service
    public DateTime PreferredDate { get; set; }
    
    // Request status (Pending, In Progress, Completed)  default is Pending
    [MaxLength(20)]
    public string Status { get; set; } = "Pending";
    
    // Customer rating (1-5 stars)  optional, filled after service completion
    public int? Rating { get; set; }
    
    // Customer comments about the service optional, max 500 characters
    [MaxLength(500)]
    public string? Comments { get; set; }
    
    // Timestamp when the request was created  automatically set to current time
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}