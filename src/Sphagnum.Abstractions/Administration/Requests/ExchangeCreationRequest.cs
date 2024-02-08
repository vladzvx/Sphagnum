using Sphagnum.Abstractions.Administration.Enums;

namespace Sphagnum.Abstractions.Administration.Requests
{
    public readonly struct ExchangeCreationRequest
    {
        public readonly string ExchangeName;
        public readonly ExchangeType ExchangeType;
    }
}
