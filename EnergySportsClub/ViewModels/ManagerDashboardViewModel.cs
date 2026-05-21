using System;
using System.Collections.Generic;

namespace EnergySportsClub.ViewModels
{
    public class ManagerDashboardViewModel
    {
        public int TotalTerrains { get; set; }
        public int AvailableTerrains { get; set; }
        public int UnavailableTerrains { get; set; }

        public int TotalMaterials { get; set; }
        public int AvailableMaterials { get; set; }
        public int UnavailableMaterials { get; set; }
        public int LowStockMaterials { get; set; }

        public int TotalTerrainReservations { get; set; }
        public int CurrentTerrainReservations { get; set; }
        public int UpcomingTerrainReservations { get; set; }

        public int TotalMaterialReservations { get; set; }
        public int CurrentMaterialReservations { get; set; }
        public int UpcomingMaterialReservations { get; set; }

        public IReadOnlyList<ManagerTerrainItem> Terrains { get; set; } = Array.Empty<ManagerTerrainItem>();
        public IReadOnlyList<ManagerMaterialItem> Materials { get; set; } = Array.Empty<ManagerMaterialItem>();
        public IReadOnlyList<ManagerReservationItem> CurrentReservations { get; set; } = Array.Empty<ManagerReservationItem>();
        public IReadOnlyList<ManagerReservationItem> UpcomingReservations { get; set; } = Array.Empty<ManagerReservationItem>();
        public IReadOnlyList<ManagerReservationItem> ReservationCalendar { get; set; } = Array.Empty<ManagerReservationItem>();
        public IReadOnlyList<ManagerNotificationItem> Notifications { get; set; } = Array.Empty<ManagerNotificationItem>();
        public IReadOnlyList<ManagerMaterialItem> LowStockAlerts { get; set; } = Array.Empty<ManagerMaterialItem>();
    }

    public class ManagerTerrainItem
    {
        public int Id { get; set; }
        public string Type { get; set; } = string.Empty;
        public string Dimensions { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public decimal PricePerHour { get; set; }
        public int ReservationCount { get; set; }
        public string EditUrl { get; set; } = string.Empty;
        public string DetailsUrl { get; set; } = string.Empty;
    }

    public class ManagerMaterialItem
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public int Stock { get; set; }
        public decimal PricePerHour { get; set; }
        public string EditUrl { get; set; } = string.Empty;
        public string DetailsUrl { get; set; } = string.Empty;
        public bool IsLowStock { get; set; }
    }

    public class ManagerReservationItem
    {
        public int Id { get; set; }
        public string ResourceType { get; set; } = string.Empty;
        public string ResourceName { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public DateTime ReservationDate { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public TimeSpan? ReturnTime { get; set; }
        public string Status { get; set; } = string.Empty;
        public string EditUrl { get; set; } = string.Empty;
        public string DeleteUrl { get; set; } = string.Empty;
        public string DetailsUrl { get; set; } = string.Empty;
        public bool CanEdit { get; set; }
        public bool CanCancel { get; set; }
        public bool CanChangeAssignedTerrain { get; set; }
    }

    public class ManagerNotificationItem
    {
        public string Title { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public string Severity { get; set; } = string.Empty;
        public string Source { get; set; } = string.Empty;
    }
}
