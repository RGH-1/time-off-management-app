using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using time_off_management_app.Shared.Enums;
using time_off_management_app.Data;
using time_off_management_app.Services;
using time_off_management_app.Shared.DTOs.Users;

namespace time_off_management_app.Controllers.V1
{
    [ApiController]
    [Authorize]
    [Route(ApiRoutes.V1Prefix + "users")]
    public class UserController : ControllerBase
    {
        private UserManager<ApplicationUser> _userManager;
        private TimeOffService _timeOffService;
        public UserController(UserManager<ApplicationUser> userManager, TimeOffService timeOffService)
        {
            _userManager = userManager;
            _timeOffService = timeOffService;
        }


        [HttpGet("fullname")]
        public async Task<IActionResult> GetFullName()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if(userId == null)
            {
                return Unauthorized();
            }

            ApplicationUser user = await _userManager.FindByIdAsync(userId);

            if(user == null)
            {
                return NotFound();
            }

            return Ok(new NameDto()
            {
                FullName = user.FullName
            });
        }


        [HttpGet("dashboard")]
        public async Task<IActionResult> GetDashboard()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if(userId == null)
            {
                return Unauthorized();
            }

            ApplicationUser user = await _userManager.FindByIdAsync(userId)!;

            if(user == null)
            {
                return NotFound();
            }

            var upcomingLeaves = await _timeOffService.GetUpcomingTimeOffSummaryAsync(userId);
            var usedAnnualLeave = await _timeOffService.GetYearlyLeaveAsync(userId, true);
            var usedOtherLeave = await _timeOffService.GetYearlyLeaveAsync(userId, false);


            UserDashboardDto toReturn = new UserDashboardDto
            {
                FullName = user.FullName,
                RemainingAnnualLeaveDays = user.MaxAnnualLeaveDays - usedAnnualLeave,
                MaxAnnualLeaveDays = user.MaxAnnualLeaveDays,
                RemainingOtherLeaveDays = user.MaxOtherLeaveDays - usedOtherLeave,
                MaxOtherLeaveDays = user.MaxOtherLeaveDays,
                UpcomingTimeOff = upcomingLeaves == null ? new() : upcomingLeaves
            };
            return Ok(toReturn);
        }
    }
}
