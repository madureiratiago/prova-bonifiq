using ProvaPub.Models;
using ProvaPub.Repository;

public class PagedService<T> where T : class
{
    protected readonly TestDbContext _ctx;

    public PagedService(TestDbContext ctx)
    {
        _ctx = ctx;
    }

    protected PagedResult<T> GetPaged(
        IQueryable<T> query, int page, int pageSize = 10)
    {
        if (page <= 0) page = 1;

        var totalCount = query.Count();

        var items = query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        return new PagedResult<T>
        {
            Items = items,
            TotalCount = totalCount,
            HasNext = page * pageSize < totalCount
        };
    }
}