using Microsoft.EntityFrameworkCore;
using time_off_management_app.Data;
using time_off_management_app.Models;
using time_off_management_app.Shared.DTOs.External;
using time_off_management_app.Shared.DTOs.Forms;
using time_off_management_app.Shared.Enums;

namespace time_off_management_app.Services
{
    public class ExternalUseFormService
    {

        private ApplicationDbContext _context;

        public ExternalUseFormService(ApplicationDbContext context)
        {
            _context = context;
        }


        public async Task<MostRequestedDayDto?> GetMostRequestedDayAsync(int year)
        {
            var start = new DateTime(year, 1, 1);
            var end = start.AddYears(1);

            var forms = await _context.TimeOffForm
                .Where(f => f.DateTimeFrom < end && f.DateTimeTo >= start)
                .Select(f => new
                {
                    DateTimeFrom = f.DateTimeFrom < start ? start.Date : f.DateTimeFrom.Date,
                    DateTimeTo = f.DateTimeTo >= end ? end.AddDays(-1).Date : f.DateTimeTo.Date,
                }).ToListAsync();

            var dayCount = new Dictionary<DateTime, int>();

            foreach(var form in forms)
            {
                for(var date = form.DateTimeFrom; date <= form.DateTimeTo; date = date.AddDays(1))
                {
                    dayCount.TryGetValue(date, out var count);
                    dayCount[date] = count + 1;
                }
            }

            if(dayCount.Count == 0)
            {
                return null;
            }

            var mostRequested = dayCount.MaxBy(x => x.Value);

            return new MostRequestedDayDto
            {
                Day = mostRequested.Key,
                NumberOfRequests = mostRequested.Value
            };
        }


        public async Task<List<TimeOffFormDto>?> GetExtFormsAsync(String? fullName, DateTime? from, DateTime? to, DateTime? date)
        {
            var forms = _context.TimeOffForm
                .Include(f => f.User)
                .Include(f => f.TimeOffReason)
                .Include(f => f.ApprovedBy)
                .Where(f => (fullName == null) || f.User.FullName.Contains(fullName));

            forms = FilterByDate(forms, from, to, date);

            if (forms == null)
            {
                return null;
            }
            else
            {
                return forms.Select(f => f.ToTimeOffFormDto()).ToList();
            }
        }


        public async Task<List<TimeOffFormDto>?> GetStatusFormAsync(ApprovalStatus? status, DateTime? from, DateTime? to, DateTime? date, String? department)
        {
            var forms = _context.TimeOffForm
                .Include(f => f.User)
                .ThenInclude(u => u.Unit)
                .ThenInclude(u => u.Department)
                .Include(f => f.TimeOffReason)
                .Include(f => f.ApprovedBy)
                .Where(f => f.Status == status);

            forms = FilterByDate(forms, from, to, date);

            if(department != null)
            {
                forms = forms.Where(f => (f.User.Unit != null && f.User.Unit.Department != null) && f.User.Unit.Department.Name.Contains(department));
            }

            if(forms == null)
            {
                return null;
            }else
            {
                return forms.Select(f => f.ToTimeOffFormDto()).ToList();
            }
        }


        private static IQueryable<TimeOffForm> FilterByDate(IQueryable<TimeOffForm> forms, DateTime? from, DateTime? to, DateTime? date)
        {
            if (date != null)
            {
                forms = forms.Where(f => f.DateTimeFrom <= date && f.DateTimeTo >= date);
            }
            else if (from != null && to != null)
            {
                forms = forms.Where(f => (from <= f.DateTimeFrom && f.DateTimeFrom <= to) || (from <= f.DateTimeTo && f.DateTimeTo <= to) || (f.DateTimeFrom <= from && f.DateTimeTo >= to));
            }
            else if (from != null && to == null)
            {
                forms = forms.Where(f => f.DateTimeFrom >= from);
            }
            else if (from == null && to != null)
            {
                forms = forms.Where(f => f.DateTimeTo <= to);
            }

            return forms;
        }
    }
}
