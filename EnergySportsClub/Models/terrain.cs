namespace EnergySportsClub.Models
{
    public class Terrain
    {
        public int Id { get; set; }
        public string Type { get; set; } 

        public string Dimensions { get; set; } 
        public enum Status
        {
            Available,
            Unavailable
        }
        public Status TerrainStatus { get; set; }
        public decimal PricePerHour { get; set; }

        public ICollection<ReservationTerrain> ReservationTerrains { get; set; } = new List<ReservationTerrain>();
        public ICollection<TerrainMaterial> TerrainMaterials { get; set; } = new List<TerrainMaterial>();
    }
}
