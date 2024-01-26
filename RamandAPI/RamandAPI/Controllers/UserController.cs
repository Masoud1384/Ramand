using Application.UserOperations.Commands;
using Application.UserOperations.IRepositoryApplication;
using Domain.IRepositories;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace RamandAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepositoryApplication _userRepository;
        public UserController(IUserRepositoryApplication userRepository)
        {
            _userRepository = userRepository;  
        }

        [HttpGet]
        public IActionResult GetAllUsers()
        {
            var users = _userRepository.GetAll();
            return Ok(users);
        }

        [HttpGet("userId")]
        public IActionResult GetUser(int userId)
        {
            var user = _userRepository.GetUserBy(userId);
            return Ok(user);
        }

        // POST api/<UserController>
        [HttpPost]
        public IActionResult Post([FromBody] CreateUserCommand createUserCommand)
        {
            var result = _userRepository.Create(createUserCommand);
            if (result)
            {
                return StatusCode(201);
            }
            return BadRequest();
        }

        // PUT api/<UserController>/5
        [HttpPut]
        public IActionResult Put([FromBody] UpdateUserCommand updateUserCommand)
        {
            var result = _userRepository.Update(updateUserCommand);
            if (result)
            {
                return StatusCode(202);
            }
            return BadRequest();
        }

        // DELETE api/<UserController>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var result = _userRepository.Delete(id);
            if (result)
            {
                return Ok();
            }
            return BadRequest();
        }
    }
}
