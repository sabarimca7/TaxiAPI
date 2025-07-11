using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net.Mail;
using System.Net;
using TaxiApplication.Server.Request;
using Microsoft.AspNetCore.Authorization;

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

        var fromEmail = "thiruvarasan121@gmail.com";
        var password = "hghotopfkjozuelh";
        var toEmail = "sabarimca7@gmail.com";

        var subject = "New Booking Recieved From " + bookingRequest.CustomerName + " Location : (" + bookingRequest.FromLocation + ") TO  (" + bookingRequest.ToLocation + ")!";
        var body = $"<h3>Dear User,</h3><p>New Booking Recieved From " + bookingRequest.CustomerName + "</p> <p>Phone:" + bookingRequest.PhoneNumber + "</p>";

        try
        {
            var mail = new MailMessage(fromEmail, toEmail, subject, body);
            mail.IsBodyHtml = true;

            using (var smtp = new SmtpClient("smtp.gmail.com", int.Parse("587")))
            {
                smtp.Credentials = new NetworkCredential(fromEmail, password);
                smtp.EnableSsl = true;
                smtp.Send(mail);
            }

        }
        catch (Exception ex)
        {
            throw ex;
        }


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
