using Application.Abstractions.Repositories;
using Application.Abstractions.Services;
using Application.Devices.Queries.GetById;
using Application.Users.Queries.GetById;
using AutoFixture;
using AutoFixture.AutoMoq;
using Domain.Entities;
using Microsoft.Extensions.Logging;
using Moq;

namespace Application.Tests.Users.Queries.Fixtures;

public class GetUserByIdQueryHandlerFixture
{
    public GetUserByIdQueryHandlerFixture()
    {
        var fixture = new Fixture().Customize(new AutoMoqCustomization());

        MockRepository = fixture.Freeze<Mock<IUserRepository>>();
        MockCacheService = fixture.Freeze<Mock<ICacheService>>();
        MockLogger = fixture.Freeze<Mock<ILogger<GetUserByIdQueryHandler>>>();

        GetUserByIdQueryHandler = new GetUserByIdQueryHandler(
            MockRepository.Object,
            MockCacheService.Object,
            MockLogger.Object);

        Name = "Name";
        User = GetUser();
        GetUserByIdQuery = GetGetUserByIdQuery();
    }

    public GetUserByIdQueryHandler GetUserByIdQueryHandler { get; }
    public Mock<IUserRepository> MockRepository { get; }
    public Mock<ICacheService> MockCacheService { get; }
    public Mock<ILogger<GetUserByIdQueryHandler>> MockLogger { get; }

    public int Id { get; }
    public string Name { get; }
    public CancellationToken CancellationToken { get; }
    public User User { get; }
    public GetUserByIdQuery GetUserByIdQuery { get; }

    private GetUserByIdQuery GetGetUserByIdQuery()
    {
        return new GetUserByIdQuery(Id);
    }

    private User GetUser()
    {
        return new User(Name, Name, Name, Name, Name, Name);
    }
}
