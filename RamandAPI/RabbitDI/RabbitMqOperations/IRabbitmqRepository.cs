using RabbitMQ.Client.Events;

namespace RabbitDI.RabbitMqOperation
{
    public interface IRabbitmqRepository
    {
        void ReceiverHandler(object? sender, BasicDeliverEventArgs args);
    }
}
