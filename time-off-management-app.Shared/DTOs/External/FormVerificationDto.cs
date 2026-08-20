
using time_off_management_app.Shared.Enums;

namespace time_off_management_app.Shared.DTOs.External
{
    public class FormVerificationDto
    {
        public String Name { get; set; }
        public DateTime Date { get; set; }
        public VerificationStatus? Status { get; set; }
    }
}
