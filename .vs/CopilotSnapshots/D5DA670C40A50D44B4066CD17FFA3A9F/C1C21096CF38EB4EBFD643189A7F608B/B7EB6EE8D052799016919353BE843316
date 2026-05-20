using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace EnergySportsClub.Models
{
    public class ReservationMaterial
    {
        public int Id { get; set; }
        public DateTime ReservationDate { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public TimeSpan ReturnTime { get; set; }

        public int MaterialId { get; set; }
        [ValidateNever]
        public Material Material { get; set; }

        public string UserId { get; set; }
        [ValidateNever]
        public ApplicationUser User { get; set; } 

    }
}
