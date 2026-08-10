using time_off_management_app.Shared.DTOs.Forms;

namespace time_off_management_app.Shared.DTOs.Users
{
    public class UserDashboardDto
    {
        public String FullName { get; set; }
        //public Position Position { get; set; }
        //public Unit Unit { get; set; }
        public int RemainingAnnualLeaveDays { get; set; }
        public int RemainingOtherLeaveDays { get; set; }
        public int PendingRequests { get; set; }

        public List<TimeOffSummaryDto> UpcomingTimeOff { get; set; }
    }
}
