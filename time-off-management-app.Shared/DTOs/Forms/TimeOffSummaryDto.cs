using time_off_management_app.Shared.DTOs.Reasons;
using time_off_management_app.Shared.Enums;

namespace time_off_management_app.Shared.DTOs.Forms
{
    public class TimeOffSummaryDto
    {
        public int Id { get; set; }
        public ReasonDto Reason { get; set; }
        public DateTime DateTimeFrom { get; set; }
        public DateTime DateTimeTo { get; set; }
        public ApprovalStatus Status { get; set; }
    }
}
