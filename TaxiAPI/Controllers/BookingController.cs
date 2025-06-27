using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net.Mail;
using System.Net;
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
            PhoneNumber = bookingRequest.PhoneNumber,
            Address = ""
        };
        _context.Booking.Add(booking);
        await _context.SaveChangesAsync();
        // Example cab owner email — in real app, fetch from DB or config
       // string ownerEmail = "sabarimca7@gmail.com";

        //// Compose email
        //string subject = "New Cab Booking";
        //string body = $"<h3>New Booking Confirmed</h3>" +
        //              $"<p><strong>Customer:</strong> {bookingRequest.CustomerName}</p>" +
        //              $"<p><strong>Pickup:</strong> {bookingRequest.FromLocation}</p>" +
        //              $"<p><strong>Drop:</strong> {bookingRequest.ToLocation}</p>" +
        //              $"<p><strong>Date:</strong> {bookingRequest.PickupDateTime}</p>";

        //// Send email
        //using (var client = new SmtpClient("smtp.gmail.com", 587))
        //{
        //    client.Credentials = new NetworkCredential("thiruvarasan121@gmail.com", "raam1234");
        //    client.EnableSsl = true;

        //    var mail = new MailMessage("thiruvarasan121@gmail.com", ownerEmail, subject, body)
        //    {
        //        IsBodyHtml = true
        //    };

        //    try
        //    {
        //        await client.SendMailAsync(mail);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
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
