using BAL.Services;
using Behin.Product.WebAPI.Controllers;
using DAB.CharityBox.Test.Mock;
using DAL.Data;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Transactions;

namespace Behin.Product.Test
{
    public class ServiceFactory
    {
        public IProductService GetProductService(ApplicationDbContext mockContext)
        {
            var mockMapper = new AutoMapperInstance().GetMapper();
            var mockLogger = new MockLogger().GetMockedLogger<ProductService>();
            return new ProductService(mockLogger, mockContext, mockMapper);
        }


        public async Task<ProductsController> GetProductController()
        {
            var mockLogger = new MockLogger().GetMockedLogger<ProductService>();
            var mockContext = await DbContextWithDataAsync();
            var mockBoxService = GetProductService(mockContext);
            return new ProductsController(mockLogger, mockBoxService);
        }

        public async Task<ApplicationDbContext> DbContextWithDataAsync()
        {
            var mockContext = (new ConnectionFactory()).CreateContextForInMemory();
            return await BusinessDataAsync(mockContext);
        }

        public ApplicationDbContext DbContextWithoutData()
        {
            return (new ConnectionFactory()).CreateContextForInMemory();
        }

        private async Task<ApplicationDbContext> BusinessDataAsync(ApplicationDbContext context)
        {
            var executionStrategy = context.Database.CreateExecutionStrategy();
            return await executionStrategy.ExecuteAsync(async () =>
            {
                using var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

                await context.Products.AddRangeAsync(MockData.MockProductSamples());
                await context.SaveChangesAsync();
                transaction.Complete();
                return context;
            }).ConfigureAwait(false);
        }

    }
}
