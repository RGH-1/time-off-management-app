using Microsoft.EntityFrameworkCore;
using time_off_management_app.Data;
using time_off_management_app.Models;
using time_off_management_app.Shared.Constants;
using time_off_management_app.Shared.DTOs.Reasons;

namespace time_off_management_app.Services
{
    public class ReasonsService
    {
        private ApplicationDbContext _context;

        public ReasonsService(ApplicationDbContext context)
        {
            _context = context;
        }


        public async Task<ReasonsListDto> GetReasonsAsync()
        {
            List<Reason> reasons = await _context.Reason.ToListAsync();
            var otherReason = reasons.FirstOrDefault(r => r.Code.Equals(ApplicationConstants.OtherCode));

            return new ReasonsListDto()
            {
                Other = otherReason!.ToReasonDto(),
                Reasons = reasons.Select(r => r.ToReasonDto()).ToList()
            };
        }


        public async Task<Reason?> GetReasonByCodeAsync(String Code)
        {
            var reason = await _context.Reason.FirstOrDefaultAsync(r => r.Code.Equals(Code));
            return reason;
        }
    }
}
