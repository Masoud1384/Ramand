using Application.UserOperations.Commands;
using Application.UserOperations.IRepositoryApplication;
using Domain.IRepositories;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;

namespace RamandAPI.V2
{
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class UserController : V1.UserController
    {
        private readonly IUserRepositoryApplication _userRepositoryApplication;
        public UserController(IUserRepositoryApplication userRepository, ITokenRepository tokenRepository, IConfiguration configuration) : base(userRepository, tokenRepository, configuration)
        {
            _userRepositoryApplication = userRepository;
        }

        [HttpGet]
        public IActionResult Get(int id)
        {
            var user = _userRepositoryApplication.GetUserBy(id);
            if (user != null)
            {
                user.links = new List<HATEOSDto>
                {
                    new HATEOSDto
                    {
                        hrref = Url.Action(nameof(Get),"User",new {user.Id },Request.Scheme),
                        Method = "GET"
                    },
                    new HATEOSDto
                    {
                        hrref = Url.Action(nameof(Delete),"User",new {username = user.Username},Request.Scheme),
                        Method = "DELETE"
                    }
                };
                return Ok(user);
            }
            return BadRequest();
        }

        [HttpGet("getpr")]
        public IActionResult Get()
        {
            var users = _userRepositoryApplication.GetAll();
            if (users.Count() > 0)
            {
                users.Select(u => u.links = new List<HATEOSDto>
                {
                    new HATEOSDto
                    {
                        hrref = Url.Action(nameof(Get),"User",new {u.Id },Request.Scheme),
                        Method = "GET"
                    },
                    new HATEOSDto
                    {
                        hrref = Url.Action(nameof(V1.UserController.Delete),"User",new {username = u.Username},Request.Scheme),
                        Method = "DELETE"
                    }
                });
                return Ok(users);
            }
            return NotFound();
        }

        [HttpPost("PostUser")]
        public IActionResult Post(CreateUserCommand createUserCommand)
        {
            var uservm = _userRepositoryApplication.Create(createUserCommand);
            if (uservm != null)
            {
                var url = Url.Action(nameof(Get), "User", new { id = uservm.Id }, Request.Scheme);
                return Created("user added", new { uservm });
            }
            return BadRequest();
        }

        [HttpPut]
        public IActionResult Put(UpdateUserCommand updateUserCommand)
        {
            var isUserUpdated = _userRepositoryApplication.Update(updateUserCommand);
            if (isUserUpdated)
            {
                string url = Url.Action(nameof(Get), "Get updated user", new { userId = updateUserCommand.userId }, Request.Scheme);
                return Ok(url);
            }
            return BadRequest();
        }

        [HttpPost]
        public IActionResult MqSender()
        {
            var user = _userRepositoryApplication.GetUserBy(1); 
            // var user = new UserVM(1, "@Admin22", "Masoud84");
            DataSender(user);
            return Created("queue sent", new {user});
        }

        private void DataSender(UserVM userVm)
        {
            var factory = new ConnectionFactory();
            factory.Uri = new Uri("amqp://guest:guest@localhost:5672");
            var cnn = factory.CreateConnection();
            var channel = cnn.CreateModel();

            string exchangeName = "MExchange";
            string routingKey = "M-routing-key";
            var queueName = "MQueue";

            var deadLetterExchangeName = "deadLetterExName";
            var deadLetterQuName = "deadLetterQuName";



            channel.ExchangeDeclare(exchangeName, ExchangeType.Direct);
            channel.ExchangeDeclare(deadLetterExchangeName, ExchangeType.Direct);
            // and we build an oher line in order to send the data to it after 10 seconds if it wouldn't proccessed 
            var arguments = new Dictionary<string, object>
            {
                { "x-dead-letter-exchange", deadLetterExchangeName }
            };
            channel.QueueDeclare(queueName, false, false, false, arguments);
            channel.QueueDeclare(deadLetterQuName, false, false, false, null);

            channel.QueueBind(queueName, exchangeName, routingKey, null);
            channel.QueueBind(deadLetterQuName, deadLetterExchangeName, routingKey, null);


            var jsonData = JsonConvert.SerializeObject(userVm);
            var messageBody = Encoding.UTF8.GetBytes(jsonData);

            var properties = channel.CreateBasicProperties();
            properties.Persistent = true;

            // we set the message TTL to 10 seconds
            properties.Expiration = "10000";

            channel.BasicPublish(exchangeName, routingKey, properties, messageBody);

            channel.Close();
            cnn.Close();
        }
    }
}