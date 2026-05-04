using Microsoft.EntityFrameworkCore;
using ProvaPub.Models;
using ProvaPub.Repository;

namespace ProvaPub.Services
{
    public class RandomService
    {
        private readonly TestDbContext _ctx;
        private static readonly Random _random = Random.Shared;

        public RandomService()
        {
            var contextOptions = new DbContextOptionsBuilder<TestDbContext>()
                .UseSqlServer(
                    @"Server=(localdb)\mssqllocaldb;Database=Teste;Trusted_Connection=True;")
                .Options;

            _ctx = new TestDbContext(contextOptions);
        }

        public async Task<int> GetRandom()
        {
            int number;
            bool exists;

            do
            {
                number = _random.Next(0, 100);
                exists = await _ctx.Numbers
                    .AnyAsync(n => n.Number == number);
            }
            while (exists);

            _ctx.Numbers.Add(new RandomNumber { Number = number });
            await _ctx.SaveChangesAsync();

            return number;
        }
    }
}
