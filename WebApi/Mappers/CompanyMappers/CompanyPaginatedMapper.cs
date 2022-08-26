using Core.Entities;
using Core.Models;
using Core.ViewModels;
using Core.ViewModels.CompanyViewModels;
using WebApi.Mappers.Interfaces;

namespace WebApi.Mappers.CompanyMappers
{
    public class CompanyPaginatedMapper : IMapper<PaginatedList<Company>, PageViewModel<CompanyReadViewModel>>
    {
        private readonly IMapper<Company, CompanyReadViewModel> _readMapper;

        public CompanyPaginatedMapper(IMapper<Company, CompanyReadViewModel> readMapper)
        {
            _readMapper = readMapper;
        }

        public PageViewModel<CompanyReadViewModel> Map(PaginatedList<Company> source)
        {
            return new PageViewModel<CompanyReadViewModel>()
            {
                CurrentPage = source.CurrentPage,
                TotalPages = source.TotalPages,
                PageSize = source.PageSize,
                TotalItems = source.TotalItems,
                HasPrevious = source.HasPrevious,
                HasNext = source.HasNext,
                Entities = source.Select(c => _readMapper.Map(c))
            };
        }
    }
}
