using Microsoft.OpenApi.Models;
using Microsoft.Net.Http.Headers;
using Swashbuckle.AspNetCore.Filters;
using MYSD_REG.Domain.Interfaces;
using MYSD_REG.Domain.Services;

namespace MYSD_REG.Api.Extentions
{
    public static class ServicesConfigurationExtensions
    {

        public static void ConfigureDepencyInjections(this IServiceCollection services)
        {

            services.AddScoped<IUserService, UserService>();
        }

            public static void ConfigureSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(s =>
            {
                s.ExampleFilters();
                s.EnableAnnotations();

                s.SwaggerDoc("v1", new OpenApiInfo { Title = "MYSD - REG Web service API", Version = "v1" });

                s.AddSecurityDefinition("basic", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "basic",
                    In = ParameterLocation.Header,
                    Description = "Basic Authorization header using the Bearer scheme."
                });

                s.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                          new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "basic"
                                }
                            },
                            new string[] {}
                    }
                });

            });
        }
    }
}
