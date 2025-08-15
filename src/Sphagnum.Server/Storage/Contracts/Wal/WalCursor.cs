namespace Sphagnum.Server.Storage.Contracts.Wal
{
    public readonly struct WalCursor
    {
        public readonly long PageId;
        public readonly long CurrentPosition;

        public WalCursor(long pageId, long currentPosition)
        {
            PageId = pageId;
            CurrentPosition = currentPosition;
        }
    }
}
