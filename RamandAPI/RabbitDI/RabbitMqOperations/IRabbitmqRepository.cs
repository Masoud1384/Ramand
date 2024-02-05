using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace RabbitDI.RabbitMqOperation
{
    public interface IRabbitmqRepository
    {
        void ReceiverFucntion(IModel channel, EventingBasicConsumer consumer);
    }
}
