using Application.UserOperations.Commands;
using Application.UserOperations.IRepositoryApplication;
using Domain.IRepositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;
using RamandAPI.V2;


namespace Testing
{
    public class UserControllerTests
    {
        private readonly Mock<IUserRepositoryApplication> _mockUserApplication;
        private readonly Mock<ITokenRepository> _mockTokenRepository;
        private readonly Mock<IConfiguration> _mockConfiguration;
        private readonly UserController _controller;

        public UserControllerTests()
        {
            _mockUserApplication = new Mock<IUserRepositoryApplication>();
            _mockTokenRepository = new Mock<ITokenRepository>();
            _mockConfiguration = new Mock<IConfiguration>();
            _controller = new UserController(_mockUserApplication.Object, _mockTokenRepository.Object, _mockConfiguration.Object);
        }


        [Fact]
        public void GetUser_ReturnsOkIfUserExists()
        {
            // Arrange
            _mockUserApplication.Setup(app => app.GetUserBy(It.IsAny<int>())).Returns(new UserVM());

            // Act
            var result = _controller.GetUser(1);

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }


        [Fact]
        public void Login_ReturnsOkIfCredentialsAreValid()
        {
            // Arrange
            _mockUserApplication.Setup(app => app.GetUserBy(It.IsAny<string>())).Returns(new UserVM { Token = new Domain.Models.Token("validToken", DateTime.Now.AddHours(1), "validRefreshToken", DateTime.Now.AddHours(2)) });

            // Act
            var result = _controller.Login(new LoginCommand());

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

    }

}
