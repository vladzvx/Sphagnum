namespace Sphagnum.Common.Contracts.Administration.Enums
{
    public enum ExchangeType : byte
    {
        /// <summary>
        /// Раздает сообщения во все топики с подходящим ключём маршрутизации.
        /// </summary>
        Broadcast,
        /// <summary>
        /// Отправляет сообщение в одну из очередей с подходящим ключём маршрутизации.
        /// </summary>
        Topic,
    }
}
