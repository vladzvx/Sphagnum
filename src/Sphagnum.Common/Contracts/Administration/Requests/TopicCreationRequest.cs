using Sphagnum.Common.Contracts.Administration.Enums;

namespace Sphagnum.Common.Contracts.Administration.Requests
{
    public readonly ref struct TopicCreationRequest
    {
        public readonly string TopicName;
        public readonly TopicType TopicType;
    }
}
