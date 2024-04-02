using Sphagnum.Common.Contracts.Login;
using Sphagnum.Common.Utils;
using Sphagnum.Server.Storage.Users.Contracts;
using System.Numerics;

namespace Sphagnum.Server.Storage.Users.Services
{
    internal class AuthInfoStorageBase : IAuthInfoStorage
    {
        private readonly Vector<byte> RootUserLogin = new(HashCalculator.Calc("root"));
        private readonly Vector<byte> RootUserPassword = new(HashCalculator.Calc("root"));
        private readonly UserRights RootUserRights = UserRights.All;
        public ValueTask AddUser(Span<byte> hashedUsername, Span<byte> hashedPassword, UserRights userRights)
        {
            return ValueTask.CompletedTask;
        }

        public ValueTask<bool> CheckRights(Span<byte> hashedUsername, Span<byte> hashedPassword, UserRights userRights, CancellationToken token = default)
        {
            var username = new Vector<byte>(hashedUsername);
            var pwd = new Vector<byte>(hashedPassword);
            return ValueTask.FromResult(username == RootUserLogin && pwd == RootUserPassword && (userRights & RootUserRights) == userRights);
        }

        public ValueTask SetRights(Span<byte> hashedUsername, UserRights userRights)
        {
            return ValueTask.CompletedTask;
        }
    }
}
