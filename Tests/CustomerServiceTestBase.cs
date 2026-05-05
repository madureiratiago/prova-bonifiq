using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using ProvaPub.Repository;

namespace ProvaPub.Tests;

[TestFixture]
public abstract class CustomerServiceTestBase
{
    protected TestDbContext Context;
    protected Mock<IDateTimeProvider> DateTimeMock;
    protected CustomerService Service;

    [SetUp]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<TestDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        Context = new TestDbContext(options);
        DateTimeMock = new Mock<IDateTimeProvider>();

        Service = new CustomerService(Context, DateTimeMock.Object);
    }
}
