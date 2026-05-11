

/********************* Development Notes/Steps ******************/


// Add Swagger Support

Install-Package Swashbuckle.AspNetCore -Version 6.6.2


// Add CORS Support

Install-Package Microsoft.AspNetCore.Cors

!!!!Also remember to Configure the Azure App Service For the Proper CORS Domain Information!!!

Azure: App Service -> API -> CORS  -> Allowed Origins


// App Settings

 // Added Settings
  "AppSettings": {
    "TokenSecretKey": "77dc058d2b691d08a1300c770a20779ea41b983b0dcaa4251fdc10946715ecdcb168f93624220f4a129410a35aa33b2298d245e1168c56b569e22ddd36e112e8",
    "DBConnectionJWTAuthentication": "Server=HAL9000;Database=JWTAuthentication;User Id=orlanmon;Password=GoWestYoungMan_1973;Encrypt=False;"
  },
  "CorsSettings": {
    "SpecificOriginsPolicy": {
      "AllowedOrigins": "http://localhost:4200;",
      "AllowedHeaders": "Content-Type;Authorization;",
      "AllowedMethods": "GET;POST;PUT;DELETE;OPTIONS;",
      "AllowCredentials": true
    },
    "AnyOriginPolicy": {
      "AllowAnyOrigin": true,
      "AllowedHeaders": "*",
      "AllowedMethods": "*"
    }
  }


  // Add Services Folder and Interfaces and Service Implementation

  IMenu
  IContent
  IAuthorize


  // Add Models Folder

  Add Needed DTO Models







  // Program.cs Changes



  //  Dependency Injection for all Services

builder.Services.AddControllers();

builder.Services.AddScoped<IMenu, MenuService>(); // Register your service
etc


// Add Swagger Support for JWT Input 

install packages


Install-Package Swashbuckle.AspNetCore.Filters 8.0.2
Install-Package System.IdentityModel.Tokens.Jwt 8.16.0
Install-Package Microsoft.AspNetCore.Authentication.JwtBearer 9.0.14
Install-Package Microsoft.IdentityModel.Tokens 8.16.0



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




// Add to Require JWT Bearer Token for Authentication

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


// Add For JWT Support
app.UseAuthentication();




// app.MapControllers();




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



            // Add to Use CORS Policy
            app.UseCors("SpecificOriginsPolicy");


            // app.Run();



// Add The Following to Enable Swagger Dependent on Environment
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
                app.UseSwagger();
                app.UseSwaggerUI();

            }


// Encryption Library 

BCrypt.Net-Core  1.6.0




/********************* JWT Notes ******************/


Secured Jason Web Token RESTful Web API

How to use this demo.

#1 Register a User Name and Password with the system.  This will store the user name and password hash in a database, pass back the Password Hash.
#2 Login by providing a registered User Name and Password.   User name and Password will be verified against database, reverse hash comparison on password.   
   JWT will then be passed back to be used in subsequent Web API Calls.

   Verify JWT with https://www.jwt.io/

#3 Now Make Web API Calls passing JWT in the Authorization Header of each Web API Call.


// General Notes

JWT 

https://supertokens.com/blog/what-is-jwt

JSON Web Token is an open industry standard (RFC 7519)

Token is a string that contains some information that can be verified securely

Header: Consists of two parts:
The signing algorithm that’s being used.
The type of token, which, in this case, is mostly “JWT”.

Payload: The payload contains the claims for the JSON object. 
Also contains fields like exp (expire date),   iat (time jwt created), etc

Signature: A string that is generated via a cryptographic algorithm that can be used to verify the integrity of the JSON payload.
Base64URLSafe(HMACHSHA256(<header>, <payload>, <secret key>))



Issuer Signing Key

TokenSecretKey = Secret Key    User   JWTSecrets Online to Generate Token Secret



https://www.youtube.com/watch?v=UwruwHl3BlU

https://www.youtube.com/watch?v=TDY_DtTEkes


https://jwt.io/introduction

https://medium.com/code-wave/how-to-make-your-own-jwt-c1a32b5c3898


Debug ASP.NET CORE IN IIS

https://learn.microsoft.com/en-us/visualstudio/debugger/how-to-enable-debugging-for-aspnet-applications?view=vs-2022




Swagger specify header as:  bearer "JWT Token String"


bearer eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTUxMiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoib3JsYW5kbyIsImh0dHA6Ly9zY2hlbWFzLm1pY3Jvc29mdC5jb20vd3MvMjAwOC8wNi9pZGVudGl0eS9jbGFpbXMvcm9sZSI6IkFkbWluIiwiZXhwIjoxNzM2MTAxNDQ2fQ.2GfymtiEvMjTzsA-Ce2gePh6gRJAkkduQKg0yO9GJzq4jiThTpgpfgx8angTISxBCgREMy8tBVS5q2EamyHnPw



// JWT

            /* Header.Payload.Signature */

            // Header 

            /*
            {
                "alg": "HmacSha512",   signing algorithm
                "typ": "JWT"
            } 
            
            Header is Base64Url encoded
            */

            // Payload

            /*
             Registered Claims
             Payload is Base64Url encoded
            */

            // Signature

            /*
             
            HMACSHA256(
            base64UrlEncode(header) + "." +
            base64UrlEncode(payload),
            secret)

            To create the signature part you have to take the encoded header, 
            the encoded payload, a secret, the algorithm specified in the header, and sign that.
            */



/*  Some SQL Commands That Come in Handy */

EXEC sp_changedbowner 'sa'

GRANT execute ON dbo.sp_GetUser to orlanmon
GRANT execute ON dbo.sp_InsertUser to orlanmon


/* Adding Logging using ILogger */

Install Pacakages 

Serilog.AspNetCore

Microsoft.Extensions.Logging.Abstractions

Microsoft.Extensions.Primitives

Microsoft.Extensions.Options



https://medium.com/@brucycenteio/adding-serilog-to-asp-net-core-net-7-8-5cba1d0dea2



In IIS Grant Your Application Pool User Write and Modify Permissions to the root directory of application.

User:   IIS AppPool\MonacosUsWebAPI













