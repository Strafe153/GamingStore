using Application.Abstractions.Repositories;
using Application.Abstractions.Services;
using Application.Users.Queries.GetById;
using AutoFixture;
using AutoFixture.AutoMoq;
using Bogus;
using Domain.Entities;
using Microsoft.Extensions.Logging;
using Moq;

namespace Application.Tests.Users.Queries.Fixtures;

public class GetUserByIdQueryHandlerFixture
{
    public GetUserByIdQueryHandlerFixture()
    {
        var fixture = new Fixture().Customize(new AutoMoqCustomization());

        var userFaker = new Faker<User>()
            .CustomInstantiator(f => new(
                f.Name.FirstName(),
                f.Name.LastName(),
                f.Internet.Email(),
                f.Internet.UserName(),
                f.Phone.PhoneNumber(),
                null));

        var getUserByIdQueryFaker = new Faker<GetUserByIdQuery>()
            .CustomInstantiator(f => new(f.Random.Int(1, 5000)));

        MockRepository = fixture.Freeze<Mock<IUserRepository>>();
        MockCacheService = fixture.Freeze<Mock<ICacheService>>();
        MockLogger = fixture.Freeze<Mock<ILogger<GetUserByIdQueryHandler>>>();

        GetUserByIdQueryHandler = new GetUserByIdQueryHandler(
            MockRepository.Object,
            MockCacheService.Object,
            MockLogger.Object);

        User = userFaker.Generate();
        GetUserByIdQuery = getUserByIdQueryFaker.Generate();
    }

    public GetUserByIdQueryHandler GetUserByIdQueryHandler { get; }
    public Mock<IUserRepository> MockRepository { get; }
    public Mock<ICacheService> MockCacheService { get; }
    public Mock<ILogger<GetUserByIdQueryHandler>> MockLogger { get; }

    public CancellationToken CancellationToken { get; }
    public User User { get; }
    public GetUserByIdQuery GetUserByIdQuery { get; }
}
