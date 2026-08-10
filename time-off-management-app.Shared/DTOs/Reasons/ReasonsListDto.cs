using System;
using System.Collections.Generic;
using System.Text;

namespace time_off_management_app.Shared.DTOs.Reasons
{
    public class ReasonsListDto
    {
        public ReasonDto Other { get; set; }
        public List<ReasonDto> Reasons { get; set; }
    }
}
