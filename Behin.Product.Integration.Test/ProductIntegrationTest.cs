using Behin.Product.WebAPI;
using Microsoft.AspNetCore.Mvc.Testing;
using Services.DTO;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;

namespace Behin.Product.Integration.Test
{
    public class ProductControllerTest : IClassFixture<WebApplicationFactory<Startup>>
    {
        #region Fields
        private WebApplicationFactory<Startup> _factory = null;
        #endregion

        #region Constructors
        public ProductControllerTest(WebApplicationFactory<Startup> factory) => this._factory = factory;
        #endregion

        #region Public methods
        [Fact]
        public async Task POST_Login_GetBearerAndUser()
        {
            HttpClient client = this._factory.CreateClient(new WebApplicationFactoryClientOptions()
            {
                AllowAutoRedirect = false
            });

            var model = new ProductDTO()
            {
               Code = "250" ,
               Name = "Product1" ,
               Price = 998
            };
            var response = await client.PostAsJsonAsync<ProductDTO>(@"/api/products/createproduct", model);

            string responseAsString = await response.Content.ReadAsStringAsync();

            Assert.True(response.IsSuccessStatusCode);
            Assert.Equal("application/json; charset=utf-8", response.Content.Headers.ContentType.ToString());


            model = await response.Content.ReadFromJsonAsync<ProductDTO>();
           
        }
        #endregion
    }
}
