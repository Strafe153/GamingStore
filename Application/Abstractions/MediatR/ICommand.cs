using MediatR;

namespace Application.Abstractions.MediatR;

public interface ICommand : IRequest
{
}

public interface ICommand<TResponse> : IRequest<TResponse>
{

}