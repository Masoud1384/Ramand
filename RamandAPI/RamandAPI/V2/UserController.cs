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
            :base(userRepository)
        {
            this._userRepositoryApplication = userRepository;
        }

        [HttpGet]
        public IActionResult Get(int id)
        {
            return Ok();
        }

        [HttpGet("getpr")]

        public IActionResult Get()
        {
            return Ok();
        }

        [HttpPost]
        public IActionResult Post(CreateUserCommand createUserCommand)
        {

            return Created("",new { });
        }
        [HttpPut]
        public IActionResult Put(UpdateUserCommand updateUserCommand)
        {
            return Ok();
        }
    }
}
