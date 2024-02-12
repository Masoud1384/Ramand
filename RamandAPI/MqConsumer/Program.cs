using Microsoft.Extensions.DependencyInjection;
using RabbitDI.RabbitMqOperation;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;


var serviceCollection = new ServiceCollection();
serviceCollection.AddScoped<IRabbitmqRepository, RabbitMqRepository>();
var serviceProvider = serviceCollection.BuildServiceProvider();
var rabbitRepository = serviceProvider.GetService<IRabbitmqRepository>();

var factory = new ConnectionFactory();
factory.Uri = new Uri("amqp://guest:guest@localhost:5672");
var cnn = factory.CreateConnection();
var channel = cnn.CreateModel();


string exchangeName = "MExchange";
string routingKey = "M-routing-key";
var queueName = "MQueue";

channel.ExchangeDeclare(exchangeName, ExchangeType.Direct);
channel.QueueDeclare(queueName, false, false, false, null);
channel.QueueBind(queueName, exchangeName, routingKey, null);
channel.BasicQos(0, 1, false);
// prefix size is used for preventing the messages to have a fixed size 
// the second one means how many messages we would like to be sent to us at once and we can increase it to as many as we want 

var consumer = new EventingBasicConsumer(channel);

consumer.Received += rabbitRepository.ReceiverHandler;
var consumerTag = channel.BasicConsume(queueName, false, consumer);
Console.ReadLine();

channel.BasicCancel(consumerTag);
channel.Close();
cnn.Close();

