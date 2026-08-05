using time_off_management_app.Data;

namespace time_off_management_app.Models
{
    public class Position
    {
        public int Id { get; set; }
        public String PositionName { get; set; }
        public int Level { get; set; }


        public List<ApplicationUser> Users { get; set; } = new List<ApplicationUser>();
    }
}
