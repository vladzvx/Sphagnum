using Sphagnum.Common.Contracts.Administration.Requests;
using System.Threading.Tasks;

namespace Sphagnum.Common.Contracts.Administration
{
    public interface IAdministrationClient
    {
        public ValueTask CreateExchange(ExchangeCreationRequest exchangeCreationRequest);
        public ValueTask CreateTopic(TopicCreationRequest topicCreationRequest);
        public ValueTask DeleteTopic(string topicName);
        public ValueTask DeleteExchange(string exchangeName);
        public ValueTask BindTopic(TopicBindingRequest topicCreationRequest);
        public ValueTask UnbindTopic(TopicBindingRequest topicCreationRequest);
    }
}
