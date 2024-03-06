using Grpc.Core;
using GrpcService1.DataAccess;
using GrpcService1.Protos;
using Microsoft.EntityFrameworkCore;

namespace GrpcService1.Services
{
    public class CategoryService : CategoryRpcService.CategoryRpcServiceBase
    {
        private readonly AppDbContext _context;

        public CategoryService(AppDbContext context)
        {
            this._context = context;
        }

        public override async Task<CategoryList> SelectAll(Nothing request, ServerCallContext context)
        {
            var categories = await _context.Categories
                .Include(c => c.Products)
                .ToListAsync();

            var categoryEntity = from c in categories
                                 select new CategoryEntity()
                                 {
                                     CategoryId = c.CategoryId,
                                     CategoryName = c.CategoryName,
                                     Products = {
                                         c.Products.Select(p => new ProductEntity
                                         {
                                             ProductId = p.ProductId,
                                             ProductName = p.ProductName,
                                             CategoryId = p.CategoryId,
                                         })
                                     }
                                 };

            return new CategoryList { Categories = { categoryEntity } };
        }
    }
}