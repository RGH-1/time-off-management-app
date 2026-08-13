using System;
using System.Collections.Generic;
using System.Text;
using time_off_management_app.Shared.Enums;

namespace time_off_management_app.Shared.Constants
{
    public class ApplicationConstants
    {
        public const string OtherCode = "OT01";
        public const int SummaryLimit = 5;
        public const int StartingYear = 2023;
        public static int CurrentYear { get => DateTime.Now.Year; } 
        public static readonly List<ApprovalStatus> ApprovalStatusList = [ApprovalStatus.Pending, ApprovalStatus.Approved, ApprovalStatus.Denied];
    }
}
