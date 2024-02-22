using Sphagnum.Common.Contracts.Administration.Enums;

namespace Sphagnum.Common.Contracts.Administration.Requests
{
    public readonly ref struct ExchangeCreationRequest
    {
        public readonly string ExchangeName;
        public readonly ExchangeType ExchangeType;
    }
}
