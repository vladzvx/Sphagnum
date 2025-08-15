using Sphagnum.Server.Storage.Contracts.Wal.Interfaces;

namespace Sphagnum.Server.Storage.Services.Wal
{
    internal class WalStreamSource : IWalStreamSource
    {
        public Stream GetStream(string pathToFile)
        {
            var stream = new FileStream(pathToFile, FileMode.OpenOrCreate, FileAccess.Write);
            return stream;
        }
    }
}
