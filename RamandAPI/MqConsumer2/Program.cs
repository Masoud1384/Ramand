using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

// This consumer has built specifically for Dead letter queue


var factory = new ConnectionFactory();
factory.Uri = new Uri("amqp://guest:guest@localhost:5672");
var cnn = factory.CreateConnection();
var channel = cnn.CreateModel();

string exchangeName = "deadLetterExName";
string routingKey = "M-routing-key";
var queueName = "deadLetterQuName";

channel.ExchangeDeclare(exchangeName, ExchangeType.Direct);
channel.QueueDeclare(queueName, false, false, false, null);
channel.QueueBind(queueName, exchangeName, routingKey, null);
channel.BasicQos(0, 1, false);
var consumer = new EventingBasicConsumer(channel);
consumer.Received += (sender, args) =>
{
    
    var body = args.Body.ToArray();
    var message = Encoding.UTF8.GetString(body);
    Console.WriteLine(message);
    channel.BasicAck(args.DeliveryTag, false);
};
var consumerTag = channel.BasicConsume(queueName, false, consumer);
Console.ReadLine();
channel.BasicCancel(consumerTag);
channel.Close();
cnn.Close();