using Application.Abstractions.MediatR;
using Domain.Entities;
using Domain.Shared.Paging;

namespace Application.Devices.Queries.GetAll;

public sealed record GetAllDevicesQuery(
	int PageNumber,
	int PageSize,
	string? CompanyName) : IQuery<PagedList<Device>>;