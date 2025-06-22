public class Booking
{
    public int Id { get; set; }
    public int CabId { get; set; }
    public Cab Cab { get; set; }
    public string FromLocation { get; set; }
    public string ToLocation { get; set; }
    public DateTime PickupDateTime { get; set; }
    public DateTime? ReturnDateTime { get; set; }
    public string CustomerName { get; set; }
    public string PhoneNumber { get; set; }
    public string Address { get; set; }
}
