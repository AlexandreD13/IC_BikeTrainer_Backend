using System.Globalization;
using Microsoft.OpenApi.Models;
using System.Reflection;

namespace IC_BikeTrainer_Backend.Configuration
{
    public static class SwaggerConfig
    {
        public static void ConfigureSwagger(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSwaggerGen(options =>
            {
                // Swagger Header
                var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
                
                var databaseName = configuration["Database:Path"].Split("/")[2].Split(".")[0];
                var capitalizedDatabaseName = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(databaseName.ToLower());

                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "IC BikeTrainer API",
                    Version = $"v1.0.0 | Current Database: {capitalizedDatabaseName}",
                    Description = "API documentation for _**IC BikeTrainer**_ service. " +
                                  "**Limited to 100 requests** per minute per IP.",
                    License = new OpenApiLicense
                    {
                        Name = "MIT License",
                        Url = new Uri("https://opensource.org/licenses/MIT")
                    }
                });
                
                // JWT Token
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "Paste your JWT token below. Don't include the 'Bearer' prefix — it's added automatically.",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT"
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
                
                // Swagger - Formatting of Controller Names
                options.DocInclusionPredicate((_, apiDesc) =>
                {
                    var controllerName = apiDesc.ActionDescriptor.RouteValues["controller"];

                    switch (controllerName)
                    {
                        case "GodMode":
                            apiDesc.ActionDescriptor.RouteValues["controller"] = "God Mode Controller";
                            apiDesc.GroupName = "godMode";
                            break;
                        
                        case "User":
                            apiDesc.ActionDescriptor.RouteValues["controller"] = "User Controller";
                            apiDesc.GroupName = "user";
                            break;
                    }

                    return true;
                });
            });
        }
    }
}