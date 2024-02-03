using RabbitMQ.Client;
using System.Text;

var factory = new ConnectionFactory();
factory.Uri = new Uri("amqp://guest:guest@localhost:7028");
 var cnn = factory.CreateConnection();
var channel = cnn.CreateModel();

string exchangeName = "MExchange";
string routingKey = "M-routing-key";
var queueName = "MQueue";

channel.ExchangeDeclare(exchangeName, ExchangeType.Direct);
channel.QueueDeclare(queueName, false, false, false, null);
channel.QueueBind(queueName,exchangeName,routingKey,null);

var messageBody = Encoding.UTF8.GetBytes("User info for user 1");
channel.BasicPublish(exchangeName,routingKey,null,messageBody);

channel.Close();
cnn.Close();