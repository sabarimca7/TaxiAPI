using Microsoft.AspNetCore.Mvc;
using TaxiApplication.Server.Request;

namespace TaxiApplication.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BookingController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    public BookingController(ApplicationDbContext context) => _context = context;

    [HttpPost]
    public async Task<IActionResult> CreateBooking([FromBody] CabBooking bookingRequest)
    {
        var cab = await _context.Cab.FindAsync(bookingRequest.CabId);
        if (cab == null || !cab.IsAvailable)
            return BadRequest("Cab not available");

        cab.IsAvailable = false;
        Booking booking = new Booking()
        {
            CabId = bookingRequest.CabId,
            FromLocation = bookingRequest.FromLocation,
            ToLocation = bookingRequest.ToLocation,
            CustomerName = bookingRequest.CustomerName,
            PickupDateTime = bookingRequest.PickupDateTime,
            ReturnDateTime = bookingRequest.ReturnDateTime,
            Address = bookingRequest.Address,
            PhoneNumber = bookingRequest.PhoneNumber
        };
        _context.Booking.Add(booking);
        await _context.SaveChangesAsync();
        return Ok(booking);
    }
}
