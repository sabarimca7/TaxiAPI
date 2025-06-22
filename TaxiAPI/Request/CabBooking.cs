namespace TaxiApplication.Server.Request
{
    public class CabBooking
    {
        public int CabId { get; set; }
        public string FromLocation { get; set; }
        public string ToLocation { get; set; }
        public DateTime PickupDateTime { get; set; }
        public DateTime? ReturnDateTime { get; set; }
        public string CustomerName { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
    }
}
