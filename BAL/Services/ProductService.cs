using AutoMapper;
using DAL.Data;
using DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Services.DTO;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BAL.Services
{
    public class ProductService : IProductService
    {
        private readonly ILogger _logger;
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public ProductService(ILogger logger, ApplicationDbContext context, IMapper mapper)
        {
            _logger = logger;
            _context = context;
            _mapper = mapper;
        }


        public async Task<Product> GetProductByIdAsync(long productId)
        {
            return await _context.Products.FindAsync(productId);
        }


        public async Task<Product> GetproductByCodeAsync(string code)
        {
            return await _context.Products.FirstOrDefaultAsync(p => p.Code.Contains(code));
        }


        public async Task<PaginationModel<Product>> GetAllProductsAsync(string filter, int? paginationIndex, int? paginationSize)
        {
            var response = new PaginationModel<Product>();

            var products = _context.Products.Where(f => !f.Deleted);

            if (!string.IsNullOrEmpty(filter))
                products = products
                .OrderByDescending(f => f.Id)
                    .Where(f => f.Code.Contains(filter));

            if (paginationIndex.HasValue && paginationSize.HasValue)
                products = products.Skip((paginationIndex.Value)
                    * paginationSize.Value).Take(paginationSize.Value);

            response.Items = await products.ToListAsync();
            response.TotalItems = products.Count();
            response.PageItems = response.Items.Count();
            return response;
        }


        public async Task<Product> CreateProductAsync(ProductDTO product)
        {

            var dbProduct = _mapper.Map<Product>(product);
            dbProduct.CreatedDate = DateTime.Now;
            await _context.AddAsync(dbProduct);
            await _context.SaveChangesAsync();
            _logger.Information("Product catalog is created !");
            return dbProduct;

        }


        public async Task<Product> UpdateProductAsync(ProductDTO product)
        {
            var productDb = await _context.Products.FindAsync(product.Id);
            if (productDb == null)
                throw new DataNotFoundException();

            productDb.Code = product.Code;
            productDb.Name = product.Name;
            productDb.Photo = product.Photo;
            productDb.Price = product.Price;

            _context.Update(productDb);
            _logger.Information("Product catalog is edited !");
            await _context.SaveChangesAsync();
            return productDb;
        }

        public async Task<bool> DeleteProductAsync(long id)
        {
            var productDb = await GetProductByIdAsync(id);
            if (productDb == null)
                throw new DataNotFoundException();

            productDb.Deleted = true;
            await _context.SaveChangesAsync();
            _logger.Information("Product catalog is removed !");
            return productDb.Deleted;
        }


    }
}
