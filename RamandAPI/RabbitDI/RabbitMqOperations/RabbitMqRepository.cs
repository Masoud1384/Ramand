using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace RabbitDI.RabbitMqOperation
{
    public class RabbitMqRepository : IRabbitmqRepository
    {
        public void ReceiverFucntion(IModel channel, EventingBasicConsumer consumer)
        {
            consumer.Received += (sender, args) =>
            {
                var body = args.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                Console.WriteLine(message);
                channel.BasicAck(args.DeliveryTag, false);
                // and we give back the delivery tag which we got at first and now we are impling on success stage of reciving the message
            };
        }
    }
}
