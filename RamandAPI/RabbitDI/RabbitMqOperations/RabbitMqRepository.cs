using Newtonsoft.Json;
using RabbitMQ.Client.Events;
using System.Text;

namespace RabbitDI.RabbitMqOperation
{
    public class RabbitMqRepository : IRabbitmqRepository
    {
        public void ReceiverHandler(object? sender, BasicDeliverEventArgs args)
        {
            var body = args.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            Console.WriteLine(message);
            var consumer = sender as EventingBasicConsumer;
            consumer?.Model.BasicAck(args.DeliveryTag, false);
        }
    }
}