using Application.UserOperations.Commands;
using Application.UserOperations.IRepositoryApplication;
using Domain.IRepositories;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography.Xml;

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
        }
        public UserController(IUserRepositoryApplication userRepository)
            : base(userRepository)
        {
            this._userRepositoryApplication = userRepository;
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
                        hrref = Url.Action(nameof(V1.UserController.Delete),"User",new {username = user.Username},Request.Scheme),
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

        [HttpPost]
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
    }
}
