using MediatR;

namespace Application.Abstractions.MediatR;

public interface IQuery<TResponse> : IRequest<TResponse>
{
}
