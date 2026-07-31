using Microsoft.AspNetCore.Identity;
using time_off_management_app.Models;

namespace time_off_management_app.Data
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        public int? DepartmentId { get; set; }
        public Department? Department { get; set; }
        public String? Position { get; set; }

        public int MaxDaysOff { get; set; }


        public List<TimeOffForm> TimeOffForms { get; set; } = new List<TimeOffForm>();
    }

}
