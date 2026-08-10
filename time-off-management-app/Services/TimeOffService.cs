using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using time_off_management_app.Data;
using time_off_management_app.Models;
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

        public async Task<List<TimeOffSummaryDto>> GetUpcomingTimeOffSummaryAsync(String userId)
        {
            var user = await _context.Users
                .Include(u => u.TimeOffForms)
                .ThenInclude(f => f.TimeOffReason)
                .FirstOrDefaultAsync(u => u.Id.Equals(userId));

            var upcomingLeaves = user.TimeOffForms
                .Where(f => f.DateTimeFrom.ToUniversalTime() > DateTime.UtcNow);

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
    }
}
