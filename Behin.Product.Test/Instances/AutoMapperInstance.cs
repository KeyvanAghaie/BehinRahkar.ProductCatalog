using AutoMapper;

namespace Behin.Product.Test
{
    public class AutoMapperInstance
    {
        public IMapper GetMapper()
        {
            var autoMapperConf = new AutomapperConfiguration();
            var mapper = new Mapper(autoMapperConf.CreateConfiguration());

            return mapper;
        }
    }
}
