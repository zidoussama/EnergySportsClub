using System;
using System.Collections.Generic;

namespace EnergySportsClub.ViewModels
{
    public class UserDashboardViewModel
    {
        public int TotalTerrains { get; set; }
        public int AvailableTerrains { get; set; }
        public int TotalMaterials { get; set; }
        public int AvailableMaterials { get; set; }
        public int CurrentReservationsCount { get; set; }
        public int UpcomingReservationsCount { get; set; }
        public int ReservationHistoryCount { get; set; }

        public IReadOnlyList<UserTerrainItem> Terrains { get; set; } = Array.Empty<UserTerrainItem>();
        public IReadOnlyList<UserMaterialItem> Materials { get; set; } = Array.Empty<UserMaterialItem>();
        public IReadOnlyList<UserReservationItem> CurrentReservations { get; set; } = Array.Empty<UserReservationItem>();
        public IReadOnlyList<UserReservationItem> UpcomingReservations { get; set; } = Array.Empty<UserReservationItem>();
        public IReadOnlyList<UserReservationItem> ReservationHistory { get; set; } = Array.Empty<UserReservationItem>();
        public IReadOnlyList<UserNotificationItem> Notifications { get; set; } = Array.Empty<UserNotificationItem>();
        public IReadOnlyList<UserReservationItem> ReservationCalendar { get; set; } = Array.Empty<UserReservationItem>();
    }

    public class UserTerrainItem
    {
        public int Id { get; set; }
        public string Type { get; set; } = string.Empty;
        public string Dimensions { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public decimal PricePerHour { get; set; }
        public int ReservationCount { get; set; }
        public string DetailsUrl { get; set; } = string.Empty;
        public string CreateReservationUrl { get; set; } = string.Empty;
    }

    public class UserMaterialItem
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public int Stock { get; set; }
        public decimal PricePerHour { get; set; }
        public string DetailsUrl { get; set; } = string.Empty;
        public string CreateReservationUrl { get; set; } = string.Empty;
    }

    public class UserReservationItem
    {
        public int Id { get; set; }
        public string ResourceType { get; set; } = string.Empty;
        public string ResourceName { get; set; } = string.Empty;
        public DateTime ReservationDate { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public TimeSpan? ReturnTime { get; set; }
        public string Status { get; set; } = string.Empty;
        public string DetailsUrl { get; set; } = string.Empty;
        public string CancelUrl { get; set; } = string.Empty;
        public bool CanCancel { get; set; }
    }

    public class UserNotificationItem
    {
        public string Title { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public string Severity { get; set; } = string.Empty;
    }
}
