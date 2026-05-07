
using monacos.us.web.api.Services;
using Swashbuckle.AspNetCore.Filters;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace monacos.us.web.api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);


            //Add support to logging with SERILOG
            builder.Host.UseSerilog((context, configuration) =>
                configuration.ReadFrom.Configuration(context.Configuration));



            // IConfiguration is accessible through the builder.Configuration property
            IConfiguration configuration = builder.Configuration;


            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi

            // Register your services with IOC Container add initialize with DB connection string

            string? dBConnectionString = configuration.GetConnectionString("DBConnection");



            builder.Services.AddScoped<INavigation>(provider =>
            {

                string? dBConnectionString = configuration.GetConnectionString("DBConnection");
                //string? dBConnectionString = _configuration["ConnectionStrings:DBConnection"];


                return new NavigationService(dBConnectionString);
            });

            builder.Services.AddScoped<IContent>(provider =>
            {

                string? dBConnectionString = configuration.GetConnectionString("DBConnection");
                //string? dBConnectionString = _configuration["ConnectionStrings:DBConnection"];


                return new ContentService(dBConnectionString);
            });


            builder.Services.AddScoped<IAuthorize>(provider =>
            {

                string? dBConnectionString = configuration.GetConnectionString("DBConnection");
                //string? dBConnectionString = _configuration["ConnectionStrings:DBConnection"];


                return new AuthorizeService(dBConnectionString, configuration);
            });




            // Support for Minimal APIs
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();


            // Modify To Include Bearer JWT Token Input Dialog In Swagger
            builder.Services.AddSwaggerGen(options =>
            {
                options.AddSecurityDefinition("oauth2", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                {
                    Description = "Standard Authorizaton header using the Bearer scheme (\"bearer {token}\")",
                    In = Microsoft.OpenApi.Models.ParameterLocation.Header,
                    Name = "Authorization",
                    Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey

                });

                // Add To Include Bearer JWT Token In Swagger
                options.OperationFilter<SecurityRequirementsOperationFilter>();

            });


            // Added to Require JWT Bearer Token for Authentication
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetSection("AppSettings:TokenSecretKey").Value!)),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                };
            });


            // Add the following to Conifgure CORS Settings

            builder.Services.AddCors(options =>
            {
                // Configure SpecificOriginsPolicy
                var specificOriginsSection = builder.Configuration.GetSection("CorsSettings:SpecificOriginsPolicy");
                if (specificOriginsSection.Exists())
                {
                    var allowedOrigins = specificOriginsSection["AllowedOrigins"]?.Split(';', StringSplitOptions.RemoveEmptyEntries);
                    var allowedHeaders = specificOriginsSection["AllowedHeaders"]?.Split(';', StringSplitOptions.RemoveEmptyEntries);
                    var allowedMethods = specificOriginsSection["AllowedMethods"]?.Split(';', StringSplitOptions.RemoveEmptyEntries);
                    var allowCredentials = specificOriginsSection.GetValue<bool>("AllowCredentials");

                    options.AddPolicy("SpecificOriginsPolicy", policy =>
                    {
                        if (allowedOrigins != null && allowedOrigins.Length > 0)
                        {
                            policy.WithOrigins(allowedOrigins);
                        }
                        if (allowedHeaders != null && allowedHeaders.Length > 0)
                        {
                            policy.WithHeaders(allowedHeaders);
                        }
                        if (allowedMethods != null && allowedMethods.Length > 0)
                        {
                            policy.WithMethods(allowedMethods);
                        }
                        if (allowCredentials)
                        {
                            policy.AllowCredentials();
                        }
                    });
                };

                // Configure AnyOriginPolicy (if needed)
                /*
                var anyOriginSection = builder.Configuration.GetSection("CorsSettings:AnyOriginPolicy");
                if (anyOriginSection.Exists() && anyOriginSection.GetValue<bool>("AllowAnyOrigin"))
                {
                    options.AddPolicy("AnyOriginPolicy", policy =>
                    {
                        policy.AllowAnyOrigin()
                              .AllowAnyHeader()
                              .AllowAnyMethod();
                    });
                }
                */

            });



            builder.Services.AddOpenApi();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            // if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
                app.UseSwagger();
                app.UseSwaggerUI();

            }

            // Forces HTTP Requests to HTTPS
            //app.UseHttpsRedirection();


            //Add support to logging request with SERILOG
            app.UseSerilogRequestLogging();


            // Add For JWT Support
            app.UseAuthentication();

            app.UseAuthorization();

            // Maps attribute-routed controllers
            app.MapControllers();

            // Use CORS Policy
            app.UseCors("SpecificOriginsPolicy");


            app.Run();
        }
    }
}
