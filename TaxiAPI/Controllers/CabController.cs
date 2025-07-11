using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace TaxiApplication.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CabController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    public CabController(ApplicationDbContext context) => _context = context;

    [HttpGet("available")]
    public async Task<IActionResult> GetAvailableCabs(string city)
    {
        var availableCabs = await _context.Cab
            .Where(c => c.IsAvailable && c.Location.ToLower() == city.Trim().ToLower())
            .ToListAsync();
        return Ok(availableCabs);
    }
}
