using EnergySportsClub.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EnergySportsClub.Data
{
    public class DbcontextTest : IdentityDbContext<ApplicationUser>
    {
        public DbcontextTest(DbContextOptions<DbcontextTest> options) : base(options)
        {
        }
        public DbSet<Terrain> Terrains { get; set; }
        public DbSet<Material> Materials { get; set; }
        public DbSet<ReservationTerrain> ReservationTerrains { get; set; }
        public DbSet<ReservationMaterial> ReservationMaterials { get; set; }
        public DbSet<EnergySportsClub.Models.TerrainMaterial> TerrainMaterial { get; set; } = default!;



    }
}
