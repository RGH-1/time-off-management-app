using time_off_management_app.Data;

namespace time_off_management_app.Models
{
    public class TimeOffForm
    {
        public int Id { get; set; }

        public String UserId { get; set; }
        public ApplicationUser User { get; set; }

        public DateTime SubmissionDate { get; set; }
        public DateTime DateTimeFrom { get; set; }
        public DateTime DateTimeTo { get; set; }

        public int ReasonId { get; set; }
        public Reason TimeOffReason { get; set; }
        public String? ReasonDescription { get; set; }

        public ApprovalStatus Status { get; set; }
        public String? ApprovedById { get; set; }
        public ApplicationUser? ApprovedBy { get; set; }

        public String? Note { get; set; }
    }
}
