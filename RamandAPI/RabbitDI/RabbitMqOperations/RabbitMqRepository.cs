using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Threading.Channels;

namespace RabbitDI.RabbitMqOperation
{
    public class RabbitMqRepository : IRabbitmqRepository
    {

        public void ReceiverHandler(object? sender, BasicDeliverEventArgs args)
        {
            var key = args.RoutingKey;
            var body = args.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            Console.WriteLine(message);
            Console.WriteLine(key);

            var consumer = sender as EventingBasicConsumer;
            consumer?.Model.BasicAck(args.DeliveryTag, false);
        }

    }
}
