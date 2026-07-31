using time_off_management_app.Data;

namespace time_off_management_app.Models
{
    public class Department
    {
        public int Id { get; set; }
        public String Name { get; set; }

        public String? ManagerId { get; set; }
        public ApplicationUser? Manager { get; set; }

        public bool IsActive { get; set; }

        public List<ApplicationUser> Users { get; set; } = new List<ApplicationUser>(); 
    }
}
