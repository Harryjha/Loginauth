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

        // ✅ Fetch all destinations (for customers)
        public async Task<IEnumerable<Destination>> GetAllDestinations()
        {
            using var connection = _context.CreateConnection();
            return await connection.QueryAsync<Destination>("SELECT * FROM Destinations");
        }

        // ✅ Fetch destinations for a specific owner
        public async Task<IEnumerable<Destination>> GetDestinationsByOwner(int ownerId)
        {
            var query = "SELECT * FROM Destinations WHERE OwnerId = @OwnerId";
            using var connection = _context.CreateConnection();
            var destinations = await connection.QueryAsync<Destination>(query, new { OwnerId = ownerId });

            if (!destinations.Any())
            {
                Console.WriteLine($"No destinations found for OwnerId: {ownerId}");
            }

            return destinations;
        }

        // ✅ Add a new destination and assign the owner
        public async Task<int> AddDestination(Destination destination)
        {
            var query = "INSERT INTO Destinations (Name, Description, Price, CreatedDate, OwnerId, ImageUrl) VALUES (@Name, @Description, @Price, @CreatedDate, @OwnerId, @ImageUrl)";
            using var connection = _context.CreateConnection();
            return await connection.ExecuteAsync(query, destination);
        }

        // ✅ Update a destination (only if owned by the user)
        public async Task<int> UpdateDestination(Destination destination, int userId)
        {
            var query = "UPDATE Destinations SET Name = @Name, Description = @Description, Price = @Price, ImageUrl = @ImageUrl WHERE Id = @Id AND OwnerId = @OwnerId";
            using var connection = _context.CreateConnection();
            return await connection.ExecuteAsync(query, new { destination.Name, destination.Description, destination.Price, destination.ImageUrl, OwnerId = userId });
        }

        // ✅ Delete a destination (only if owned by the user)
        public async Task<int> DeleteDestination(int id, int userId)
        {
            var query = "DELETE FROM Destinations WHERE Id = @Id AND OwnerId = @OwnerId";
            using var connection = _context.CreateConnection();
            return await connection.ExecuteAsync(query, new { Id = id, OwnerId = userId });
        }
    }
}
