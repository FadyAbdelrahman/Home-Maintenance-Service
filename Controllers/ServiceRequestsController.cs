using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HomeMaintenance.Data;
using HomeMaintenance.Models;

namespace HomeMaintenance.Controllers;

// API Controller for managing service requests
[ApiController]
[Route("api/[controller]")]
public class ServiceRequestsController : ControllerBase
{
    private readonly AppDbContext _context;
    
    // Constructor inject database context
    public ServiceRequestsController(AppDbContext context)
    {
        _context = context;
    }
    
    // GET: api/servicerequests Get all service requests
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ServiceRequest>>> GetServiceRequests()
    {
        return await _context.ServiceRequests
            .Include(r => r.Service)
            .ToListAsync();
    }
    
    // GET: api/servicerequests/5 Get a specific request by ID
    [HttpGet("{id}")]
    public async Task<ActionResult<ServiceRequest>> GetServiceRequest(int id)
    {
        var serviceRequest = await _context.ServiceRequests
            .Include(r => r.Service)
            .FirstOrDefaultAsync(r => r.Id == id);
        
        if (serviceRequest == null)
        {
            return NotFound();
        }
        
        return serviceRequest;
    }
    
    // GET: api/servicerequests/customer/email@example.com Get requests by customer email
    [HttpGet("customer/{email}")]
    public async Task<ActionResult<IEnumerable<ServiceRequest>>> GetServiceRequestsByCustomer(string email)
    {
        return await _context.ServiceRequests
            .Include(r => r.Service)
            .Where(r => r.Email.ToLower() == email.ToLower())
            .ToListAsync();
    }
    
    // GET: api/servicerequests/status/Pending Get requests by status
    [HttpGet("status/{status}")]
    public async Task<ActionResult<IEnumerable<ServiceRequest>>> GetServiceRequestsByStatus(string status)
    {
        return await _context.ServiceRequests
            .Include(r => r.Service)
            .Where(r => r.Status.ToLower() == status.ToLower())
            .ToListAsync();
    }
    
    // POST: api/servicerequests Create a new service request
    [HttpPost]
    public async Task<ActionResult<ServiceRequest>> CreateServiceRequest(ServiceRequest serviceRequest)
    {
        // Set initial status and creation time
        serviceRequest.Status = "Pending";
        serviceRequest.CreatedAt = DateTime.Now;
        
        _context.ServiceRequests.Add(serviceRequest);
        await _context.SaveChangesAsync();
        
        return CreatedAtAction(nameof(GetServiceRequest), new { id = serviceRequest.Id }, serviceRequest);
    }
    
    // PUT: api/servicerequests/5/status Update request status
    [HttpPut("{id}/status")]
    public async Task<IActionResult> UpdateStatus(int id, [FromBody] string status)
    {
        var serviceRequest = await _context.ServiceRequests.FindAsync(id);
        if (serviceRequest == null)
        {
            return NotFound();
        }
        
        serviceRequest.Status = status;
        await _context.SaveChangesAsync();
        
        return NoContent();
    }
    
    // PUT: api/servicerequests/5/rating Add customer rating and comments
    [HttpPut("{id}/rating")]
    public async Task<IActionResult> AddRating(int id, [FromBody] RatingModel rating)
    {
        var serviceRequest = await _context.ServiceRequests.FindAsync(id);
        if (serviceRequest == null)
        {
            return NotFound();
        }
        
        serviceRequest.Rating = rating.Rating;
        serviceRequest.Comments = rating.Comments;
        
        await _context.SaveChangesAsync();
        
        return NoContent();
    }
    
    // DELETE: api/servicerequests/5 Cancel/delete a service request
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteServiceRequest(int id)
    {
        var serviceRequest = await _context.ServiceRequests.FindAsync(id);
        if (serviceRequest == null)
        {
            return NotFound();
        }
        
        _context.ServiceRequests.Remove(serviceRequest);
        await _context.SaveChangesAsync();
        
        return NoContent();
    }
}

// Model for receiving rating data from frontend
public class RatingModel
{
    public int Rating { get; set; }
    public string? Comments { get; set; }
}