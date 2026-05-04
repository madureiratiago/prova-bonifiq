using Microsoft.EntityFrameworkCore;
using ProvaPub.Interfaces;
using ProvaPub.Models;
using ProvaPub.Repository;

public class CustomerService : PagedService<Customer>, ICustomerService
{
    private readonly TestDbContext _ctx;
    private readonly IDateTimeProvider _dateTimeProvider;

    public CustomerService(TestDbContext ctx, IDateTimeProvider dateTimeProvider)
        : base(ctx)
    {
        _ctx = ctx;
        _dateTimeProvider = dateTimeProvider;
    }

    public async Task<bool> CanPurchase(int customerId, decimal purchaseValue)
    {
        if (customerId <= 0)
            throw new ArgumentOutOfRangeException(nameof(customerId));

        if (purchaseValue <= 0)
            throw new ArgumentOutOfRangeException(nameof(purchaseValue));

        var customer = await _ctx.Customers
            .Include(c => c.Orders)
            .FirstOrDefaultAsync(c => c.Id == customerId);

        if (customer == null)
            throw new InvalidOperationException($"Customer Id {customerId} does not exists");

        var baseDate = _dateTimeProvider.UtcNow.AddMonths(-1);

        var ordersInThisMonth = customer.Orders
            .Count(o => o.OrderDate >= baseDate);

        if (ordersInThisMonth > 0)
            return false;

        if (!customer.Orders.Any() && purchaseValue > 100)
            return false;

        var now = _dateTimeProvider.UtcNow;

        if (now.Hour < 8 || now.Hour > 18 ||
            now.DayOfWeek == DayOfWeek.Saturday ||
            now.DayOfWeek == DayOfWeek.Sunday)
            return false;

        return true;
    }

    public PagedResult<Customer> ListCustomers(int page)
    {
        return GetPaged(_ctx.Customers.AsQueryable(), page);

    }
}