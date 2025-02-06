using CRUD_Dapper.Models;

namespace CRUD_Dapper.Repository
{
    public interface IBookingRepository
    {
        Task<IEnumerable<Booking>> GetUserBookings(int userId);  

        Task AddBooking(Booking booking);
        Task CancelBooking(int bookingId, int userId);
    }
}



