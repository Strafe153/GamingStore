using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using GamingDevicesStore.Models;
using GamingDevicesStore.Dtos.User;
using GamingDevicesStore.Controllers;
using GamingDevicesStore.Repositories.Interfaces;
using Moq;
using Xunit;
using AutoMapper;

namespace GamingDevicesStore.Tests
{
    public class UsersControllerTests
    {
        private static readonly Mock<IUserControllable> _repo = new();
        private static readonly Mock<IMapper> _mapper = new();
        private static readonly UsersController _controller = new(_repo.Object, _mapper.Object);

        [Fact]
        public async Task GetAllUsersAsync_ValidData_ReturnsOkObjectResult()
        {
            // Act
            var result = await _controller.GetAllUsersAsync();

            // Assert
            Assert.IsType<ActionResult<IEnumerable<UserReadDto>>>(result);
            Assert.IsType<OkObjectResult>(result.Result);
        }

        [Fact]
        public async Task GetUserAsync_ExistingUser_ReturnsOkObjectResult()
        {
            // Arrange
            _repo.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(new User());

            // Act
            var result = await _controller.GetUserAsync(Guid.Empty);

            // Assert
            Assert.IsType<ActionResult<UserReadDto>>(result);
            Assert.IsType<OkObjectResult>(result.Result);
        }

        [Fact]
        public async Task GetUserAsync_NonexistingUser_ReturnsNotFoundObjectResult()
        {
            // Arrange
            _repo.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((User?)null);

            // Act
            var result = await _controller.GetUserAsync(Guid.Empty);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result.Result);
        }

        [Fact]
        public async Task RegisterUserAsync_ValidData_ReturnsCreatedAtActionResult()
        {
            // Arrange
            _mapper.Setup(m => m.Map<User>(It.IsAny<UserRegisterDto>())).Returns(new User());
            _mapper.Setup(m => m.Map<UserReadDto>(It.IsAny<User>())).Returns(new UserReadDto());

            // Act
            var result = await _controller.RegisterUserAsync(new UserRegisterDto());

            // Assert
            Assert.IsType<ActionResult<UserReadDto>>(result);
            Assert.IsType<CreatedAtActionResult>(result.Result);
        }

        [Fact]
        public async Task LoginuserAsync_ExistingUser_ReturnsOkObjectResult()
        {
            // Arrange
            _repo.Setup(r => r.GetByNameAsync(It.IsAny<string>())).ReturnsAsync(new User());
            _repo.Setup(r => r.VerifyPasswordHash(It.IsAny<string>(), 
                It.IsAny<byte[]>(), It.IsAny<byte[]>())).Returns(true);
            _mapper.Setup(m => m.Map<UserWithTokenReadDto>(It.IsAny<User>())).Returns(new UserWithTokenReadDto());

            // Act
            var result = await _controller.LoginUserAsync(new UserLoginDto());

            // Assert
            Assert.IsType<ActionResult<UserWithTokenReadDto>>(result);
            Assert.IsType<OkObjectResult>(result.Result);
        }

        [Fact]
        public async Task LoginuserAsync_NonexistingUser_ReturnsNotFoundObjectResult()
        {
            // Arrange
            _repo.Setup(r => r.GetByNameAsync(It.IsAny<string>())).ReturnsAsync((User?)null);

            // Act
            var result = await _controller.LoginUserAsync(new UserLoginDto());

            // Assert
            Assert.IsType<NotFoundObjectResult>(result.Result);
        }

        [Fact]
        public async Task LoginuserAsync_IncorrectPassword_ReturnsBadRequestObjectResult()
        {
            // Arrange
            _repo.Setup(r => r.GetByNameAsync(It.IsAny<string>())).ReturnsAsync(new User());

            // Act
            var result = await _controller.LoginUserAsync(new UserLoginDto());

            // Assert
            Assert.IsType<BadRequestObjectResult>(result.Result);
        }

        [Fact]
        public async Task UpdateUserAsync_ExistingUser_ReturnsNoContentResult()
        {
            // Arrange
            _repo.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(
                new User() 
                { 
                    Username = "identity_name" 
                });
            MockUserIdentityName(_controller);

            // Act
            var result = await _controller.UpdateUserAsync(Guid.Empty, new UserUpdateDto());

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task UpdateUserAsync_NonexistingUser_ReturnsNotFoundObjectResult()
        {
            // Arrange
            _repo.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((User?)null);

            // Act
            var result = await _controller.UpdateUserAsync(Guid.Empty, new UserUpdateDto());

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }


        [Fact]
        public async Task UpdateUserAsync_IsNotAdminNorOwner_ReturnsForbidResult()
        {
            // Arrange
            _repo.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(
                new User() 
                { 
                    Username = "test_name"
                });
            MockUserIdentityName(_controller);

            // Act
            var result = await _controller.UpdateUserAsync(Guid.Empty, new UserUpdateDto());

            // Assert
            Assert.IsType<ForbidResult>(result);
        }

        [Fact]
        public async Task DeleteUserAsync_ExistingUser_ReturnsNoContentResult()
        {
            // Arrange
            _repo.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(
                new User() 
                { 
                    Username = "identity_name"
                });
            MockUserIdentityName(_controller);

            // Act
            var result = await _controller.DeleteUserAsync(Guid.Empty);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeleteUserAsync_NonexistingUser_ReturnsNotFoundObjectResult()
        {

            // Arrange
            _repo.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((User?)null);

            // Act
            var result = await _controller.DeleteUserAsync(Guid.Empty);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async Task DeleteUserAsync_IsNotAdminNorOwner_ReturnsForbidResult()
        {
            // Arrange
            _repo.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(
                new User()
                {
                    Username = "test_name"
                });
            MockUserIdentityName(_controller);

            // Act
            var result = await _controller.DeleteUserAsync(Guid.Empty);

            // Assert
            Assert.IsType<ForbidResult>(result);
        }

        private static void MockUserIdentityName(ControllerBase controller)
        {
            ClaimsPrincipal user = new(
                new ClaimsIdentity(
                    new Claim[] 
                    { 
                        new Claim(ClaimTypes.Name, "identity_name") 
                    },
                    "mock"
                )
            );

            controller.ControllerContext.HttpContext = new DefaultHttpContext() 
            { 
                User = user 
            };
        }
    }
}
