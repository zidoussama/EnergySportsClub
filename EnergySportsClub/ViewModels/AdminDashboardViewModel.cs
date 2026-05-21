using System.Collections.Generic;

namespace EnergySportsClub.ViewModels
{
    public class AdminDashboardViewModel
    {
        public int TotalReservations { get; set; }
        public int TotalTerrains { get; set; }
        public int TotalClients { get; set; }
        public int ReservationsToday { get; set; }
        public int ReservationsThisWeek { get; set; }
        public int? PeakReservationHour { get; set; }
        public IReadOnlyList<ReservationSummaryItem> ReservationsByDay { get; set; } = new List<ReservationSummaryItem>();
        public IReadOnlyList<ReservationSummaryItem> ReservationsByWeekDay { get; set; } = new List<ReservationSummaryItem>();
    }

    public class ReservationSummaryItem
    {
        public string Label { get; set; } = string.Empty;
        public int Count { get; set; }
    }
}
