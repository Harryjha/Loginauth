using CRUD_Dapper.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

using CRUD_Dapper.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CRUD_Dapper.Repository
{
    public interface IDestinationRepository
    {
        Task<IEnumerable<Destination>> GetAllDestinations();
        Task AddDestination(Destination destination);
    }
}
