using Application.Abstractions.MediatR;
using Application.Abstractions.Repositories;
using Application.Abstractions.Services;
using Domain.Entities;
using Domain.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Application.Companies.Commands.Update;

public sealed class UpdateCompanyCommandHandler : ICommandHandler<UpdateCompanyCommand>
{
    private readonly IRepository<Company> _companyRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPictureService _pictureService;
    private readonly ILogger<UpdateCompanyCommandHandler> _logger;

    public UpdateCompanyCommandHandler(
        IRepository<Company> companyRepository, 
        IUnitOfWork unitOfWork, 
        IPictureService pictureService,
        ILogger<UpdateCompanyCommandHandler> logger)
    {
        _companyRepository = companyRepository;
        _unitOfWork = unitOfWork;
        _pictureService = pictureService;
        _logger = logger;
    }

    public async Task<Unit> Handle(UpdateCompanyCommand command, CancellationToken cancellationToken)
    {
        try
        {
            await _pictureService.DeleteAsync(command.Company.Picture);

            command.Company.Name = command.Name;
            command.Company.Picture = await _pictureService.UploadAsync(command.Picture, "company-pictures", command.Name);

            _companyRepository.Update(command.Company);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("Successfully updated a company with id {Id}", command.Company.Id);

            return Unit.Value;
        }
        catch (DbUpdateException)
        {
            _logger.LogWarning("Failed to update the company wiht id {Id}. The name '{Name}' is already taken",
                command.Company.Id, command.Company.Name);
            throw new ValueNotUniqueException($"Name '{command.Company.Name}' is already taken");
        }
    }
}
