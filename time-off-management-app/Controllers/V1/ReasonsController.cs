using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using time_off_management_app.Data;
using time_off_management_app.Models;
using time_off_management_app.Services;
using time_off_management_app.Shared.Constants;
using time_off_management_app.Shared.DTOs.Reasons;

namespace time_off_management_app.Controllers.V1
{
    [ApiController]
    [Authorize]
    [Route(ApiRoutes.V1Prefix +  "reasons")]
    public class ReasonsController : ControllerBase
    {
        private ReasonsService _reasonsService;

        public ReasonsController(ReasonsService reasonsService)
        {
            _reasonsService = reasonsService;
        }


        [HttpGet]
        public async Task<IActionResult> GetReasons()
        {
            ReasonsListDto reasons = await _reasonsService.GetReasonsAsync();
            
            if(reasons == null)
            {
                return BadRequest();
            }

            return Ok(reasons);
        }
    }
}
