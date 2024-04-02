namespace Sphagnum.Server.DataProcessing.Contracts
{
    internal interface IDataProcessor
    {
        public bool RegisterCoprocessor(string key, Func<byte[], Task> func);
        public Func<byte[], Task>? UnregisterCoprocessor(string key);
        public ValueTask PutMessage(ReadOnlySpan<byte> message);
    }
}
