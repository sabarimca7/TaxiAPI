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
        //var cab = await _context.Cab.FindAsync(bookingRequest.CabId);
        //if (cab == null || !cab.IsAvailable)
        //    return BadRequest("Cab not available");

        //cab.IsAvailable = false;
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
        Console.WriteLine("Booking received");
    }

    [HttpGet]
    public async Task<IActionResult> GetBookingList()
    {
        var BookingList = await _context.Booking
            .ToListAsync();

        return Ok(BookingList); // <-- This sends data to Angular
    }

    [HttpGet("id")]
    public async Task<IActionResult> GetBookingListById(int id)
    {
        var User = await _context.Booking.Where(x => x.Id == id).FirstOrDefaultAsync();

        if (User == null)
        {
            return NotFound();
        }

        return Ok(User);
    }

    [HttpPut("id")]
    public async Task<IActionResult> UpdateBooking(int id, Booking UserRequest)
    {
        var User = await _context.Booking.Where(x => x.Id == id).FirstOrDefaultAsync();
        if (User == null)
        {
            return NotFound();
        }
        else
        {
            User.CabId = UserRequest.CabId;
            User.FromLocation = UserRequest.FromLocation;
            User.ToLocation = UserRequest.ToLocation;
            User.PickupDateTime = UserRequest.PickupDateTime;
            User.ReturnDateTime = UserRequest.ReturnDateTime;
            User.CustomerName = UserRequest.CustomerName;
            User.PhoneNumber = UserRequest.PhoneNumber;
            User.Address = UserRequest.Address;
        }

        await _context.SaveChangesAsync();

        return Ok(User);
    }
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser(int id)
    {
        var user = await _context.Booking.FirstOrDefaultAsync(x => x.Id == id);
        if (user == null)
            return NotFound();

        _context.Booking.Remove(user);
        await _context.SaveChangesAsync();
        return Ok(user);
    }
}
