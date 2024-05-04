using Application.Abstractions.MediatR;
using Application.Abstractions.Repositories;
using Application.Abstractions.Services;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Companies.Commands.Delete;

public sealed class DeleteCompanyCommandHandler : ICommandHandler<DeleteCompanyCommand>
{
    private readonly IRepository<Company> _companyRepository;
    private readonly IDatabaseRepository _databaseRepository;
    private readonly IPictureService _pictureService;
    private readonly ILogger<DeleteCompanyCommandHandler> _logger;

    public DeleteCompanyCommandHandler(
        IRepository<Company> companyRepository,
        IDatabaseRepository databaseRepository, 
        IPictureService pictureService,
        ILogger<DeleteCompanyCommandHandler> logger)
    {
        _companyRepository = companyRepository;
        _databaseRepository = databaseRepository;
        _pictureService = pictureService;
        _logger = logger;
    }

    public async Task<Unit> Handle(DeleteCompanyCommand command, CancellationToken cancellationToken)
    {
        _companyRepository.Delete(command.Company);
        await _databaseRepository.SaveChangesAsync();
        await _pictureService.DeleteAsync(command.Company.Picture!);

        _logger.LogInformation("Succesfully deleted a company with id {Id}", command.Company.Id);

        return Unit.Value;
    }
}
