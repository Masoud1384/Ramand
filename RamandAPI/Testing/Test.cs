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
        private readonly Mock<IMessagesRepository> _mockMessages;
        private readonly UserController _controller;

        public UserControllerTests()
        {
            _mockUserApplication = new Mock<IUserRepositoryApplication>();
            _mockTokenRepository = new Mock<ITokenRepository>();
            _mockConfiguration = new Mock<IConfiguration>();
            _mockMessages = new Mock<IMessagesRepository>();
            _controller = new UserController(_mockMessages.Object,_mockUserApplication.Object, _mockTokenRepository.Object, _mockConfiguration.Object);
        }


        [Fact]
        public void GetUser_ReturnsOkIfUserExists()
        {
            // Arrange
            var mockUserRepository = new Mock<IUserRepository>();
            var user = new UserVM();
            mockUserRepository.Setup(repo => repo.GetUserBy(It.IsAny<int>())).Returns(user);

            // Act
            var result = _controller.GetUser(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<UserVM>(okResult.Value);
            Assert.Equal(user, returnValue);
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
