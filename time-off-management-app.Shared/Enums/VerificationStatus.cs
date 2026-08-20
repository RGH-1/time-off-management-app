using System.Runtime.Serialization;

namespace time_off_management_app.Shared.Enums
{
    public enum VerificationStatus
    {
        [EnumMember(Value = "Approved")]
        Approved,

        [EnumMember(Value = "Denied")]
        Denied,

        [EnumMember(Value = "Pending")]
        Pending,

        [EnumMember(Value = "NotRequested")]
        NotRequested
    }
}
