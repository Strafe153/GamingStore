using Application.Abstractions.MediatR;
using Application.Abstractions.Repositories;
using Application.Abstractions.Services;
using Domain.Entities;
using Domain.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Application.Companies.Commands.Create;

public sealed class CreateCompanyCommandHandler : ICommandHandler<CreateCompanyCommand, Company>
{
    private readonly IRepository<Company> _companyRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPictureService _pictureService;
    private readonly ILogger<CreateCompanyCommandHandler> _logger;

    public CreateCompanyCommandHandler(
        IRepository<Company> companyRepository, 
        IUnitOfWork unitOfWork, 
        IPictureService pictureService,
        ILogger<CreateCompanyCommandHandler> logger)
    {
        _companyRepository = companyRepository;
        _unitOfWork = unitOfWork;
        _pictureService = pictureService;
        _logger = logger;
    }

    public async Task<Company> Handle(CreateCompanyCommand command, CancellationToken cancellationToken)
    {
        var picture = await _pictureService.UploadAsync(command.Picture, "company-pictures", command.Name);
        var company = new Company(command.Name, picture);

        try
        {
            _companyRepository.Create(company);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("Succesfully created a company");

            return company;
        }
        catch (DbUpdateException)
        {
            _logger.LogWarning("Failed to create a company. The name '{Name}' is already taken", command.Name);
            throw new ValueNotUniqueException($"Name '{command.Name}' is already taken");
        }
    }
}
