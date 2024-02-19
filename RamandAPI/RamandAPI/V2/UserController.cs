using Application.UserOperations.Commands;
using Application.UserOperations.IRepositoryApplication;
using Domain.IRepositories;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RabbitDI.RabbitMqOperation;
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
        private readonly IMessagesRepository _messageRepository;
        public UserController(IMessagesRepository messagesRepository, IUserRepositoryApplication userRepository, ITokenRepository tokenRepository, IConfiguration configuration) : base(userRepository, tokenRepository, configuration)
        {
            _messageRepository = messagesRepository;
            _userRepositoryApplication = userRepository;
        }

        [HttpGet]
        public IActionResult Get(int id)
        {
            var user = _userRepositoryApplication.GetUserBy(id);
            if (user != null)
            {
                List<HATEOSDto> hATEOSDtos = new List<HATEOSDto>
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
                user.links = hATEOSDtos;
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
                users.Select(u =>
                {
                    List<HATEOSDto> hATEOSDtos = u.links = new List<HATEOSDto>
                {
                    new HATEOSDto
                    {
                        hrref = Url.Action(nameof(Get),"User",new {u.Id },Request.Scheme),
                        Method = "GET"
                    },
                    new HATEOSDto
                    {
                        hrref = Url.Action(nameof(Delete),"User",new {username = u.Username},Request.Scheme),
                        Method = "DELETE"
                    }
                };
                    return hATEOSDtos;
                });
                return Ok(users);
            }
            return NotFound();
        }

        [HttpPost("InsertUsers")]
        public IActionResult InsertMultipleUsers(List<CreateUserCommand> users)
        {
            var affectedColumns = _userRepositoryApplication.InsertUsers(users);
            return Ok(affectedColumns);
        }

        [HttpPut]
        public IActionResult Put(UpdateUserCommand updateUserCommand)
        {
            var isPasswordConfirmed = VerifyPassword(new LoginCommand(updateUserCommand.Username, updateUserCommand.Password));
            if (isPasswordConfirmed)
            {
                var isUserUpdated = _userRepositoryApplication.Update(updateUserCommand);
                if (isUserUpdated)
                {
                    return Ok(Url.Action(nameof(Get), "Get updated user", new { userId = updateUserCommand.userId }, Request.Scheme));
                }
            }
            return BadRequest();
        }

        [HttpPost]
        public IActionResult Post()
        {
            //var user = _userRepositoryApplication.GetUserBy(1); 
            var user = new UserVM(1, "@Admin22", "Masoud84");
            var jsonData = JsonConvert.SerializeObject(user);
            DataSender(jsonData);
            _messageRepository.InsertMessage(new Domain.Models.Messages(jsonData));
            return Created("queue sent", new { user });
        }

        private void DataSender(string message)
        {
            var messageBody = Encoding.UTF8.GetBytes(message);
            var channel = RabbitSender.getterChannel();

            if (!channel.IsOpen)
            {
                channel = RabbitSender.init();
            }

            channel.BasicPublish("MExchange", "M-routing-key", null, messageBody);

        }
    }
}