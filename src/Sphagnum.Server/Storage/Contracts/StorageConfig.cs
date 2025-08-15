namespace Sphagnum.Server.Storage.Contracts
{
    public class StorageConfig
    {
        public readonly string WalDirectory;
        public readonly long WalPageMinSize = 1024 * 4;
    }
}
