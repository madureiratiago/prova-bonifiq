using ProvaPub.Interfaces;
using ProvaPub.Models;
using ProvaPub.Repository;

namespace ProvaPub.Services
{
    public class ProductService : PagedService<Product>, IProductService
    {
        private readonly TestDbContext _ctx;

        public ProductService(TestDbContext ctx) : base(ctx) { }

        public PagedResult<Product> ListProducts(int page)
        {
            return GetPaged(_ctx.Products.AsQueryable(), page);
        }
    }
}


