using Sphagnum.Server.Storage.Contracts;
using Sphagnum.Server.Storage.Contracts.Wal;
using Sphagnum.Server.Storage.Contracts.Wal.Interfaces;

namespace Sphagnum.Server.Storage.Services.Wal
{
    internal class WalCursorManager : IWalCursorManager
    {
        private readonly string _cursorPath;
        private byte[]? data;
        private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(0, 1);
        public WalCursorManager(StorageConfig storageConfig)
        {
            _cursorPath = Path.Combine(storageConfig.WalDirectory, "cursor");
        }

        public async ValueTask<WalCursor> GetCursor()
        {
            await _semaphore.WaitAsync();
            try
            {
                if (data == null)
                {
                    data = await File.ReadAllBytesAsync(_cursorPath);
                }
                var result = new WalCursor(BitConverter.ToInt64(data, 0), BitConverter.ToInt64(data, 8));
                return result;
            }
            finally { _semaphore.Release(); }
        }

        public async ValueTask SetCursor(WalCursor walCursor)
        {
            try
            {
                if (data == null)
                {
                    data = new byte[16];
                }
                BitConverter.TryWriteBytes(data.AsSpan(0, 8), walCursor.PageId);
                BitConverter.TryWriteBytes(data.AsSpan(8, 8), walCursor.CurrentPosition);
                await File.WriteAllBytesAsync(_cursorPath, data);
            }
            finally { _semaphore.Release(); }
        }
    }
}
