using Application.Abstractions.Services;
using Domain.Entities;
using Nest;

namespace Application.Services;

public class ElasticSearchService : IElasticSearchService
{
    private readonly ElasticClient _elasticClient;

    public ElasticSearchService(ElasticClient elasticClient)
    {
        _elasticClient = elasticClient;
    }

    //public ISearchResponse<LogEntry> Get(string query) =>
    //    _elasticClient.Search<LogEntry>(s => s
    //        .Query(q => q
    //            .Match(m => m
    //                .Field(f => f.Message)
    //                .Query(query))));

    //public IReadOnlyCollection<Log> Get(string query) =>
    //    _elasticClient
    //        .Search<Log>(sd => sd
    //            .Source(sfd => sfd
    //                .Includes(fd => fd
    //                    .Fields(
    //                        l => l.Timestamp,
    //                        l => l.Level,
    //                        l => l.Message)))
    //            .Query(qcd => qcd
    //                .Match(mqd => mqd
    //                    .Field(l => l.Message)
    //                    .Query(query))))
    //    .Documents;
    public IReadOnlyCollection<Log> Get(string query) =>
        _elasticClient
            .Search<Log>(sd => sd.map))
        .Documents;

    //public IReadOnlyCollection<object> Get(string query) =>
    //    _elasticClient
    //        .Search<object>()
    //    .Documents;
}
