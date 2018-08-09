using HT.CheckerApp.API.Models;
using HT.CheckerApp.API.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Swagger;
namespace HT.CheckerApp.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var configurationSection = Configuration.GetSection("ConnectionStrings:DefaultConnection");
            services.AddDbContext<DataBaseContext>(options => options.UseSqlServer(configurationSection.Value));
            // Add framework services.

            services.AddMvc();
            services.AddMemoryCache();
            services.AddScoped<IPBDrawResultRepository, PBDrawResultRepository>();
            services.AddScoped<IPBOnDemandRepository, PBOnDemandRepository>();
            services.AddScoped<IPBSubscriptionsRepository, PBSubscriptionsRepository>();
            services.AddScoped<IPBSubscriptionRevenueRepository, PBSubscriptionRevenueRepository>();

            // Register the Swagger generator, defining one or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "My API", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });

            AutoMapper.Mapper.Initialize(config =>
            {
                config.CreateMap<PBDrawResult, PBDrawResultViewModel>().ReverseMap();
                config.CreateMap<PBOnDemand, PBOnDemandViewModel>().ReverseMap();
                config.CreateMap<PBSubscriptionRevenue, PBSubscriptionRevenueViewModel>().ReverseMap();
                config.CreateMap<PBSubscriptions, PBSubscriptionsViewModel>().ReverseMap();
            });

            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseDefaultFiles();
            app.UseStaticFiles();

            app.UseMvcWithDefaultRoute();
        }
    }
}
