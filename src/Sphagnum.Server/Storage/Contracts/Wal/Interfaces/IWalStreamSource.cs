namespace Sphagnum.Server.Storage.Contracts.Wal.Interfaces
{
    internal interface IWalStreamSource
    {
        public Stream GetStream(string pathToFile);
    }
}
