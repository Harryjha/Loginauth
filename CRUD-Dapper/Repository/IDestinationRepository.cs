using CRUD_Dapper.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CRUD_Dapper.Repository
{
    public interface IDestinationRepository
    {
        Task<IEnumerable<Destination>> GetAllDestinations();
        Task<Destination> GetDestinationById(int id);
        Task AddDestination(Destination destination);
        Task UpdateDestination(Destination destination);
        Task DeleteDestination(int id);
    }
}
