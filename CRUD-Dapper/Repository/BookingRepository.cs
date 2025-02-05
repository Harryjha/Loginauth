using CRUD_Dapper.Data;
using CRUD_Dapper.Models;
using Dapper;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CRUD_Dapper.Repository
{
    public class BookingRepository : IBookingRepository
    {
        private readonly IDapperContext _context;

        public BookingRepository(IDapperContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Booking>> GetUserBookings(int userId)
        {
            var query = "SELECT * FROM Bookings WHERE UserId = @UserId";
            using (var connection = _context.CreateConnection())
            {
                return await connection.QueryAsync<Booking>(query, new { UserId = userId });
            }
        }


        public async Task AddBooking(Booking booking)
        {
            var query = "INSERT INTO Bookings (DestinationId, UserId, BookingDate, Status) VALUES (@DestinationId, @UserId, @BookingDate, @Status)";
            using (var connection = _context.CreateConnection())
            {
                await connection.ExecuteAsync(query, booking);
            }
        }

        public async Task CancelBooking(int bookingId, int userId)
        {
            var query = "DELETE FROM Bookings WHERE Id = @Id AND UserId = @UserId";
            using (var connection = _context.CreateConnection())
            {
                await connection.ExecuteAsync(query, new { Id = bookingId, UserId = userId });
            }
        }
    }
}

