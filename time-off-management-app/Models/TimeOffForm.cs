using time_off_management_app.Data;
using time_off_management_app.Shared.DTOs.Forms;
using time_off_management_app.Shared.Enums;

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



        public TimeOffSummaryDto ToTimeOffSummaryDto()
        {
            return new TimeOffSummaryDto
            {
                Id = this.Id,
                Reason = this.TimeOffReason.ToReasonDto(),
                DateTimeFrom = this.DateTimeFrom,
                DateTimeTo = this.DateTimeTo,
                Status = this.Status
            };
        }

        public TimeOffFormDto ToTimeOffFormDto()
        {
            return new TimeOffFormDto
            {
                Id = this.Id,
                UserId = this.UserId,
                SubmissionDate = this.SubmissionDate,
                DateTimeFrom = this.DateTimeFrom,
                DateTimeTo = this.DateTimeTo,
                TimeOffReason = this.TimeOffReason.ToReasonDto(),
                ReasonDescription = this.ReasonDescription,
                Status = this.Status,
                ApprovedById = this.ApprovedById,
                ApprovedByFullName = this.ApprovedBy == null ? null : this.ApprovedBy.FullName,
                Note = this.Note
            };
        }
    }
}
