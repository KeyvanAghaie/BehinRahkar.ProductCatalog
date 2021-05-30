using DAL.Data;
using DAL.Entities;
using DAL.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DAL.Repository
{
    class ProductRepository : IRepository<Product>
    {
        ApplicationDbContext _dbContext;
        public ProductRepository(ApplicationDbContext applicationDbContext)
        {
            _dbContext = applicationDbContext;
        }
        public async Task<Product> Create(Product _object)
        {
            var obj = await _dbContext.Products.AddAsync(_object);
            _dbContext.SaveChanges();
            return obj.Entity;
        }

        public void Delete(Product _object)
        {
            _dbContext.Remove(_object);
            _dbContext.SaveChanges();
        }

        public IEnumerable<Product> GetAll()
        {
            try
            {
                return _dbContext.Products.Where(x => x.Deleted == false).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public Product GetById(int Id)
        {
            return _dbContext.Products.Where(x => x.Deleted == false && x.Id == Id).FirstOrDefault();
        }

        public void Update(Product _object)
        {
            _dbContext.Products.Update(_object);
            _dbContext.SaveChanges();
        }
    }
}
