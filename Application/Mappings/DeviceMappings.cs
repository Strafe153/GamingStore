using Application.Devices.Commands.Create;
using Application.Devices.Commands.Update;
using Application.Devices.Queries;
using Domain.Entities;
using Domain.Shared.Paging;

namespace Application.Mappings;

public static class DeviceMappings
{
    public static GetDeviceResponse ToResponse(this Device device) => new(
        device.Id,
        device.Name,
        device.Category,
        device.Price,
        device.InStock,
        device.CompanyId,
        device.Picture);

    public static PagedModel<GetDeviceResponse> ToPagedModel(this PagedList<Device> list) =>
        new()
        {
            Entities = list.Select(ToResponse)
        };

    public static CreateDeviceCommand ToCommand(this CreateDeviceRequest request) => new(
        request.Name,
        request.Category,
        request.Price,
        request.InStock,
        request.CompanyId,
        request.Picture);

    public static UpdateDeviceCommand ToCommand(this UpdateDeviceRequest request) => new(
        default!,
        request.Name,
        request.Category,
        request.Price,
        request.InStock,
        request.CompanyId,
        request.Picture);
}