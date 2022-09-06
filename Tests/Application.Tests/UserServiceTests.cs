using Application.Tests.Fixtures;
using Core.Entities;
using Core.Exceptions;
using Core.Models;
using FluentAssertions;
using Moq;
using System.Linq.Expressions;
using System.Numerics;
using Xunit;

namespace Application.Tests
{
    public class UserServiceTests : IClassFixture<UserServiceFixture>
    {
        private readonly UserServiceFixture _fixture;

        public UserServiceTests(UserServiceFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task GetAllAsync_ValidParameters_ReturnsPaginatedListOfUser()
        {
            // Arrange
            _fixture.MockUserRepository
                .Setup(r => r.GetAllAsync(
                    It.IsAny<int>(), 
                    It.IsAny<int>(), 
                    It.IsAny<Expression<Func<User, bool>>>()))
                .ReturnsAsync(_fixture.PaginatedList);

            // Act
            var result = await _fixture.MockUserService.GetAllAsync(_fixture.Id, _fixture.Id);

            // Assert
            result.Should().NotBeNull();
            result.Should().NotBeEmpty();
            result.Should().BeOfType<PaginatedList<User>>();
        }

        [Fact]
        public async Task GetByIdAsync_ExistingUser_ReturnsUser()
        {
            // Arrange
            _fixture.MockUserRepository
                .Setup(r => r.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(_fixture.User);

            // Act
            var result = await _fixture.MockUserService.GetByIdAsync(_fixture.Id);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<User>();
        }

        [Fact]
        public async Task GetByIdAsync_NonexistingUser_ThrowsNullReferenceException()
        {
            // Arrange
            _fixture.MockUserRepository
                .Setup(r => r.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync((User)null!);

            // Act
            var result = async () => await _fixture.MockUserService.GetByIdAsync(_fixture.Id);

            // Assert
            result.Should().NotBeNull();
            await result.Should().ThrowAsync<NullReferenceException>();
        }

        [Fact]
        public async Task GetByNameAsync_ExistingUser_ReturnsUser()
        {
            // Arrange
            _fixture.MockUserRepository
                .Setup(r => r.GetByNameAsync(It.IsAny<string>()))
                .ReturnsAsync(_fixture.User);

            // Act
            var result = await _fixture.MockUserService.GetByNameAsync(_fixture.Name);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<User>();
        }

        [Fact]
        public async Task GetByNameAsync_NonexistingUser_ThrowsNullReferenceException()
        {
            // Arrange
            _fixture.MockUserRepository
                .Setup(r => r.GetByNameAsync(It.IsAny<string>()))
                .ReturnsAsync((User)null!);

            // Act
            var result = async() => await _fixture.MockUserService.GetByNameAsync(_fixture.Name);

            // Assert
            result.Should().NotBeNull();
            await result.Should().ThrowAsync<NullReferenceException>();
        }

        [Fact]
        public void CreateAsync_ValidUser_ReturnsTask()
        {
            // Act
            var result = _fixture.MockUserService.CreateAsync(_fixture.User);

            // Assert
            result.Should().NotBeNull();
        }

        [Fact]
        public void UpdateAsync_ValidUser_ReturnsTask()
        {
            // Act
            var result = _fixture.MockUserService.UpdateAsync(_fixture.User);

            // Assert
            result.Should().NotBeNull();
        }

        [Fact]
        public void DeleteAsync_ValidUser_ReturnsTask()
        {
            // Act
            var result = _fixture.MockUserService.DeleteAsync(_fixture.User);

            // Assert
            result.Should().NotBeNull();
        }

        [Fact]
        public void ConstructUser_ValidParameters_ReturnsUser()
        {
            // Act
            var result = _fixture.MockUserService.ConstructUser(_fixture.Name!, _fixture.Name, _fixture.Bytes, _fixture.Bytes);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<User>();
        }

        [Fact]
        public void ChangePasswordData_ValidParameters_ReturnsVoid()
        {
            // Act
            var result = () => _fixture.MockUserService
                .ChangePasswordData(_fixture.User, _fixture.Bytes, _fixture.Bytes);

            // Assert
            result.Should().NotBeNull();
        }

        [Fact]
        public void VerifyUserAccessRights_SufficientRights_ReturnsVoid()
        {
            // Act
            var result = () => _fixture.MockUserService
                .VerifyUserAccessRights(_fixture.User, _fixture.IIdentity, _fixture.SufficientClaims);

            // Assert
            result.Should().NotBeNull();
        }

        [Fact]
        public void VerifyUserAccessRights_InsufficientRights_ThrowsNotEnoughRightsException()
        {
            // Act
            var result = () => _fixture.MockUserService
                .VerifyUserAccessRights(_fixture.User, _fixture.IIdentity, _fixture.InsufficientClaims);

            // Assert
            result.Should().NotBeNull();
            result.Should().Throw<NotEnoughRightsException>();
        }
    }
}
