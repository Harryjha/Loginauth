using CRUD_Dapper.Data;
using CRUD_Dapper.Models;
using Dapper;
using System.Collections.Generic;
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

        public async Task<IEnumerable<Destination>> GetAllDestinations()
        {
            using (var connection = _context.CreateConnection())
            {
                string query = "SELECT * FROM Destinations ORDER BY CreatedDate DESC";
                return await connection.QueryAsync<Destination>(query);
            }
        }

        public async Task<Destination> GetDestinationById(int id)
        {
            using (var connection = _context.CreateConnection())
            {
                string query = "SELECT * FROM Destinations WHERE Id = @Id";
                return await connection.QuerySingleOrDefaultAsync<Destination>(query, new { Id = id });
            }
        }

        public async Task AddDestination(Destination destination)
        {
            using (var connection = _context.CreateConnection())
            {
                string query = "INSERT INTO Destinations (Name, Description, ImageUrl) VALUES (@Name, @Description, @ImageUrl)";
                await connection.ExecuteAsync(query, destination);
            }
        }

        public async Task UpdateDestination(Destination destination)
        {
            using (var connection = _context.CreateConnection())
            {
                string query = "UPDATE Destinations SET Name = @Name, Description = @Description, ImageUrl = @ImageUrl WHERE Id = @Id";
                await connection.ExecuteAsync(query, destination);
            }
        }

        public async Task DeleteDestination(int id)
        {
            using (var connection = _context.CreateConnection())
            {
                string query = "DELETE FROM Destinations WHERE Id = @Id";
                await connection.ExecuteAsync(query, new { Id = id });
            }
        }
    }
}
