using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HomeMaintenance.Data;
using HomeMaintenance.Models;

namespace HomeMaintenance.Controllers;

// API Controller for managing services
[ApiController]
[Route("api/[controller]")]
public class ServicesController : ControllerBase
{
    private readonly AppDbContext _context;
    
    // Constructor inject database context
    public ServicesController(AppDbContext context)
    {
        _context = context;
    }
    
    // GET: api/services Get all available services
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Service>>> GetServices()
    {
        return await _context.Services.Where(s => s.Available).ToListAsync();
    }
    
    // GET: api/services/5 Get a specific service by ID
    [HttpGet("{id}")]
    public async Task<ActionResult<Service>> GetService(int id)
    {
        var service = await _context.Services.FindAsync(id);
        if (service == null)
        {
            return NotFound();
        }
        return service;
    }
    
    // GET: api/services/category/Electrical Get services by category
    [HttpGet("category/{category}")]
    public async Task<ActionResult<IEnumerable<Service>>> GetServicesByCategory(string category)
    {
        return await _context.Services
            .Where(s => s.Category.ToLower() == category.ToLower() && s.Available)
            .ToListAsync();
    }
    
    // POST: api/services Create a new service
    [HttpPost]
    public async Task<ActionResult<Service>> CreateService(Service service)
    {
        _context.Services.Add(service);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetService), new { id = service.Id }, service);
    }
    
    // PUT: api/services/5 Update an existing service
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateService(int id, Service service)
    {
        if (id != service.Id)
        {
            return BadRequest();
        }
        
        _context.Entry(service).State = EntityState.Modified;
        
        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!ServiceExists(id))
            {
                return NotFound();
            }
            throw;
        }
        
        return NoContent();
    }
    
    // DELETE: api/services/5 Delete a service
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteService(int id)
    {
        var service = await _context.Services.FindAsync(id);
        if (service == null)
        {
            return NotFound();
        }
        
        _context.Services.Remove(service);
        await _context.SaveChangesAsync();
        
        return NoContent();
    }
    
    // Helper method to check if a service exists
    private bool ServiceExists(int id)
    {
        return _context.Services.Any(s => s.Id == id);
    }
}