namespace time_off_management_app.Shared.DTOs.Reasons
{
    public class ReasonDto
    {
        public int Id { get; set; }
        public String Name { get; set; }

        public String Code { get; set; }
        public bool IsAnnualLeave { get; set; }
    }
}
