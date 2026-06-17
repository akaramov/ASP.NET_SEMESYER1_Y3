using APAERMENT_LAST_API.Helpers;
using APAERMENT_LAST_API.Models;

namespace APAERMENT_LAST_API.Repositories.Interfaces
{
    public interface IBuildingRepository
    {
        Task<PagedResult<Building>> GetBuildings(int page = 1, int pageSize = 10);
        Task<Building> GetBuilding(int id);
        Task<Building> Create(Building building);
        Task<Building> Update(Building building);
        Task<bool> Delete(Building building);
    }
}
