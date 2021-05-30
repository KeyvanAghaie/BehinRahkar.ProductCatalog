using AutoMapper;
using BAL.Configuration;
using BAL.Services;
using Behin.Product.WebAPI;
using DAL.Data;
using DAL.Entities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System;

namespace Behin.IntegrationTests
{
    public class FakeStartup //: Startup
    {

        private static AppSettings appSettings;
        private readonly string AllowedOriginsName = "_allowedOrigins";
        public FakeStartup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public virtual void ConfigureServices(IServiceCollection services)
        {
            services.Configure<AppSettings>(Configuration);
            appSettings = Configuration.Get<AppSettings>();
            ConfigureDatabase(services);
            services.AddControllers();
            services.AddHttpClient();
            // Auto Mapper Configurations
            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new ApiProfile());
            });
            IMapper mapper = mapperConfig.CreateMapper();
            services.AddSingleton(mapper);

            services.AddTransient<IProductService, ProductService>();
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Behin.Product.WebAPI", Version = "v1" });
            });

            
        



        }
        
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
           
            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }



        private static void ConfigureDatabase(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(appSettings.ConnectionStrings.MSSQL, sqlserverOptions =>
                {
                    sqlserverOptions.CommandTimeout(180); // 3 minutes
                    sqlserverOptions.EnableRetryOnFailure(3);
                    sqlserverOptions.MigrationsAssembly(typeof(Startup).Assembly.GetName().Name);
                    sqlserverOptions.EnableRetryOnFailure(
                                        maxRetryCount: 10,
                                        maxRetryDelay: TimeSpan.FromSeconds(30),
                                        errorNumbersToAdd: null);
                });

                options.EnableDetailedErrors(true);
            });
        }

    }
}