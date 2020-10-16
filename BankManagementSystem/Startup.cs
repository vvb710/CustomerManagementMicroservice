using BankManagementSystem.Extensions;
using BankManagementSystem.Logger;
using BankManagementSystem.Logging;
using BankManagementSystem.Repository;
using BankManagementSystem.Validation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NLog;
using System;
using System.IO;
using System.Linq;
using static BankManagementSystem.Models.ApplyMethods;

namespace BankManagementSystem
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            LogManager.LoadConfiguration(string.Concat(Directory.GetCurrentDirectory(), "\\Logger\\nlog.config"));
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.ConfigureSqlServerContext(Configuration);
            services.ConfigureRepositoryWrapper();
            services.ConfigureGenericRepository();
            services.ConfigureServices();
            services.ConfigureLoggerService();
            services.AddMvc(option =>
            {
                option.Filters.Add(typeof(ValidateException));
                option.Filters.Add(typeof(LoggingActionFilter));
            });

            services.AddApiVersioning(v =>
            {
                v.AssumeDefaultVersionWhenUnspecified = true;
                v.DefaultApiVersion = new ApiVersion(1, 0);
            });

            services.AddSwaggerGen(s =>
            {
                s.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "CustomerAPI", Version = "v1", Description = "API version 1" });
                s.SwaggerDoc("v2", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "CustomerAPI", Version = "v2", Description = "API version 2" });
                s.ResolveConflictingActions(a => a.First());
                s.OperationFilter<RemoveVersionFromParameter>();
                s.DocumentFilter<ReplaceVersionWithExactValueInPath>();
            });

            services.ConfigureConsulService(Configuration);

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ApplicationDbContext applicationDbContext)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //app.ConfigureExceptionHandler(logger);

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "API version 1 for Customers");
                c.SwaggerEndpoint("/swagger/v2/swagger.json", "API version 2 for Customers");
            });

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseDeveloperExceptionPage();

            app.UseConsul(Configuration);

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            applicationDbContext.Database.EnsureCreated();
        }


    }
}
