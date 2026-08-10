namespace time_off_management_app.Utils
{
    public class BusinessDateCalculator
    {
        public static int GetWorkDays(DateTime startDate, DateTime endDate, HashSet<DateTime> holidays = null)
        {
            if (startDate > endDate)
            {
                var temp = startDate;
                startDate = endDate;
                endDate = temp;
            }

            int workDays = 0;
            DateTime current = startDate.Date;
            DateTime final = endDate.Date;

            while(current <= final)
            {
                if(current.DayOfWeek != DayOfWeek.Saturday && current.DayOfWeek != DayOfWeek.Sunday)
                {
                    if(holidays == null || !holidays.Contains(current))
                    {
                        workDays++;
                    }
                }
                current = current.AddDays(1);
            }

            return workDays;
        }
    }
}
