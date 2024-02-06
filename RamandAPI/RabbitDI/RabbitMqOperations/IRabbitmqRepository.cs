using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace RabbitDI.RabbitMqOperation
{
    public interface IRabbitmqRepository
    {
        public void ReceiverHandler(object? sender, BasicDeliverEventArgs args);
    }
}
