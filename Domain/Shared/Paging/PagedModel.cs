﻿namespace Domain.Shared.Paging;

public class PagedModel<T>
{
	public int CurrentPage { get; init; }
	public int TotalPages { get; init; }
	public int PageSize { get; init; }
	public int TotalItems { get; init; }
	public bool HasPrevious { get; init; }
	public bool HasNext { get; init; }

	public IEnumerable<T> Entities { get; init; } = new List<T>();
}
