using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using time_off_management_app.Services;
using time_off_management_app.Shared.DTOs.Forms;
using time_off_management_app.Data;
using time_off_management_app.Shared.Enums;
using Microsoft.AspNetCore.Authorization;

namespace time_off_management_app.Controllers.V1
{
    [ApiController]
    [Authorize]
    [Route(ApiRoutes.V1Prefix + "forms")]
    public class FormsController : ControllerBase
    {
        private UserManager<ApplicationUser> _userManager;
        private TimeOffService _timeOffService;
        private ReasonsService _reasonsService;

        public FormsController(UserManager<ApplicationUser> userManager, TimeOffService timeOffService, ReasonsService reasonsService)
        {
            _userManager = userManager;
            _timeOffService = timeOffService;
            _reasonsService = reasonsService;
        }


        [HttpGet("me")]
        public async Task<IActionResult> GetForms([FromQuery] int year, [FromQuery] ApprovalStatus? status)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return Unauthorized();
            }

            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return NotFound();
            }

            var forms = await _timeOffService.GetForms(userId, year, status);

            return Ok(forms);
        }


        [HttpPost]
        public async Task<IActionResult> AddNewForm([FromBody] TimeOffFormInput input)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return Unauthorized();
            }

            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return NotFound();
            }

            var reason = await _reasonsService.GetReasonByCodeAsync(input.ReasonCode);

            if (reason == null)
            {
                return NotFound("Invalid Reason Code");
            }

            await _timeOffService.AddNewForm(userId, input, reason);

            return Created();
        }

        [HttpGet("review")]
        public async Task<IActionResult> GetManagedForms([FromQuery] int year, [FromQuery] String? search, [FromQuery] ApprovalStatus? status)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return Unauthorized();
            }

            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return NotFound();
            }

            var forms = await _timeOffService.GetReviewFormsAsync(userId, year, search, status);

            return Ok(forms);
        }



        [HttpPost("{id}/approve")]
        public async Task<IActionResult> Approve(int id, [FromBody] ReviewFormDto input)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return Unauthorized();
            }

            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return NotFound();
            }

            //TODO: check if user can approve form. Return forbidden if not

            var success = await _timeOffService.ReviewForm(id, input.Note, ApprovalStatus.Approved);

            if(success)
            {
                return NoContent();
            }else
            {
                return BadRequest("Form does not exist or is not pending");
            }
        }

        [HttpPost("{id}/deny")]
        public async Task<IActionResult> Deny(int id, [FromBody] ReviewFormDto input)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return Unauthorized();
            }

            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return NotFound();
            }

            //TODO: check if user can deny form. Return forbidden if not

            var success = await _timeOffService.ReviewForm(id, input.Note, ApprovalStatus.Denied);

            if (success)
            {
                return NoContent();
            }
            else
            {
                return BadRequest("Form does not exist or is not pending");
            }
        }


        [HttpPost("approve")]
        public async Task<IActionResult> ApproveMulti([FromBody] List<ReviewFormDto> input)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return Unauthorized();
            }

            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return NotFound();
            }

            //TODO: check if user can approve each form. Return Forbidden if not

            var success = await _timeOffService.ReviewForms(input, ApprovalStatus.Approved);

            if(success)
            {
                return NoContent();
            }else
            {
                return BadRequest("Bad input or form doesn't exist");
            }
        }

        [HttpPost("deny")]
        public async Task<IActionResult> DenyMulti([FromBody] List<ReviewFormDto> input)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return Unauthorized();
            }

            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return NotFound();
            }

            //TODO: check if user can approve each form. Return Forbidden if not

            var success = await _timeOffService.ReviewForms(input, ApprovalStatus.Denied);

            if (success)
            {
                return NoContent();
            }
            else
            {
                return BadRequest("Bad input or form doesn't exist");
            }
        }
    }
}
