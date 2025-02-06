namespace CRUD_Dapper.Models
{
    public class Booking
    {
        public int Id { get; set; }
        public int DestinationId { get; set; }
        public int UserId { get; set; }
        public DateTime BookingDate { get; set; }
        public string Status { get; set; } = "Pending";
    }
}
