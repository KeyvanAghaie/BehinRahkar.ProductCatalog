using AutoMapper;
using BAL.Configuration;
using BAL.Services;
using DAL.Data;
using DAL.Entities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Serilog;
using System;

namespace Behin.Product.WebAPI
{
    public class Startup
    {
        private static AppSettings appSettings;
        private readonly string AllowedOriginsName = "_allowedOrigins";
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
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

            ConfigureLogger(services);
            if (appSettings.EnableCORS)
            {
                services.AddCors(options =>
                    options.AddPolicy(name: AllowedOriginsName,
                        builder =>
                        {
                            builder.WithOrigins(appSettings.CORSTrustedOrigins)
                            .AllowAnyHeader()
                            .AllowAnyMethod()
                            .AllowCredentials();
                        })
                    );
            }

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Behin.Product.WebAPI v1"));
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private void ConfigureLogger(IServiceCollection services)
        {
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            services.AddSingleton<ILogger>(f =>
            {
                var logConf = new LoggerConfiguration()
                    .ReadFrom.Configuration(Configuration)
                    .Enrich.FromLogContext()
                    .Enrich.WithCorrelationId()
                    .WriteTo.File(Configuration["Serilog:LogFileName"],
                                  restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Error,
                                  flushToDiskInterval: TimeSpan.FromMinutes(1),
                                  rollOnFileSizeLimit: true,
                                  encoding: System.Text.Encoding.UTF8,
                                  outputTemplate: "{NewLine}{Timestamp:yyyy/MM/dd HH:mm:ss} [{Level}] ({CorrelationToken}) {Message}{NewLine}{Exception}")
                    .WriteTo.MSSqlServer(connectionString: appSettings.ConnectionStrings.MSSQL,
                        sinkOptions: new Serilog.Sinks.MSSqlServer.MSSqlServerSinkOptions
                        {
                            AutoCreateSqlTable = true,
                            TableName = "Logs"
                        }).Enrich.WithProperty("Environment", environment)
                        .Enrich.WithProperty("ModuleName", "ProductCatalog"); ;

                return logConf.CreateLogger();
            });
        }

        private static void ConfigureVersioning(IServiceCollection services)
        {
            services.AddApiVersioning(config =>
            {
                config.DefaultApiVersion = new ApiVersion(1, 0);
                config.AssumeDefaultVersionWhenUnspecified = true;
                config.ReportApiVersions = true;
            });

            services.AddVersionedApiExplorer(options =>
            {
                // add the versioned api explorer, which also adds IApiVersionDescriptionProvider service
                // note: the specified format code will format the version as "'v'major[.minor][-status]"
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;
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

        private static void UpgradeDatabase(IApplicationBuilder app)
        {
            using var serviceScope = app.ApplicationServices.CreateScope();
            var logger = serviceScope.ServiceProvider.GetService<ILogger>();
            try
            {
                logger.Information("Migrating the database...");
                var context = serviceScope.ServiceProvider.GetService<ApplicationDbContext>();
                if (context != null && context.Database != null)
                {
                    context.Database.Migrate();
                }
                logger.Information("Migration has been done successfully.");

                try
                {
                    //logger.Information("Seeding information.");
                    //var dbContext = serviceScope.ServiceProvider.GetService<DataContext>();
                    //var seedData = new SeedData(dbContext);
                    //seedData.SeedCountries().Wait();
                    //seedData.SeedOccupations().Wait();
                    //logger.Information("Seeding information completed.");
                }
                catch (Exception ex)
                {
                    logger.Error(ex, "An error occurred when seeding information.");
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex, "An error occurred when migrating the database.");
            }
        }

    }
}
