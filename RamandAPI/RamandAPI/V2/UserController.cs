using Application.UserOperations.Commands;
using Application.UserOperations.IRepositoryApplication;
using Domain.IRepositories;
using Microsoft.AspNetCore.Mvc;

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
                return Ok(updateUserCommand);
            }
            return BadRequest();
        }
    }
}
