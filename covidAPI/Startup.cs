using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CornerKitchen.Tech.UnitOfWork.Functions;
using CornerKitchen.Tech.UnitOfWork.UnitOfWork;
using covidAPI.DataAccess;
using covidAPI.DataAccess.context;
using covidAPI.DataAccess.@interface;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;

namespace covidAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers().AddNewtonsoftJson(opt => opt.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore);
            services.AddSwaggerGen(x =>
            {
                x.SwaggerDoc(Configuration.GetSection("Swagger")["Version"], new OpenApiInfo
                {
                    Title = Configuration.GetSection("Swagger")["Title"],
                    Description = Configuration.GetSection("Swagger")["Description"],
                    Version = Configuration.GetSection("Swagger")["Version"]
                });
            });

            services.Configure<AppSettings>(Configuration);
            services.AddHealthChecks();

            //services.AddInfra
            string mySqlConnectionStr = Configuration.GetConnectionString("CK_DB");
            services.AddDbContextPool<databaseContext>(options => options.UseMySql(mySqlConnectionStr, ServerVersion.AutoDetect(mySqlConnectionStr)));

            services
                .AddSingleton<IAppSettings>(sp => (IAppSettings)sp.GetRequiredService<IOptions<AppSettings>>().Value)
                .AddScoped(typeof(IUnitOfWork<>), typeof(UnitOfWork<>))
                .AddScoped(typeof(IFunctions<>), typeof(Functions<>))
                .AddScoped<IcovidDataAccess, covidDataAccess>()
                .AddHttpClient();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseDeveloperExceptionPage();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseSwagger(x =>
            {
                x.PreSerializeFilters.Add((swaggerDoc, httpReq) =>
                {
                    swaggerDoc.Servers = new List<OpenApiServer> { new OpenApiServer { Url = $"https://{httpReq.Host.Value}{Configuration.GetSection("Swagger")["BasePath"]}" } };
                });
            });

            app.UseSwaggerUI(opt =>
            {
                opt.SwaggerEndpoint(Configuration.GetSection("Swagger")["Endpoint"], Configuration.GetSection("Swagger")["Name"]);
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseHealthChecks("/health");
        }
    }
}
