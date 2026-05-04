using ProvaPub.Interfaces;

public class PaypalPayment : IPaymentStrategy
{
    public string PaymentMethod => "paypal";

    public async Task ProcessPayment(decimal value, int customerId)
    {
        // Lógica pagamento PayPal
    }
}
