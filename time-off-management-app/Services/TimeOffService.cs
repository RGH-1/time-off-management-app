using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using time_off_management_app.Data;
using time_off_management_app.Models;
using time_off_management_app.Shared.Constants;
using time_off_management_app.Shared.DTOs.Forms;
using time_off_management_app.Shared.Enums;

namespace time_off_management_app.Services
{
    public class TimeOffService
    {
        private UserManager<ApplicationUser> _userManager;
        private ApplicationDbContext _context;

        public TimeOffService(UserManager<ApplicationUser> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }


        public async Task<List<TimeOffFormDto>> GetForms(String userId, int year, ApprovalStatus? status)
        {
            var user = await _context.Users
                .Include(u => u.TimeOffForms)
                .ThenInclude(f => f.TimeOffReason)
                .FirstOrDefaultAsync(u => u.Id.Equals(userId));

            var forms = user.TimeOffForms
                .Where(f => f.DateTimeFrom.Year == year);

            if(status != null)
            {
                forms = forms.Where(f => f.Status == status);
            }

            return forms.Select(f => f.ToTimeOffFormDto()).ToList();
        }


        public async Task AddNewForm(String userId, TimeOffFormInput input, Reason reason)
        {
            var user = await _context.Users.FindAsync(userId);

            TimeOffForm form = new TimeOffForm()
            {
                SubmissionDate = DateTime.Now,
                DateTimeFrom = input.DateTimeFrom,
                DateTimeTo = input.DateTimeTo,
                TimeOffReason = reason,
                ReasonDescription = input.ReasonDescription,
                Status = ApprovalStatus.Pending,
            };


            user.TimeOffForms.Add(form);

            await _context.SaveChangesAsync();
        }

        public async Task<List<TimeOffSummaryDto>> GetUpcomingTimeOffSummaryAsync(String userId, int limit = ApplicationConstants.SummaryLimit)
        {
            var user = await _context.Users
                .Include(u => u.TimeOffForms)
                .ThenInclude(f => f.TimeOffReason)
                .FirstOrDefaultAsync(u => u.Id.Equals(userId));

            var upcomingLeaves = user.TimeOffForms
                .Where(f => f.DateTimeFrom.ToUniversalTime() > DateTime.UtcNow)
                .OrderBy(f => f.DateTimeFrom)
                .Take(limit);

            List<TimeOffSummaryDto> timeOffsDtos = upcomingLeaves.Select(f => f.ToTimeOffSummaryDto()).ToList();

            return timeOffsDtos;
        }

        public async Task<int> GetYearlyLeaveAsync(String userId, bool isAnnual, DateTime? date = null)
        {
            var user = await _context.Users
                .Include(u => u.TimeOffForms)
                .ThenInclude(f => f.TimeOffReason)
                .FirstOrDefaultAsync(u => u.Id.Equals(userId));

            var approvedRequestsCurYear = user.TimeOffForms.Where(f => f.Status == ApprovalStatus.Approved)
                .Where(f =>
                {
                    if (isAnnual)
                    {
                        return f.TimeOffReason.IsAnnualLeave;
                    }else
                    {
                        return !f.TimeOffReason.IsAnnualLeave;
                    }
                })
                .Where(f =>
                {
                    if(date != null)
                    {
                        return f.DateTimeFrom.Year == date.Value.Year;
                    }else
                    {
                        return f.DateTimeFrom.Year == DateTime.Now.Year;
                    }
                })
                .Sum(f => Utils.BusinessDateCalculator.GetWorkDays(f.DateTimeFrom, f.DateTimeTo));

            return approvedRequestsCurYear;
        }


        public async Task<List<TimeOffReviewDto>> GetReviewFormsAsync(String userId, int year, String? search, ApprovalStatus? status)
        {
            var user = await _context.Users
                .Include(u => u.Position)
                .Include(u => u.Unit)
                .FirstOrDefaultAsync(u => u.Id.Equals(userId));

            if(user!.Unit == null || user.Position == null)
            {
                return new();
            }

            var higherLevelDepartmentUsers = _context.Users
                .Where(u => u.Unit != null && u.Unit.DepartmentId == user.Unit.DepartmentId)
                .Where(u => u.Position != null && u.Position.Level > user.Position.Level);
                

            if (!String.IsNullOrEmpty(search))
            {
                higherLevelDepartmentUsers = higherLevelDepartmentUsers.Where(u => EF.Functions.Like(u.FullName, $"%{search}%"));
            }

            higherLevelDepartmentUsers = higherLevelDepartmentUsers
                .Include(u => u.TimeOffForms)
                .ThenInclude(f => f.TimeOffReason);

            var users = await higherLevelDepartmentUsers.ToListAsync();

            var forms = users
                .SelectMany(u => u.TimeOffForms)
                .Where(f => f.DateTimeFrom.Year == year);

            if(status != null)
            {
                forms = forms.Where(f => f.Status == status);
            }

            return forms.Select(f => f.ToTimeOffReviewDto()).ToList();
        }
    }
}
