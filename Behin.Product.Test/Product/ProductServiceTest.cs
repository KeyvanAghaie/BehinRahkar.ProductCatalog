using Microsoft.AspNetCore.Mvc;
using Services.DTO;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Behin.Product.Test
{
    public class ProductServiceTest
    {

        #region Get
        [Fact]
        public async Task GetProductById_WithSampleInputId_MustReturnCorrectDataAsync()
        {
            //Arrange
            var serviceFactory = new ServiceFactory();
            var context = await serviceFactory.DbContextWithDataAsync();
            var service = (new ServiceFactory()).GetProductService(context);
            var product = MockData.MockProductSamples()[0];
            //Act
            var result = await service.GetProductByIdAsync(product.Id);
            //Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task GetProductByCode_WithSampleInputCode_MustReturnCorrectDataAsync()
        {
            //Arrange
            var serviceFactory = new ServiceFactory();
            var context = await serviceFactory.DbContextWithDataAsync();
            var service = (new ServiceFactory()).GetProductService(context);
            var product = MockData.MockProductSamples()[0];
            //Act
            var result = await service.GetproductByCodeAsync(product.Code);
            //Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task GetProducts_WithNoInput_MustReturnPagedDataAsync()
        {
            //Arrange
            var serviceFactory = new ServiceFactory();
            var context = await serviceFactory.DbContextWithDataAsync();
            var service = (new ServiceFactory()).GetProductService(context);
            int length = MockData.MockProductSamples().Where(x => !x.Deleted).Count();
            //Act
            var result = await service.GetAllProductsAsync(null, 0, 10);
            //Assert
            Assert.Equal(length, result.TotalItems);
        }


        [Fact]
        public async Task GetBoxes_Withfilter_MustReturnfiteredListAsync()
        {
            //Arrange
            var serviceFactory = new ServiceFactory();
            var context = await serviceFactory.DbContextWithDataAsync();
            var service = (new ServiceFactory()).GetProductService(context);
            int length = MockData.MockProductSamples().Where(x => !x.Deleted).Count();
            //Act
            var result = await service.GetAllProductsAsync("101", 0, 10);
            //Assert
            Assert.True(result.TotalItems == 1);
        }

        #endregion

        #region Create
        [Fact]
        public async Task CreateAsync_WithCorrectData_MustCreateProductAsync()
        {
            //Arrange
            var serviceFactory = new ServiceFactory();
            var context = serviceFactory.DbContextWithoutData();
            var service = (new ServiceFactory()).GetProductService(context);
            var product = MockData.MockProductSamples()[0];
            var productModel = new ProductDTO(0, product.Code, product.Name , product.Photo , product.Price);
            //Act
            var result = await service.CreateProductAsync(productModel);

            //Assert
            Assert.Equal("100", result.Code);
            Assert.NotNull(result);
        }

        [Fact]
        public async Task CreateAsync_WithDuplicateCode_MustReturnExceptionAsync()
        {
            //Arrange
            var controller = await (new ServiceFactory()).GetProductController();
            var mapper = (new AutoMapperInstance()).GetMapper();
            foreach (var productItem in MockData.MockProductSamples())
            {
                await controller.CreateProduct(mapper.Map<ProductDTO>(productItem));
            }
            var product = MockData.MockProductSamples()[0];
            var productModel = new ProductDTO(0, product.Code, product.Name, product.Photo, product.Price);

            //Act
            var result = await controller.CreateProduct(productModel);
            var errorResult = result as StatusCodeResult;

            //Assert
            Assert.NotNull(errorResult);
            Assert.Equal(500,  errorResult.StatusCode);
        }

        #endregion

        #region Update
        [Fact]
        public async Task UpdateAsync_WithCorrectAndExistItem_MustUpdateProductAysnc()
        {
            //Arrange
            var serviceFactory = new ServiceFactory();
            var context = await serviceFactory.DbContextWithDataAsync();
            var service = serviceFactory.GetProductService(context);
            var product = MockData.MockProductSamples()[0];
            var productModel = new ProductDTO(1001, "155", product.Name, product.Photo, product.Price);
            //Act
            var result = await service.UpdateProductAsync(productModel);
            //Assert
            Assert.Equal("155", result.Code);
        }

        #endregion

        #region Delete
        [Fact]
        public async Task DeleteAsync_WithCorrectData_MustReturnTrueAsync()
        {
            //Arrange
            var serviceFactory = new ServiceFactory();
            var context = await serviceFactory.DbContextWithDataAsync();
            var service = (new ServiceFactory()).GetProductService(context);
            var product = MockData.MockProductSamples()[1];

            //Act
            var result = await service.DeleteProductAsync(product.Id);

            //Assert
            Assert.True(result);
        }
        #endregion

    }
}
