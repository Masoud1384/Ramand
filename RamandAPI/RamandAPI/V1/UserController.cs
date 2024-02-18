using Application.TokenOperations.TokenCommands;
using Application.UserOperations.Commands;
using Application.UserOperations.IRepositoryApplication;
using Domain.IRepositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace RamandAPI.V1
{
    [ApiVersion("1.0")]
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
        public IActionResult GetAllUsers([FromHeader] string jwtToken)
        {
            if (!_tokenRepository.ValidateToken(jwtToken))
            {
                return Unauthorized(new { message = "Invalid or expired token." });
            }
            var users = _userRepository.GetAll();
            if (users != null)
            {
                users.Select(u => u.links = new List<HATEOSDto>
                {
                    new HATEOSDto
                    {
                        hrref = Url.Action(nameof(GetUser),"User",new {u.Id },Request.Scheme),
                        Method = "GET"
                    },
                    new HATEOSDto
                    {
                        hrref = Url.Action(nameof(V1.UserController.Delete),"User",new {username = u.Username},Request.Scheme),
                        Method = "DELETE"
                    }
                });
                return Ok(new { message = "Users retrieved successfully.", users });
            }
            return NotFound(new { message = "No users found." });
        }

        [HttpGet("{userId}")]
        public IActionResult GetUser(int userId)
        {
            try
            {
                var user = _userRepository.GetUserBy(userId);
                if (user != null)
                {
                    var hateos = new List<HATEOSDto>
                {
                    new HATEOSDto
                    {
                        hrref = Url.Action(nameof(Delete),"User",new {username = user.Username},Request.Scheme),
                        Method = "DELETE"
                    }
                };
                    user.links = hateos;
                    return Ok(new { message = "User found.", user });
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
            }
            return NotFound(new { message = "User not found." });
        }

        [HttpPost("SignUp")]
        public IActionResult SignUp([FromBody] CreateUserCommand createUserCommand)
        {
            var isExists = _userRepository.IsUsernameExist(createUserCommand.Username);
            if (!isExists)
            {
                var uservm = _userRepository.Create(createUserCommand);
                var jwtToken = TokenGenerator(uservm);
                var user = _userRepository.GetUserBy(uservm.Username);
                if (user != null && user.Token != null)
                {
                    return StatusCode(201, new { message = "User successfully created.", token = new TokenCommand { Expire = jwtToken.Expire, Id = jwtToken.Id, RefreshTokenExp = jwtToken.RefreshTokenExp, JwtToken = jwtToken.JwtToken, RefreshToken = jwtToken.RefreshToken } });
                }
            }
            return Conflict(new { message = "Username already exists." });
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
                    return Ok(new { message = "Login successful.", token = new TokenCommand { Expire = jwtToken.Expire, Id = jwtToken.Id, RefreshTokenExp = jwtToken.RefreshTokenExp, JwtToken = jwtToken.JwtToken, RefreshToken = jwtToken.RefreshToken } });
                }
                else
                {
                    return Unauthorized(new { message = "Token expired." });
                }
            }
            return Unauthorized(new { message = "Invalid credentials." });
        }

        [HttpPost("RefreshToken")]
        public IActionResult RefreshToken(string refreshToken)
        {
            var userToken = _tokenRepository.GetRefreshToken(refreshToken);
            if (userToken == null)
            {
                return NotFound(new { message = "Refresh token not found." });
            }
            if (userToken.RefreshTokenExp < DateTime.Now)
            {
                return Unauthorized(new { message = "Refresh token expired." });
            }
            var newToken = _tokenRepository.CreateRefreshToken(refreshToken);
            return StatusCode(202, new { message = "Refresh token successfully updated.", token = new TokenCommand { RefreshToken = newToken.RefreshToken, RefreshTokenExp = newToken.RefreshTokenExp } });
        }

        [HttpDelete]
        public IActionResult Delete(string username)
        {
            var result = _userRepository.Delete(username);
            if (result)
            {
                return Ok(new { message = "User successfully deleted." });
            }
            return NotFound(new { message = "User not found." });
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
