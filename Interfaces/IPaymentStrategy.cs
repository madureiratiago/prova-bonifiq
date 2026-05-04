namespace ProvaPub.Interfaces;

public interface IPaymentStrategy
{
    string PaymentMethod { get; }
    Task ProcessPayment(decimal value, int customerId);
}
