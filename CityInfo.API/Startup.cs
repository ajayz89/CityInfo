using AutoMapper;
using CityInfo.API.Entities;
using CityInfo.API.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Serialization;
using NLog.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CityInfo.API
{
    public class Startup
    {

        public static IConfigurationRoot Configuration;

        public Startup(IHostingEnvironment env) {
            var builder = new ConfigurationBuilder().SetBasePath(env.ContentRootPath)
                .AddJsonFile("appSettings.Json", optional:false, reloadOnChange:true)
                .AddJsonFile($"appSettings.{env.EnvironmentName}.Json", optional: true, reloadOnChange: true);
            Configuration = builder.Build();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            // manipulate naming conventions for Json 
            services.AddMvc()
                .AddJsonOptions(k =>
                {
                    if (k.SerializerSettings.ContractResolver != null)
                    {
                        var castedRes = k.SerializerSettings.ContractResolver as DefaultContractResolver;
                        castedRes.NamingStrategy = null;
                    }
                })
               .AddMvcOptions(l => l.OutputFormatters.Add(new XmlDataContractSerializerOutputFormatter()));
            services.AddTransient<IMailService,LocalMailService>();
            services.AddDbContext<CityInfoContext>(k => k.UseSqlServer(Startup.Configuration["DefaultConnectionString"]));
            services.AddScoped<ICityInfoRepository, CityInfoRepository>();

            var config = new MapperConfiguration(
                cfg => {
                    cfg.CreateMap<Entities.City, Models.CityWithoutPOI>();
                    cfg.CreateMap<Entities.City, Models.CityDto>();
                    cfg.CreateMap<Entities.PointOfInterest, Models.PointOfInterestDto>();
                    cfg.CreateMap<Entities.PointOfInterest, Models.CreatePointOfInterestDto>();
                    cfg.CreateMap<Models.CreatePointOfInterestDto, Entities.PointOfInterest>();
                    cfg.CreateMap<Models.PointOfInterestDto, Entities.PointOfInterest>();
                }
                ) ;
            var mapper = config.CreateMapper();

            services.AddSingleton(mapper);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, CityInfoContext cityctx)
        {
            loggerFactory.AddConsole();
            loggerFactory.AddDebug();
            // one way of integrating
            loggerFactory.AddProvider(new NLogLoggerProvider());
            // or easily 
           // loggerFactory.AddNLog();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else {
                app.UseDeveloperExceptionPage();
            }

            cityctx.EnsureSeedDataForContext();
            app.UseStatusCodePages();
       
            app.UseMvc();
            //app.Run(async (context) =>
            //{
            //    await context.Response.WriteAsync("Hello World!");
            //});

        }
    }
}
