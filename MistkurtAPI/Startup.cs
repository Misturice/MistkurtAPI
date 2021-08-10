using Contracts;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using MistkurtAPI.Classes.Common;
using MistkurtAPI.Extensions;
using NLog;
using System;
using System.IO;

namespace MistkurtAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            LogManager.LoadConfiguration(String.Concat(Directory.GetCurrentDirectory(), "/nlog.config"));
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.ConfigureAuthentication();
            services.ConfigureLoggerService();
            services.ConfigureCors();
            services.ConfigureJWT();
            services.ConfigurePostgresContext(Configuration);
            services.ConfigureSwagger();
            services.ConfigureRepositoryWrapper();
            services.ConfigureActionFilters();

            services.AddAutoMapper(typeof(Startup));

            services.ConfigureControllers();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerManager logger)
        {
            if (env.IsDevelopment())
            {
                app.UseCors("AllowAngularDevClient");
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "MistkurtAPI v1"));
            }

            app.ConfigureExceptionHandler(logger);

            app.UseMiddleware<RequestLoggingMiddleware>();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

    }
}
