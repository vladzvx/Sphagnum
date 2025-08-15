namespace Sphagnum.Server.Storage.Contracts.Wal.Interfaces
{
    public interface IWalWriter
    {
        public Task WriteData(byte[] data);
    }
}
