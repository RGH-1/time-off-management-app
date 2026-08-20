using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using time_off_management_app.Services;
using time_off_management_app.Shared.Constants;
using time_off_management_app.Shared.Enums;

namespace time_off_management_app.Controllers.V1
{
    [ApiController]
    [Authorize(AuthenticationSchemes = "ApiKey")]
    [Route(ApiRoutes.V1Prefix + "ext")]
    public class ExternalController : ControllerBase
    {
        private ExternalUseFormService _externalUseFormService;

        public ExternalController(ExternalUseFormService externalUseFormService)
        {
            _externalUseFormService = externalUseFormService;
        }


        /// <summary>
        /// Fetching User Requests based on parameters
        /// </summary>
        /// <param name="name">Full or Partial name of user</param>
        /// <param name="from">from Date (if to is null it will fetch all requests in dates after)</param>
        /// <param name="to">to Date (if from is null it will fetch all requests in previous dates)</param>
        /// <param name="date">only include requests made in this date. Overrides from and to</param>
        /// <returns>List of TimeOffFormDto of matching requests</returns>
        /// <response code="200">Returns the list of requests</response>
        /// <response code="404">If there is no result matching the filters</response>
        [HttpGet("requests")]
        public async Task<IActionResult> GetRequests([FromQuery] string? name, [FromQuery] DateTime? from, [FromQuery] DateTime? to, [FromQuery] DateTime? date)
        {
            var list = await _externalUseFormService.GetExtFormsAsync(name, from, to, date);
            if(list == null)
            {
                return NotFound("No Matching Results");
            }else
            {
                return Ok(list);
            }
        }



        /// <summary>
        /// Fetches all requests based on ApprovalStatus in the route. Filtered by parameters
        /// </summary>
        /// <param name="status">ApprovalStatus at the end of the route</param>
        /// <param name="date">Only include requests made in this exact date. Overrides from and to</param>
        /// <param name="from">from Date (if to is null it will fetch all requests in dates after)</param>
        /// <param name="to">to Date (if from is null it will fetch all requests in previous dates)</param>
        /// <param name="department">Filtering by a specific department in the company. Includes all Units in the department</param>
        /// <returns>List of TimeOffFormDto of matching requests</returns>
        /// <response code="200">Returns the list of requests</response>
        /// <response code="404">If there is no result matching the filters</response>
        [HttpGet("requests/{status}")]
        public async Task<IActionResult> GetStatusRequests(ApprovalStatus status, 
            [FromQuery] DateTime? date, [FromQuery] DateTime? from, [FromQuery] DateTime? to, [FromQuery] String? department)
        {
            var list = await _externalUseFormService.GetStatusFormAsync(status, from, to, date, department);
            if (list == null)
            {
                return NotFound("No Matching Results");
            }
            else
            {
                return Ok(list);
            }
        }



        /// <summary>
        /// Fetch the most requested day off of the specified year
        /// </summary>
        /// <param name="year">Year that you want to fetch the day of</param>
        /// <returns>The most requested day and the number of times it was requested</returns>
        /// <response code="200">Returns the most requested day and count of requests that day</response>
        /// <response code="404">If there is no result matching the year</response>
        /// <response code="400">If the year is outside the accepted range</response>
        [HttpGet("most_day/{year}")]
        public async Task<IActionResult> GetMostRequestedDay(int year)
        {
            if(year < ApplicationConstants.StartingYear || year > ApplicationConstants.CurrentYear)
            {
                return BadRequest("Year does not fall within accepted range");
            }

            var day = await _externalUseFormService.GetMostRequestedDayAsync(year);

            if(day == null)
            {
                return NotFound("No Requests in the specified year");
            }
            return Ok(day);
        }
    }
}

/*
 * Documentation for External Use:
 * Structure:
 * - endpoint : brief description
 *   - Class1
 *     - Class11 used in Class1
 *     - Class12 used in Class1
 * 
 * 
 * Endpoints:
 * - api/v1/ext/requests : returns all requests filtered by query parameters. Returns List<TimeOffFormDto> on success
 *   - Shared.DTOs.Forms.TimeOffFormDto
 *     - Shared.DTOs.Reasons.ReasonDto
 *     - Shared.Enums.ApprovalStatus
 * 
 * 
 * - api/v1/ext/requests/Approved | Pending | Denied : returns all requests of the status requested filtered by the query parameters. Returns List<TimeOffFormDto> on success
 *   - Shared.Enums.ApprovalStatus
 *   - Shared.DTOs.Forms.TimeOffFormDto
 *     - Shared.DTOs.Reasons.ReasonDto
 * 
 * 
 * - api/v1/ext/most_day/{year} : Returns the most requested day in the specified year and the number of requests.
 *   - Shared.DTOs.External.MostRequestedDayDto
 *     
*/