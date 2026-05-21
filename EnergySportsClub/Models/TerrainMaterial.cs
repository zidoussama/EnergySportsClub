using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace EnergySportsClub.Models
{
    public class TerrainMaterial
    {
        public int Id { get; set; }
        public int TerrainId { get; set; }
        [ValidateNever]
        public Terrain Terrain { get; set; } 

        public int MaterialId { get; set; }
        [ValidateNever]
        public Material Material { get; set; }
    }
}
