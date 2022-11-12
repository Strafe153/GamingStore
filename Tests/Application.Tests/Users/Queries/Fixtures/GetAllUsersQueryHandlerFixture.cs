using Application.Abstractions.Repositories;
using Application.Abstractions.Services;
using Application.Users.Queries.GetAll;
using AutoFixture;
using AutoFixture.AutoMoq;
using Domain.Entities;
using Domain.Shared;
using Microsoft.Extensions.Logging;
using Moq;

namespace Application.Tests.Users.Queries.Fixtures;

public class GetAllUsersQueryHandlerFixture
{
    public GetAllUsersQueryHandlerFixture()
    {
        var fixture = new Fixture().Customize(new AutoMoqCustomization());

        MockRepository = fixture.Freeze<Mock<IUserRepository>>();
        MockCacheService = fixture.Freeze<Mock<ICacheService>>();
        MockLogger = fixture.Freeze<Mock<ILogger<GetAllUsersQueryHandler>>>();

        GetAllUsersQueryHandler = new GetAllUsersQueryHandler(
            MockRepository.Object,
            MockCacheService.Object,
            MockLogger.Object);

        Name = "Name";
        GetAllUsersQuery = GetGetAllUsersQuery();
        PaginatedList = GetPaginatedList();
    }

    public GetAllUsersQueryHandler GetAllUsersQueryHandler { get; }
    public Mock<IUserRepository> MockRepository { get; }
    public Mock<ICacheService> MockCacheService { get; }
    public Mock<ILogger<GetAllUsersQueryHandler>> MockLogger { get; }

    public string Name { get; }
    public CancellationToken CancellationToken { get; }
    public GetAllUsersQuery GetAllUsersQuery { get; }
    public PaginatedList<User> PaginatedList { get; }

    private GetAllUsersQuery GetGetAllUsersQuery()
    {
        return new GetAllUsersQuery(1, 5);
    }

    private User GetUser()
    {
        return new User(Name, Name, Name, Name, Name, Name);
    }

    private List<User> GetUsers()
    {
        return new List<User>()
        {
            GetUser(),
            GetUser()
        };
    }

    private PaginatedList<User> GetPaginatedList()
    {
        return new PaginatedList<User>(GetUsers(), 6, 1, 5);
    }
}
