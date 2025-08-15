using Microsoft.Extensions.Logging;
using Sphagnum.Server.Storage.Contracts;
using Sphagnum.Server.Storage.Contracts.Wal.Interfaces;
using System.Globalization;

namespace Sphagnum.Server.Storage.Services.Wal
{
    internal class WalWriterDefault : IWalWriter
    {
        private readonly SemaphoreSlim _semaphore = new(0, 1);

        private readonly string _walDirectory;
        private readonly long _walPageSize;

        private string? _walPagePath;
        private long CurrentPageSize = 0;
        private long CurrentPage = 0;

        private readonly IWalStreamSource _streamSource;
        private readonly ILogger<WalWriterDefault> _logger;
        public WalWriterDefault(StorageConfig storageConfig, IWalStreamSource streamSource, ILogger<WalWriterDefault> logger)
        {
            _streamSource = streamSource;
            _walDirectory = storageConfig.WalDirectory;
            _walPageSize = storageConfig.WalPageMinSize;
            _logger = logger;
        }

        public async Task WriteData(byte[] data)
        {
            if (!string.IsNullOrWhiteSpace(_walPagePath))
            {
                await _semaphore.WaitAsync();
                try
                {
                    if (CurrentPageSize > _walPageSize)
                    {
                        CurrentPageSize = 0;
                        CurrentPage += 1;
                        _walPagePath = Path.Combine(_walDirectory, CurrentPage.ToString(CultureInfo.InvariantCulture));
                    }
                    using var stream = _streamSource.GetStream(_walPagePath);
                    await stream.WriteAsync(data);
                    CurrentPageSize += data.LongLength;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Walwriting failed!");
                }
                _semaphore.Release();
            }
        }
    }
}
