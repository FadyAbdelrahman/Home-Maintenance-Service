using Microsoft.EntityFrameworkCore;
using HomeMaintenance.Data;

// Create the web application builder
var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi();

// Setup SQLite database connection
var connectionString = "Data Source=homemaintenance.db";
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(connectionString));

// Build the application
var app = builder.Build();

// Create database and tables if they don't exist
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    context.Database.EnsureCreated();
}

// Enable serving static files (HTML, CSS, JS)
app.UseStaticFiles();

// Enable routing
app.UseRouting();

// Map API controllers
app.MapControllers();

// Serve index.html for any route that doesn't match an API
app.MapFallbackToFile("index.html");

// Run the application
app.Run();