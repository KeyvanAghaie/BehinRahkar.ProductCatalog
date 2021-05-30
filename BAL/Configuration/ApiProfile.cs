using AutoMapper;
using DAL.Entities;
using Services.DTO;

namespace BAL.Configuration
{
    public class ApiProfile : Profile
    {
        public ApiProfile()
        {
            CreateMap<Product, ProductDTO>()
              .ReverseMap(); 
            
            CreateMap<ProductDTO, Product>()
              .ReverseMap();
        }
    }
}
