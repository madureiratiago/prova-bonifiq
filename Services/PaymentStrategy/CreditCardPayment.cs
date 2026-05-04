using ProvaPub.Interfaces;

public class CreditCardPayment : IPaymentStrategy
{
    public string PaymentMethod => "creditcard";

    public async Task ProcessPayment(decimal value, int customerId)
    {
        // Lógica pagamento cartão
    }
}