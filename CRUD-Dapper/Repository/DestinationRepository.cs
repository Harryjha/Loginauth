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

        public async Task<IEnumerable<Destination>> GetAllDestinations()
        {
            using var connection = _context.CreateConnection();
            return await connection.QueryAsync<Destination>("SELECT * FROM Destinations");
        }

    
        public async Task AddDestination(Destination destination)
        {
            var query = "INSERT INTO Destinations (Name, Description, Price) VALUES (@Name, @Description, @Price)";
            using (var connection = _context.CreateConnection())
            {
                await connection.ExecuteAsync(query, destination);
            }
        }

    }
}
