using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaxiApplication.Server.Request;

namespace TaxiApplication.Server.Controllers;


[ApiController]
[Route("api/[controller]")]
public class BookingController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    public BookingController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpPost]
    public async Task<IActionResult> CreateBooking([FromBody] CabBooking bookingRequest)
    {
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

    [HttpGet]
    public async Task<IActionResult> GetBookingList()
    {
        var BookingList = await _context.Booking
            .ToListAsync();

        return Ok(BookingList);
    }

    [HttpGet("id")]
    public async Task<IActionResult> GetBookingListById(int id)
    {
        var booking = await _context.Booking.Where(x => x.Id == id).FirstOrDefaultAsync();

        if (booking == null)
        {
            return NotFound();
        }

        return Ok(booking);
    }

    [HttpPut("id")]
    public async Task<IActionResult> UpdateBooking(int id, [FromBody] CabBooking request)
    {
        var booking = await _context.Booking.Where(x => x.Id == id).FirstOrDefaultAsync();
        if (booking == null)
        {
            return NotFound();
        }
        else
        {
            booking.CabId = request.CabId;
            booking.FromLocation = request.FromLocation;
            booking.ToLocation = request.ToLocation;
            booking.PickupDateTime = request.PickupDateTime;
            booking.ReturnDateTime = request.ReturnDateTime;
            booking.CustomerName = request.CustomerName;
            booking.PhoneNumber = request.PhoneNumber;
            booking.Address = request.Address;
        }
        await _context.SaveChangesAsync();
        return Ok(User);
    }
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser(int id)
    {
        var booking = await _context.Booking.FirstOrDefaultAsync(x => x.Id == id);
        if (booking == null)
            return NotFound();

        _context.Booking.Remove(booking);
        await _context.SaveChangesAsync();
        return Ok(booking);
    }
}
