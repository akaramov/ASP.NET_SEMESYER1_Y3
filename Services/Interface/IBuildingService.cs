using APAERMENT_LAST_API.DTOs.Requests;
using APAERMENT_LAST_API.DTOs.Responses;
using APAERMENT_LAST_API.Helpers;

namespace APAERMENT_LAST_API.Services.Interface
{
    public interface IBuildingService
    {
        Task<PagedResult<BuildingResDto>> GetBuildings(int page = 1, int pageSize = 10);
        Task<BuildingResDto> GetBuilding(int id);
        Task<BuildingResDto> Create(BuildingReqDto building);
        Task<BuildingResDto> Update(int id, BuildingReqDto building);
        Task<bool> Delete(int id);
    }
}
