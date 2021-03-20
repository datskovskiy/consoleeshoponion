using System.Runtime.Serialization;

namespace EShopOnion.DataAccess.Enums
{
    public enum OrderStatus
    {
        [EnumMember(Value = "New")]
        New,

        [EnumMember(Value = "Canceled by Administrator")]
        CanceledByAdministrator,

        [EnumMember(Value = "Payment received")]
        PaymentReceived,

        [EnumMember(Value = "Sent")]
        Sent,

        [EnumMember(Value = "Received")]
        Received,

        [EnumMember(Value = "Completed")]
        Completed,

        [EnumMember(Value = "Canceled by user")]
        CanceledByUser
    }
}
