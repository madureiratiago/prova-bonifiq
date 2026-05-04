using ProvaPub.Interfaces;

public class PixPayment : IPaymentStrategy
{
    public string PaymentMethod => "pix";

    public async Task ProcessPayment(decimal value, int customerId)
    {
        // Lógica pagamento PIX
    }
}