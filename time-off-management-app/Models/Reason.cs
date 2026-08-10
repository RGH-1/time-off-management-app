using time_off_management_app.Shared.DTOs.Reasons;

namespace time_off_management_app.Models
{
    public class Reason
    {
        public int Id { get; set; }
        public String Name { get; set; }

        public String Code { get; set; }
        public bool IsAnnualLeave { get; set; }

        public List<TimeOffForm> TimeOffForms { get; set; }


        public ReasonDto ToReasonDto()
        {
            return new ReasonDto
            {
                Id = this.Id,
                Name = this.Name,
                Code = this.Code,
                IsAnnualLeave = this.IsAnnualLeave
            };
        }
    }
}
