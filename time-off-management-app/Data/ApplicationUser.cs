using Microsoft.AspNetCore.Identity;
using time_off_management_app.Models;

namespace time_off_management_app.Data
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        public String FullName { get; set; } = string.Empty;

        public int? UnitId { get; set; }
        public Unit? Unit { get; set; }

        public int? PositionId { get; set; }
        public Position? Position { get; set; }

        public int MaxAnnualLeaveDays { get; set; }
        public int MaxOtherLeaveDays { get; set; }

        public String? ManagerId { get; set; }
        public ApplicationUser? Manager { get; set; }


        public List<TimeOffForm> TimeOffForms { get; set; } = new List<TimeOffForm>();
    }

}
