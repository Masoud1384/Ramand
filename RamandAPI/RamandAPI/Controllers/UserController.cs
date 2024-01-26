using Application.TokenOperations.TokenCommands;
using Application.UserOperations.Commands;
using Application.UserOperations.IRepositoryApplication;
using Domain.IRepositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.UserSecrets;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace RamandAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly ITokenRepository _tokenRepository;
        private readonly IUserRepositoryApplication _userRepository;
        public UserController(IUserRepositoryApplication userRepository, ITokenRepository tokenRepository, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _tokenRepository = tokenRepository;
            _configuration = configuration;
        }

        [HttpGet]
        [Route("SelectAll")]
        public IActionResult GetAllUsers()
        {
            var users = _userRepository.GetAll();
            return Ok(users);
        }

        [HttpGet("{userId}")]
        public IActionResult GetUser(int userId)
        {
            var user = _userRepository.GetUserBy(userId);
            return Ok(user);
        }
        [HttpPost("SignUp")]
        public IActionResult SignUp([FromBody] CreateUserCommand createUserCommand)
        {
            var isExists = _userRepository.IsUsernameExist(createUserCommand.username);
            if (!isExists)
            {
                var uservm = _userRepository.Create(createUserCommand);
                var jwtToken = TokenGenerator(uservm);
                _tokenRepository.SaveToken(uservm.Id, new Domain.Models.Token(jwtToken.JwtToken, jwtToken.Expire, jwtToken.RefreshToken, jwtToken.RefreshTokenExp));
                var user = _userRepository.GetUserBy(uservm.Username);
                if (user != null&&user.Token != null)
                {
                    return Ok(new TokenCommand {Expire= jwtToken.Expire,Id= jwtToken.Id,RefreshTokenExp = jwtToken.RefreshTokenExp, JwtToken = jwtToken.JwtToken, RefreshToken = jwtToken.RefreshToken });
                }
            }
            return BadRequest("Username already exists");
        }
        [HttpPost]
        [Route("Login")]
        public IActionResult Login([FromBody] LoginCommand user)
        {
            var isPassOk = VerifyPassword(user);
            if (isPassOk)
            {
                var uservm = _userRepository.GetUserBy(user.username);
                var jwtToken = uservm.Token;
                if (uservm.Token.Expire > DateTime.Now)
                {
                    return Ok(new TokenCommand {Expire = jwtToken.Expire, Id = jwtToken.Id, RefreshTokenExp = jwtToken.RefreshTokenExp, JwtToken = jwtToken.JwtToken, RefreshToken = jwtToken.RefreshToken });

                }
            }
            return Unauthorized();
        }
        [HttpPost("RefreshToken")]
        public IActionResult RefreshToken(string refreshToken)
        {
            var userToken = _tokenRepository.GetRefreshToken(refreshToken);
            if (userToken == null)
            {
                return NotFound();
            }
            if (userToken.RefreshTokenExp < DateTime.Now)
            {
                return Unauthorized();
            }
            var newToken = _tokenRepository.CreateRefreshToken(refreshToken);
            return Ok(new TokenCommand { RefreshToken = newToken.RefreshToken, JwtToken = newToken.JwtToken });
        }
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

        private TokenCommand TokenGenerator(UserVM userVm)
        {
            var cliams = new List<Claim>()
                {
                    new Claim("Username",userVm.Username),
                    new Claim("Id",userVm.Id.ToString())
                };
            string key = "{267DC4ED-5334-4C50-A007-DC3A8396462A}";
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var credentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
            var expireDate = DateTime.Now.AddHours(int.Parse(_configuration.GetValue<string>("JWT:expire")));
            var token = new JwtSecurityToken(
                issuer: _configuration.GetValue<string>("JWT:issuer"),
                audience: _configuration.GetValue<string>("JWT:audience"),
                expires: expireDate,
                notBefore: DateTime.Now,
                claims: cliams,
                signingCredentials: credentials
                );
            var jwtToken = new JwtSecurityTokenHandler().WriteToken(token);
            var tokenvm = new TokenCommand
            {
                Expire = expireDate,
                JwtToken = jwtToken,
                Id = userVm.Id,
                RefreshToken = Guid.NewGuid().ToString(),
                RefreshTokenExp = expireDate.AddDays(5)
            };
            _tokenRepository.SaveToken(userVm.Id, tokenvm);
            return tokenvm;
        }
        private bool VerifyPassword(LoginCommand loginCommand)
        {
            var user = _userRepository.GetUserBy(loginCommand.username);
            if (user != null)
            {
                return user.Password == loginCommand.password;
            }
            return false;
        }

    }
}
