using Microsoft.AspNetCore.Identity;

namespace EnergySportsClub.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;

        public int Age { get; set; }
        public string Gender { get; set; } = string.Empty;

        

        public ICollection<ReservationTerrain> ReservationTerrains { get; set; } = new List<ReservationTerrain>();
        public ICollection<ReservationMaterial> ReservationMaterials { get; set; } = new List<ReservationMaterial>();
    }
}