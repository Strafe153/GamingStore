using Domain.Entities;

namespace Application.Abstractions.Services;

public interface IElasticSearchService
{
    IReadOnlyCollection<Log> Get(string query);
    //IReadOnlyCollection<object> Get(string query);
}
