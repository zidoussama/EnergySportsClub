using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace EnergySportsClub.Models
{
    public class ReservationTerrain
    {
        public int Id { get; set; }
        public DateTime ReservationDate { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }


        public string UserId { get; set; }
        [ValidateNever]
        public ApplicationUser User { get; set; } 

        public int TerrainId { get; set; }
        [ValidateNever]
        public Terrain Terrain { get; set; } 
    }
}
