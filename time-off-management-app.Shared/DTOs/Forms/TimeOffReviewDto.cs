using System;
using System.Collections.Generic;
using System.Text;
using time_off_management_app.Shared.DTOs.Reasons;
using time_off_management_app.Shared.Enums;

namespace time_off_management_app.Shared.DTOs.Forms
{
    public class TimeOffReviewDto
    {
        public int Id { get; set; }
        public String FullName { get; set; }
        public DateTime DateTimeFrom { get; set; }
        public DateTime DateTimeTo { get; set; }
        public ReasonDto Reason { get; set; }
        public String? ReasonDescription { get; set; }
        public ApprovalStatus Status { get; set; }

        public String? Note { get; set; }


        public bool Selected { get; set; }
    }
}
