using time_off_management_app.Data;

namespace time_off_management_app.Models
{
    public class Department
    {
        public int Id { get; set; }
        public String Name { get; set; }


        public bool IsActive { get; set; }

        public List<Unit> Units { get; set; } = new List<Unit>();
    }
}
