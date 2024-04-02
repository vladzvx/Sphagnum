using Sphagnum.Common.Contracts.Login;
using Sphagnum.Common.Utils;
using System;

namespace Sphagnum.Common.Contracts.Messaging.Messages
{
    internal readonly ref struct AuthMessage
    {
        public readonly ReadOnlyMemory<byte> Payload;
        public readonly Guid MessageId;

        public AuthMessage(string login, string pwd, UserRights userRights)
        {
            MessageId = Guid.NewGuid();
            var data = new byte[Constants.HashedUserDataSizeInfBytes + Constants.HashedUserDataSizeInfBytes + 2];
            HashCalculator.Calc(login).CopyTo(data, 0);
            HashCalculator.Calc(pwd).CopyTo(data, Constants.HashedUserDataSizeInfBytes);
            BitConverter.TryWriteBytes(data.AsSpan(Constants.HashedUserDataSizeInfBytes + Constants.HashedUserDataSizeInfBytes), (ushort)userRights);
            Payload = data;
        }
    }
}
