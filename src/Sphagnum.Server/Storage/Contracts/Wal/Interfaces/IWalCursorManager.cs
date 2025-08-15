namespace Sphagnum.Server.Storage.Contracts.Wal.Interfaces
{
    internal interface IWalCursorManager
    {
        ValueTask<WalCursor> GetCursor();
        ValueTask SetCursor(WalCursor walCursor);
    }
}