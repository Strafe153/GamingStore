namespace Domain.Shared.PageParameters;

public class PageParameters
{
    private readonly int _maxPageSize = 20;
    private int _pageSize = 5;

    public int PageSize
    {
        get => _pageSize;

        set
        {
            _pageSize = value > _maxPageSize ? _maxPageSize : value;
        }
    }

    public int PageNumber { get; set; } = 1;
}
