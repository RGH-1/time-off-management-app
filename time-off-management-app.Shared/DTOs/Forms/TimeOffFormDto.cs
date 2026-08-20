using time_off_management_app.Shared.DTOs.Reasons;
using time_off_management_app.Shared.Enums;

namespace time_off_management_app.Shared.DTOs.Forms
{
    public class TimeOffFormDto
    {
        public int Id { get; set; }

        public String UserId { get; set; }
        public String FullName { get; set; }

        public DateTime SubmissionDate { get; set; }
        public DateTime DateTimeFrom { get; set; }
        public DateTime DateTimeTo { get; set; }

        public ReasonDto TimeOffReason { get; set; }
        public String? ReasonDescription { get; set; }

        public ApprovalStatus Status { get; set; }
        public String? ApprovedById { get; set; }
        public String? ApprovedByFullName { get; set; }

        public String? Note { get; set; }
    }
}
