using time_off_management_app.Data;

namespace time_off_management_app.Models
{
    public class Unit
    {
        public int Id { get; set; }
        public String Name { get; set; }

        public int DepartmentId { get; set; }
        public Department Department { get; set; }

        public List<ApplicationUser> Users { get; set; } = new List<ApplicationUser>();
    }
}
