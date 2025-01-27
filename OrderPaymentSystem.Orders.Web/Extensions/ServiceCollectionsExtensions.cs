using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using OrderPaymentSystem.Orders.Application.Abstractions;
using OrderPaymentSystem.Orders.Application.Services;
using OrderPaymentSystem.Orders.Domain;

namespace OrderPaymentSystem.Orders.Web.Extensions;

public static class ServiceCollectionsExtensions
{
    public static WebApplicationBuilder AddSwagger(this WebApplicationBuilder builder)
    {
        builder.Services.AddSwaggerGen(option =>
        {
            option.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Orders API",
                Version = "v1"
            });
            option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Please enter a valid token",
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                BearerFormat = "JWT",
                Scheme = "Bearer"
            });
            option.AddSecurityRequirement(new OpenApiSecurityRequirement
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
        });

        return builder;
    }

    public static WebApplicationBuilder AddData(this WebApplicationBuilder builder)
    {
        builder.Services.AddDbContext<OrdersDbContext>(opt =>
            opt.UseNpgsql(builder.Configuration.GetConnectionString("Orders")));

        return builder;
    }


    // Сервисы из application слоя, пока пусто
    public static WebApplicationBuilder AddApplicationServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<ICartsService, CartsService>();
        builder.Services.AddScoped<IOrdersService, OrdersService>();

        return builder;
    }

    // Сервисы из интеграций, пока пусто
    public static WebApplicationBuilder AddIntegrationServices(this WebApplicationBuilder builder)
    {
        return builder;
    }
}