using Application.Tests.Fixtures;
using Core.Entities;
using Core.Exceptions;
using Core.Models;
using FluentAssertions;
using Moq;
using System.Linq.Expressions;
using Xunit;

namespace Application.Tests;

public class UserServiceTests : IClassFixture<UserServiceFixture>
{
    private readonly UserServiceFixture _fixture;

    public UserServiceTests(UserServiceFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task GetAllAsync_ValidParametersDataFromRepository_ReturnsPaginatedListOfUser()
    {
        // Arrange
        _fixture.MockCacheService
            .Setup(s => s.GetAsync<List<User>>(It.IsAny<string>()))
            .ReturnsAsync((List<User>)null!);

        _fixture.MockUserRepository
            .Setup(r => r.GetAllAsync(
                It.IsAny<int>(), 
                It.IsAny<int>(), 
                It.IsAny<CancellationToken>(),
                It.IsAny<Expression<Func<User, bool>>>()))
            .ReturnsAsync(_fixture.PaginatedList);

        // Act
        var result = await _fixture.MockUserService.GetAllAsync(_fixture.Id, _fixture.Id);

        // Assert
        result.Should().NotBeNull().And.NotBeEmpty().And.BeOfType<PaginatedList<User>>();
    }

    [Fact]
    public async Task GetAllAsync_ValidParametersDataFromCache_ReturnsPaginatedListOfUser()
    {
        // Arrange
        _fixture.MockCacheService
            .Setup(s => s.GetAsync<List<User>>(It.IsAny<string>()))
            .ReturnsAsync(_fixture.PaginatedList);

        // Act
        var result = await _fixture.MockUserService.GetAllAsync(_fixture.Id, _fixture.Id);

        // Assert
        result.Should().NotBeNull().And.NotBeEmpty().And.BeOfType<PaginatedList<User>>();
    }

    [Fact]
    public async Task GetByIdAsync_ExistingUserInRepository_ReturnsUser()
    {
        // Arrange
        _fixture.MockCacheService
            .Setup(s => s.GetAsync<User>(It.IsAny<string>()))
            .ReturnsAsync((User)null!);

        _fixture.MockUserRepository
            .Setup(r => r.GetByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(_fixture.User);

        // Act
        var result = await _fixture.MockUserService.GetByIdAsync(_fixture.Id);

        // Assert
        result.Should().NotBeNull().And.BeOfType<User>();
    }

    [Fact]
    public async Task GetByIdAsync_ExistingUserInCache_ReturnsUser()
    {
        // Arrange
        _fixture.MockCacheService
            .Setup(s => s.GetAsync<User>(It.IsAny<string>()))
            .ReturnsAsync(_fixture.User);

        // Act
        var result = await _fixture.MockUserService.GetByIdAsync(_fixture.Id);

        // Assert
        result.Should().NotBeNull().And.BeOfType<User>();
    }

    [Fact]
    public async Task GetByIdAsync_NonexistingUser_ThrowsNullReferenceException()
    {
        // Arrange
        _fixture.MockUserRepository
            .Setup(r => r.GetByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((User)null!);

        _fixture.MockCacheService
            .Setup(s => s.GetAsync<User>(It.IsAny<string>()))
            .ReturnsAsync((User)null!);

        // Act
        var result = async () => await _fixture.MockUserService.GetByIdAsync(_fixture.Id);

        // Assert
        result.Should().NotBeNull();
        await result.Should().ThrowAsync<NullReferenceException>();
    }

    [Fact]
    public async Task GetByEmailAsync_ExistingUserInRepository_ReturnsUser()
    {
        // Arrange
        _fixture.MockCacheService
            .Setup(s => s.GetAsync<User>(It.IsAny<string>()))
            .ReturnsAsync((User)null!);

        _fixture.MockUserRepository
            .Setup(r => r.GetByEmailAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(_fixture.User);

        // Act
        var result = await _fixture.MockUserService.GetByEmailAsync(_fixture.Name);

        // Assert
        result.Should().NotBeNull().And.BeOfType<User>();
    }

    [Fact]
    public async Task GetByEmailAsync_ExistingUserInCache_ReturnsUser()
    {
        // Arrange
        _fixture.MockCacheService
            .Setup(s => s.GetAsync<User>(It.IsAny<string>()))
            .ReturnsAsync(_fixture.User);

        // Act
        var result = await _fixture.MockUserService.GetByEmailAsync(_fixture.Name);

        // Assert
        result.Should().NotBeNull().And.BeOfType<User>();
    }

    [Fact]
    public async Task GetByEmailAsync_NonexistingUser_ThrowsNullReferenceException()
    {
        // Arrange
        _fixture.MockCacheService
            .Setup(s => s.GetAsync<User>(It.IsAny<string>()))
            .ReturnsAsync((User)null!);

        _fixture.MockUserRepository
            .Setup(r => r.GetByEmailAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((User)null!);

        // Act
        var result = async() => await _fixture.MockUserService.GetByEmailAsync(_fixture.Name);

        // Assert
        result.Should().NotBeNull();
        await result.Should().ThrowAsync<NullReferenceException>();
    }

    [Fact]
    public void CreateAsync_IdentityResultSucceeded_ReturnsTask()
    {
        // Arrange
        _fixture.MockUserRepository
            .Setup(r => r.CreateAsync(It.IsAny<User>(), It.IsAny<string>()))
            .ReturnsAsync(_fixture.SucceededResult);

        // Act
        var result = async () => await _fixture.MockUserService.CreateAsync(_fixture.User, _fixture.Name);

        // Assert
        result.Should().NotBeNull();
    }

    [Fact]
    public async Task CreateAsync_IdentityResultFailed_ThrowsOperationFailedException()
    {
        // Arrange
        _fixture.MockUserRepository
            .Setup(r => r.CreateAsync(It.IsAny<User>(), It.IsAny<string>()))
            .ReturnsAsync(_fixture.FailedResult);

        // Act
        var result = async () => await _fixture.MockUserService.CreateAsync(_fixture.User, _fixture.Name);

        // Assert
        result.Should().NotBeNull();
        await result.Should().ThrowAsync<NameNotUniqueException>();
    }

    [Fact]
    public void UpdateAsync_IdentityResultSucceeded_ReturnsTask()
    {
        // Arrange
        _fixture.MockUserRepository
            .Setup(r => r.UpdateAsync(It.IsAny<User>()))
            .ReturnsAsync(_fixture.SucceededResult);

        // Act
        var result = async () => await _fixture.MockUserService.UpdateAsync(_fixture.User);

        // Assert
        result.Should().NotBeNull();
    }

    [Fact]
    public async Task UpdateAsync_IdentityResultFailed_ThrowsOperationFailedException()
    {
        // Arrange
        _fixture.MockUserRepository
            .Setup(r => r.UpdateAsync(It.IsAny<User>()))
            .ReturnsAsync(_fixture.FailedResult);

        // Act
        var result = async () => await _fixture.MockUserService.UpdateAsync(_fixture.User);

        // Assert
        result.Should().NotBeNull();
        await result.Should().ThrowAsync<NameNotUniqueException>();
    }

    [Fact]
    public void DeleteAsync_IdentityResultSucceeded_ReturnsTask()
    {
        // Arrange
        _fixture.MockUserRepository
            .Setup(r => r.DeleteAsync(It.IsAny<User>()))
            .ReturnsAsync(_fixture.SucceededResult);

        // Act
        var result = async () => await _fixture.MockUserService.DeleteAsync(_fixture.User);

        // Assert
        result.Should().NotBeNull();
    }

    [Fact]
    public async Task DeleteAsync_IdentityResultFailed_ThrowsOperationFailedException()
    {
        // Arrange
        _fixture.MockUserRepository
            .Setup(r => r.DeleteAsync(It.IsAny<User>()))
            .ReturnsAsync(_fixture.FailedResult);

        // Act
        var result = async () => await _fixture.MockUserService.DeleteAsync(_fixture.User);

        // Assert
        result.Should().NotBeNull();
        await result.Should().ThrowAsync<OperationFailedException>();
    }

    [Fact]
    public void AssignRoleAsync_IdentityResultSucceeded_ReturnsTask()
    {
        // Arrange
        _fixture.MockUserRepository
            .Setup(r => r.RemoveFromRolesAsync(It.IsAny<User>()))
            .ReturnsAsync(_fixture.SucceededResult);

        _fixture.MockUserRepository
            .Setup(r => r.AssignRoleAsync(It.IsAny<User>(), It.IsAny<string>()))
            .ReturnsAsync(_fixture.SucceededResult);

        // Act
        var result = async () => await _fixture.MockUserService.AssignRoleAsync(_fixture.User, _fixture.Name);

        // Assert
        result.Should().NotBeNull();
    }

    [Fact]
    public async Task AssignRoleAsync_RemoveFromRolesAsyncIdentityResultFailed_ThrowsOperationFailedException()
    {
        // Arrange
        _fixture.MockUserRepository
            .Setup(r => r.RemoveFromRolesAsync(It.IsAny<User>()))
            .ReturnsAsync(_fixture.SucceededResult);

        _fixture.MockUserRepository
            .Setup(r => r.AssignRoleAsync(It.IsAny<User>(), It.IsAny<string>()))
            .ReturnsAsync(_fixture.FailedResult);

        // Act
        var result = async () => await _fixture.MockUserService.AssignRoleAsync(_fixture.User, _fixture.Name);

        // Assert
        result.Should().NotBeNull();
        await result.Should().ThrowAsync<OperationFailedException>();
    }

    [Fact]
    public async Task AssignRoleAsync_AssignRoleAsyncIdentityResultFailed_ThrowsOperationFailedException()
    {
        // Arrange
        _fixture.MockUserRepository
            .Setup(r => r.RemoveFromRolesAsync(It.IsAny<User>()))
            .ReturnsAsync(_fixture.FailedResult);

        // Act
        var result = async () => await _fixture.MockUserService.AssignRoleAsync(_fixture.User, _fixture.Name);

        // Assert
        result.Should().NotBeNull();
        await result.Should().ThrowAsync<OperationFailedException>();
    }

    [Fact]
    public void ChangePasswordAsync_IdentityResultSucceeded_ReturnsTask()
    {
        // Arrange
        _fixture.MockUserRepository
            .Setup(r => r.ChangePasswordAsync(
                It.IsAny<User>(), 
                It.IsAny<string>(),
                It.IsAny<string>()))
            .ReturnsAsync(_fixture.SucceededResult);

        // Act
        var result = async () => await _fixture.MockUserService.ChangePasswordAsync(_fixture.User, _fixture.Name, _fixture.Name);

        // Assert
        result.Should().NotBeNull();
    }

    [Fact]
    public async Task ChangePasswordAsync_IdentityResultFailed_ThrowsOperationFailedException()
    {
        // Arrange
        _fixture.MockUserRepository
            .Setup(r => r.ChangePasswordAsync(
                It.IsAny<User>(),
                It.IsAny<string>(),
                It.IsAny<string>()))
            .ReturnsAsync(_fixture.FailedResult);

        // Act
        var result = async () => await _fixture.MockUserService.ChangePasswordAsync(_fixture.User, _fixture.Name, _fixture.Name);

        // Assert
        result.Should().NotBeNull();
        await result.Should().ThrowAsync<OperationFailedException>();
    }

    [Fact]
    public void VerifyUserAccessRights_SufficientClaims_ReturnsVoid()
    {
        // Arrange
        _fixture.MockHttpContextAccessor
            .Setup(a => a.HttpContext)
            .Returns(_fixture.HttpContextWithSufficientClaims);

        // Act
        var result = () => _fixture.MockUserService.VerifyUserAccessRights(_fixture.User);

        // Assert
        result.Should().NotBeNull();
    }

    [Fact]
    public void VerifyUserAccessRights_InsufficientClaims_ThrowsNotEnoughRightsException()
    {
        // Arrange
        _fixture.MockHttpContextAccessor
            .Setup(a => a.HttpContext)
            .Returns(_fixture.HttpContextWithInsufficientClaims);

        // Act
        var result = () => _fixture.MockUserService.VerifyUserAccessRights(_fixture.User);

        // Assert
        result.Should().Throw<NotEnoughRightsException>();
    }
}
