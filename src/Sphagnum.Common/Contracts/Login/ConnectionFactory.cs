using System;
using System.Threading.Tasks;
using Sphagnum.Common.Contracts.Infrastructure;
using Sphagnum.Common.Services;
using Sphagnum.Common.Utils;

namespace Sphagnum.Common.Contracts.Login
{
    public class ConnectionFactory
    {
        public int Port { get; set; }
        public string Hostname { get; set; } = string.Empty;
        public string Login { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public UserRights UserRights { get; set; }

        internal virtual async Task<IConnection> CreateDefaultConnected()
        {
            var conn = new DefaultConnection();
            await conn.ConnectAsync(Hostname, Port);
            return conn;
        }

        internal virtual IConnection CreateDefault()
        {
            return new DefaultConnection();
        }
        
        internal byte[] GetAuthPayload()
        {
            var data = new byte[Constants.HashedUserDataSizeInfBytes + Constants.HashedUserDataSizeInfBytes + 2];
            HashCalculator.Calc(Login).CopyTo(data, 0);
            HashCalculator.Calc(Password).CopyTo(data, Constants.HashedUserDataSizeInfBytes);
            BitConverter.TryWriteBytes(data.AsSpan(Constants.HashedUserDataSizeInfBytes + Constants.HashedUserDataSizeInfBytes), (ushort)UserRights);
            return data;
        }
    }
}
