using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitDI.RabbitMqOperation;
using Microsoft.Extensions.DependencyInjection;

var serviceCollection = new ServiceCollection();
serviceCollection.AddScoped<IRabbitmqRepository, RabbitMqRepository>();
var serviceProvider = serviceCollection.BuildServiceProvider();
var rabbitRepository = serviceProvider.GetService<IRabbitmqRepository>();

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
rabbitRepository.ReceiverFucntion(channel, consumer);
var consumerTag = channel.BasicConsume(queueName, false, consumer);
Console.ReadLine();
channel.BasicCancel(consumerTag);
channel.Close();
cnn.Close();