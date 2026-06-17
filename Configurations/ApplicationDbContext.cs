using APAERMENT_LAST_API.Models;
using Microsoft.EntityFrameworkCore;

namespace APAERMENT_LAST_API.Configurations
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> opts)
            : base(opts)
        {

        }
        public DbSet<Building> TblBuilding { get; set; }
    }
}
