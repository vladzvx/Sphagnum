using Sphagnum.Common.Old.Contracts.Login;
using System.Threading.Tasks;

namespace Sphagnum.Common.Infrastructure.Contracts
{
    public interface IConnectionFactory
    {
        public int Port { get; }
        public string Hostname { get; }
        public string Login { get; }
        public string Password { get; }
        public UserRights UserRights { get; set; }
        internal Task<IConnection> CreateConnection(bool connected = true);
    }
}
