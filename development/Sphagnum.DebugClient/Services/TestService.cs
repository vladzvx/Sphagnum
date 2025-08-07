
using Sphagnum.Client;

namespace Sphagnum.DebugClient.Services
{
    public class TestService : IHostedService
    {
        private readonly CancellationTokenSource _cts;
        private readonly ClientDefault _clientDefault;
        private readonly Task _consumingTask;
        public TestService(ClientDefault clientDefault)
        {
            _cts = new CancellationTokenSource();
            _clientDefault = clientDefault;
            _consumingTask = Consuming();
        }

        private async Task Consuming()
        {
            while (!_cts.IsCancellationRequested)
            {
                var message = await _clientDefault.Consume(_cts.Token);


                await _clientDefault.Ack(message.MessageId);
            }
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _cts.Cancel();
            return Task.CompletedTask;
        }
    }
}
