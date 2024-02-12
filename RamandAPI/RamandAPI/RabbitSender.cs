using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Threading.Channels;

namespace RamandAPI
{
    public static class RabbitSender
    {
        private static IConnection _cnn;
        private static IModel Channel;

        public static IModel init()
        {
            var factory = new ConnectionFactory();
            factory.Uri = new Uri("amqp://guest:guest@localhost:5672");
            _cnn = factory.CreateConnection();
            Channel = _cnn.CreateModel();

            string exchangeName = "MExchange";
            string routingKey = "M-routing-key";
            var queueName = "MQueue";

            Channel.ExchangeDeclare(exchangeName, ExchangeType.Direct);
            Channel.QueueDeclare(queueName, false, false, false, null);

            Channel.QueueBind(queueName, exchangeName, routingKey, null);
            return Channel;
        }
        public static IModel getterChannel()
        {
            return Channel;
        }
    }

}
