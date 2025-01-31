using CRUD_Dapper.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CRUD_Dapper.Repository
{
    public interface IDestinationRepository
    {
        Task<IEnumerable<Destination>> GetAllDestinations();
        Task<IEnumerable<Destination>> GetDestinationsByOwner(int ownerId);
        Task<int> AddDestination(Destination destination);
        Task<int> UpdateDestination(Destination destination, int userId);
        Task<int> DeleteDestination(int id, int userId);
    }
}

