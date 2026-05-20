namespace EnergySportsClub.Models
{
    public class Material
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public enum Status
        {
            Available,
            Unavailable
        }
        public Status MaterialStatus { get; set; }
        public int QteStock { get; set; }

        public decimal PricePerHour { get; set; }

        public ICollection<ReservationMaterial> ReservationMaterials { get; set; } = new List<ReservationMaterial>();
        public ICollection<TerrainMaterial> TerrainMaterials { get; set; } = new List<TerrainMaterial>();
    }
}
