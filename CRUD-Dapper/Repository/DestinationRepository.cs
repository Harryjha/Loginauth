using Dapper;
using CRUD_Dapper.Data;
using CRUD_Dapper.Models;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace CRUD_Dapper.Repository
{
    public class DestinationRepository : IDestinationRepository
    {
        private readonly IDapperContext _context;

        public DestinationRepository(IDapperContext context)
        {
            _context = context;
        }

        // ✅ Fetch all destinations (for Admin/General view)
        public async Task<IEnumerable<Destination>> GetAllDestinations()
        {
            var query = "SELECT * FROM Destinations";
            using var connection = _context.CreateConnection();
            return await connection.QueryAsync<Destination>(query);
        }

        // ✅ Fetch destinations owned by a specific user
        public async Task<IEnumerable<Destination>> GetDestinationsByOwner(int ownerId)
        {
            var query = "SELECT * FROM Destinations WHERE OwnerId = @OwnerId";
            using var connection = _context.CreateConnection();
            return await connection.QueryAsync<Destination>(query, new { OwnerId = ownerId });
        }

        // ✅ Add a new destination
        public async Task<int> AddDestination(Destination destination)
        {
            var query = @"
        INSERT INTO Destinations (Name, Description, Price, CreatedDate, OwnerId, ImageUrl)
        VALUES (@Name, @Description, @Price, @CreatedDate, @OwnerId, @ImageUrl);
        SELECT CAST(SCOPE_IDENTITY() AS INT);"; // ✅ This returns the newly generated Id

            using var connection = _context.CreateConnection();
            var newId = await connection.QuerySingleAsync<int>(query, new
            {
                destination.Name,
                destination.Description,
                destination.Price,
                destination.CreatedDate,
                destination.OwnerId,
                destination.ImageUrl
            });

            return newId;
        }


        // ✅ Update a destination (validate owner)
        public async Task<int> UpdateDestination(Destination destination, int userId)
        {
            var query = @"
                UPDATE Destinations
                SET Name = @Name, Description = @Description, Price = @Price, ImageUrl = @ImageUrl
                WHERE Id = @Id AND OwnerId = @OwnerId";   // ✅ Ensure only the owner can update

            using var connection = _context.CreateConnection();
            return await connection.ExecuteAsync(query, new
            {
                destination.Name,
                destination.Description,
                destination.Price,
                destination.ImageUrl,
                destination.Id,
                OwnerId = userId
            });
        }

        // ✅ Delete a destination (restrict to owner)
        public async Task<int> DeleteDestination(int id, int userId)
        {
            var query = "DELETE FROM Destinations WHERE Id = @Id AND OwnerId = @OwnerId"; // ✅ Ownership check
            using var connection = _context.CreateConnection();
            return await connection.ExecuteAsync(query, new { Id = id, OwnerId = userId });
        }
    }
}


