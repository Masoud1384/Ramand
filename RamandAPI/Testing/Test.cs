using Application.UserOperations.Commands;
using Application.UserOperations.IRepositoryApplication;
using Microsoft.AspNetCore.Mvc;
using Moq;
using RamandAPI.V2;


namespace Testing
{
    public class Test
    {

        private readonly Mock<IUserRepositoryApplication> _mockUserApplication;
        private readonly UserController _controller;

        public Test()
        {
            _mockUserApplication = new Mock<IUserRepositoryApplication>();
            _controller = new UserController(_mockUserApplication.Object);
        }

        [Fact]
        public void Get_OkIfAllUsersExists()
        {
            // Arrange
            _mockUserApplication.Setup(app => app.GetAll()).Returns(new List<UserVM>());

            // Act
            var result = _controller.Get();

            // Assert
            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public void Post_ReturnCreatedIfUserAdded()
        {
            // Arrange
            int? userId = null;
            _mockUserApplication.Setup(app => app.Create(It.IsAny<CreateUserCommand>())).Returns(new UserVM());

            // Act
            var result = _controller.Post(new CreateUserCommand());

            // Assert
            Assert.IsType<CreatedResult>(result);
        }
        [Fact]
        public void Get_OkIfUserExists()
        {
            // Arrange
            _mockUserApplication.Setup(app => app.GetUserBy(It.IsAny<int>())).Returns(new UserVM());

            // Act
            var result = _controller.Get(1);

            // Assert
            Assert.IsType<OkResult>(result);
        }
        [Fact]
        public void Put_OkIfUserUpdated()
        {
            // Arrange
            _mockUserApplication.Setup(app => app.Update(It.IsAny<UpdateUserCommand>())).Returns(true);

            // Act
            var result = _controller.Put(new UpdateUserCommand());

            // Assert
            Assert.IsType<OkResult>(result);
        }
    }
}
