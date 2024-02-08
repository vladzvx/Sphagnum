using Sphagnum.Abstractions.Administration.Enums;

namespace Sphagnum.Abstractions.Administration.Requests
{
    public readonly struct TopicCreationRequest
    {
        public readonly string TopicName;
        public readonly TopicType TopicType;
    }
}
