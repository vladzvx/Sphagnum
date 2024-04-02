namespace Sphagnum.Common.Utils.Models
{
    internal enum MessageType : byte
    {
        Unknown = 0,
        Auth = 1,

        AuthSuccessfull = 2,
        AuthSuccessFailed = 3,

        Common = 4,
        Ack = 6,
        Nack = 7,
        Reject = 8,

        MessageAccepted = 5,
    }
}
