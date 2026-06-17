using APAERMENT_LAST_API.Configurations;
using APAERMENT_LAST_API.Helpers;
using APAERMENT_LAST_API.Models;
using APAERMENT_LAST_API.Repositories.Interfaces;

namespace APAERMENT_LAST_API.Repositories
{
    public class BuildingRepository : IBuildingRepository
    {
        private readonly ApplicationDbContext _context;

        public BuildingRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<PagedResult<Building>> GetBuildings(int page = 1, int pageSize = 10)
        {
            var data = await _context.TblBuilding.ToPagedResultAsync(page, pageSize);
            return data;
        }

        public async Task<Building> GetBuilding(int id)
        {
            var data = await _context.TblBuilding.FindAsync(id);
            return data!;
        }

        public async Task<Building> Create(Building building)
        {
            await _context.TblBuilding.AddAsync(building);
            await _context.SaveChangesAsync();
            return building;
        }

        public async Task<Building> Update(Building building)
        {
            _context.TblBuilding.Update(building);
            await _context.SaveChangesAsync();
            return building;
        }

        public async Task<bool> Delete(Building building)
        {
            _context.TblBuilding.Remove(building);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
