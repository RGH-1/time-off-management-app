using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using time_off_management_app.Services;
using time_off_management_app.Shared.DTOs.Forms;
using time_off_management_app.Data;
using time_off_management_app.Shared.Enums;
using Microsoft.AspNetCore.Authorization;
using time_off_management_app.Shared.Constants;

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

        [Authorize(Policy = "CanReview")]
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

            var canReviewAll = false;
            if(User.HasClaim(Permissions.Type, Permissions.FormsReviewAll))
            {
                canReviewAll = true;
            }

            var forms = await _timeOffService.GetReviewFormsAsync(userId, year, search, status, canReviewAll);

            return Ok(forms);
        }



        [Authorize(Policy = "CanReview")]
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



            if(!User.HasClaim(Permissions.Type, Permissions.FormsReviewAll))
            {
                var val = await _timeOffService.VerifyReviewRightsAsync(userId, id);

                if (!val.Success)
                {
                    return Forbid(val.Reason!);
                }
            }

            

            var success = await _timeOffService.ReviewForm(id, input.Note, ApprovalStatus.Approved, user);

            if(success)
            {
                return NoContent();
            }else
            {
                return BadRequest("Form does not exist or is not pending");
            }
        }

        [Authorize(Policy = "CanReview")]
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



            if (!User.HasClaim(Permissions.Type, Permissions.FormsReviewAll))
            {
                var val = await _timeOffService.VerifyReviewRightsAsync(userId, id);

                if (!val.Success)
                {
                    return Forbid(val.Reason!);
                }
            }
            


            var success = await _timeOffService.ReviewForm(id, input.Note, ApprovalStatus.Denied, user);

            if (success)
            {
                return NoContent();
            }
            else
            {
                return BadRequest("Form does not exist or is not pending");
            }
        }


        //[Authorize(Policy = "CanReview")]
        //[HttpPost("approve")]
        //public async Task<IActionResult> ApproveMulti([FromBody] List<ReviewFormDto> input)
        //{
        //    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        //    if (userId == null)
        //    {
        //        return Unauthorized();
        //    }

        //    var user = await _userManager.FindByIdAsync(userId);

        //    if (user == null)
        //    {
        //        return NotFound();
        //    }



        //    if (!User.HasClaim(Permissions.Type, Permissions.FormsReviewAll))
        //    {
        //        var forms = await _timeOffService.GetReviewFormsAsync(userId, ApplicationConstants.CurrentYear, null, null);
        //        var ids = forms.Select(f => f.Id).ToHashSet();

        //        foreach (var form in input)
        //        {
        //            if (form.FormId == null)
        //            {
        //                return BadRequest("Missing Id in one of the forms");
        //            }
        //            if (!ids.Contains((int)form.FormId!))
        //            {
        //                return Forbid($"User doesn't have the right to review form {form.FormId}");
        //            }
        //            var valres = await _timeOffService.VerifyReviewRightsAsync(userId, (int)form.FormId, forms);

        //            if (!valres.Success)
        //            {
        //                return Forbid(valres.Reason!);
        //            }
        //        }
        //    }



        //    var success = await _timeOffService.ReviewForms(input, ApprovalStatus.Approved, user);

        //    if(success)
        //    {
        //        return NoContent();
        //    }else
        //    {
        //        return BadRequest("Bad input or form doesn't exist");
        //    }
        //}

        //[Authorize(Policy = "CanReview")]
        //[HttpPost("deny")]
        //public async Task<IActionResult> DenyMulti([FromBody] List<ReviewFormDto> input)
        //{
        //    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        //    if (userId == null)
        //    {
        //        return Unauthorized();
        //    }

        //    var user = await _userManager.FindByIdAsync(userId);

        //    if (user == null)
        //    {
        //        return NotFound();
        //    }



        //    if (!User.HasClaim(Permissions.Type, Permissions.FormsReviewAll))
        //    {
        //        var forms = await _timeOffService.GetReviewFormsAsync(userId, ApplicationConstants.CurrentYear, null, null);
        //        var ids = forms.Select(f => f.Id).ToHashSet();

        //        foreach (var form in input)
        //        {
        //            if (form.FormId == null)
        //            {
        //                return BadRequest("Missing Id in one of the forms");
        //            }
        //            if (!ids.Contains((int)form.FormId!))
        //            {
        //                return Forbid($"User doesn't have the right to review form {form.FormId}");
        //            }
        //            var valres = await _timeOffService.VerifyReviewRightsAsync(userId, (int)form.FormId, forms);

        //            if (!valres.Success)
        //            {
        //                return Forbid(valres.Reason!);
        //            }
        //        }
        //    }
            


        //    var success = await _timeOffService.ReviewForms(input, ApprovalStatus.Denied, user);

        //    if (success)
        //    {
        //        return NoContent();
        //    }
        //    else
        //    {
        //        return BadRequest("Bad input or form doesn't exist");
        //    }
        //}
    }
}
