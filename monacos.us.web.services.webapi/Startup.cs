using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dto = monacos.us.web.services.dto;
using monacos.us.web.services.webapi.ServiceImplementation;


namespace monacos.us.web.services.webapi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

            // Configure AutoMapper For Model to DTOs
            dto.AutoMapperConfiguration.Configure();


        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {


            // Depedency Injection of Web API Service into the Contollers

            string DatabaseConnectionString = Configuration.GetConnectionString("DatabaseConnection");

            services.AddTransient<IContentService> (s => new ContentService(DatabaseConnectionString));


            // Pre Controller Action Call
            //services.Add(new ServiceDescriptor(typeof(IContentService), typeof(ContentService), ServiceLifetime.Transient)); // Transient

            services.AddControllers();

            services.AddSwaggerGen();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {

                endpoints.MapControllers();


                //endpoints.MapControllerRoute("default", "api/{controller=HomeController}/{action=IntroPage}");

            });

            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            //app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

            app.UseHttpsRedirection();

            //app.UseAuthorization();

            /*
            if (env.IsDevelopment())
            {
                app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
                options.RoutePrefix = string.Empty;
            });
            };
            */


        }
    }
}
