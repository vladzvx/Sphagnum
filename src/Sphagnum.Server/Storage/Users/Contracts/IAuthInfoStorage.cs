using Sphagnum.Common.Contracts.Login;

namespace Sphagnum.Server.Storage.Users.Contracts
{
    internal interface IAuthInfoStorage
    {
        public ValueTask<bool> CheckRights(Span<byte> hashedUsername, Span<byte> hashedPassword, UserRights userRights, CancellationToken token = default);

        public ValueTask AddUser(Span<byte> hashedUsername, Span<byte> hashedPassword, UserRights userRights);

        public ValueTask SetRights(Span<byte> hashedUsername, UserRights userRights);
    }
}
