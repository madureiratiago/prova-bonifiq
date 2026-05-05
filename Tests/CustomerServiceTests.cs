using NUnit.Framework;
using ProvaPub.Models;

namespace ProvaPub.Tests
{
    public class CustomerServiceTests : CustomerServiceTestBase
    {
        public CustomerServiceTests():base()
        {
                
        }

        [Test(Description = "Cliente inexistente")]
        public void CanPurchase_CustomerDoesNotExist_ThrowsException()
        {
            Assert.ThrowsAsync<InvalidOperationException>(new AsyncTestDelegate(() => Service.CanPurchase(99, 50)));
        }

        [Test(Description = "CustomerId inválido")]
        public void CanPurchase_InvalidCustomerId_ThrowsException()
        {
            Assert.ThrowsAsync<ArgumentOutOfRangeException>(new AsyncTestDelegate(() => Service.CanPurchase(0, 50)));
        }

        [Test(Description = "Valor de compra inválido")]
        public void CanPurchase_InvalidPurchaseValue_ThrowsException()
        {
            Assert.ThrowsAsync<ArgumentOutOfRangeException>(new AsyncTestDelegate(() => Service.CanPurchase(1, 0)));
        }

        [Test(Description = "Cliente já comprou no mês")]
        public async Task CanPurchase_AlreadyBoughtThisMonth_ReturnsFalse()
        {
            DateTimeMock.Setup(d => d.UtcNow)
                .Returns(new DateTime(2025, 5, 10, 10, 0, 0));

            var customer = new Customer
            {
                Id = 1,
                Orders = new List<Order>
                {
                    new() { OrderDate = DateTimeMock.Object.UtcNow.AddDays(-5) }
                }
            };

            Context.Customers.Add(customer);
            Context.SaveChanges();

            var result = await Service.CanPurchase(1, 50);

            Assert.Equals(false, result);
        }

        [Test(Description = " Primeira compra acima do limite")]
        public async Task CanPurchase_FirstPurchaseAboveLimit_ReturnsFalse()
        {
            DateTimeMock.Setup(d => d.UtcNow)
                .Returns(new DateTime(2025, 5, 12, 10, 0, 0));

            Context.Customers.Add(new Customer { Id = 1 });
            Context.SaveChanges();

            var result = await Service.CanPurchase(1, 150);
            
            Assert.Equals(false, result);
        }

        [Test(Description = "Fora do horário comercial")]
        public async Task CanPurchase_OutsideBusinessHours_ReturnsFalse()
        {
            DateTimeMock.Setup(d => d.UtcNow)
                .Returns(new DateTime(2025, 5, 12, 7, 30, 0));

            Context.Customers.Add(new Customer { Id = 1 });
            Context.SaveChanges();

            var result = await Service.CanPurchase(1, 50);

            Assert.Equals(false, result);
        }

        [Test(Description = "Compra válida")]
        public async Task CanPurchase_ValidScenario_ReturnsTrue()
        {
            DateTimeMock.Setup(d => d.UtcNow)
                .Returns(new DateTime(2025, 5, 13, 10, 0, 0));

            Context.Customers.Add(new Customer { Id = 1 });
            Context.SaveChanges();

            var result = await Service.CanPurchase(1, 80);

            Assert.Equals(true, result);
        }
    }
}
