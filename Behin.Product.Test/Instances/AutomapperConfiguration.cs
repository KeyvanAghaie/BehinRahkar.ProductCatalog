using AutoMapper;
using BAL.Configuration;

namespace Behin.Product.Test
{
    public class AutomapperConfiguration
    {
        public MapperConfiguration CreateConfiguration()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddMaps(new ApiProfile().GetType().Assembly);
            });

            return config;
        }
    }
}
