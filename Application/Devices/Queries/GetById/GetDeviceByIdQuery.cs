using Application.Abstractions.MediatR;
using Domain.Entities;

namespace Application.Devices.Queries.GetById;

public sealed record GetDeviceByIdQuery(int Id) : IQuery<Device>;
