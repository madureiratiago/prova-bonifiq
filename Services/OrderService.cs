using ProvaPub.Interfaces;
using ProvaPub.Models;
using ProvaPub.Repository;

public class OrderService
{
    private readonly TestDbContext _ctx;
    private readonly IEnumerable<IPaymentStrategy> _paymentStrategies;

    public OrderService(TestDbContext ctx, IEnumerable<IPaymentStrategy> paymentStrategies)
    {
        _ctx = ctx;
        _paymentStrategies = paymentStrategies;
    }

    public async Task<Order> PayOrder(string paymentMethod, decimal paymentValue, int customerId)
    {
        var strategy = _paymentStrategies
            .FirstOrDefault(p => p.PaymentMethod == paymentMethod.ToLower());

        //Não lançar exceções. Uma boa forma de não pesar o código. Não quiz me alongar aqui mas, sigo: 
        //https://www.wellingtonjhn.com/posts/n%C3%A3o-lance-exceptions-em-seu-dom%C3%ADnio-use-notifications/
        //Uso também o patner Either : https://stackoverflow.com/questions/63231450/how-to-use-the-either-type-in-c
        if (strategy == null)
            throw new ArgumentException("Forma de pagamento inválida");

        await strategy.ProcessPayment(paymentValue, customerId);

        var order = new Order
        {
            Value = paymentValue,
            OrderDate = DateTime.UtcNow // ✅ UTC
        };

        return await InsertOrder(order);
    }

    private async Task<Order> InsertOrder(Order order)
    {
        await _ctx.Orders.AddAsync(order);
        await _ctx.SaveChangesAsync();
        return order;
    }
}