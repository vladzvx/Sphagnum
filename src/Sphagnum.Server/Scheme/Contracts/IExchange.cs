namespace Sphagnum.Server.Scheme.Contracts
{
    public interface IExchange
    {
        public string Name { get; init; }

        public Task SendMessage();
    }
}
