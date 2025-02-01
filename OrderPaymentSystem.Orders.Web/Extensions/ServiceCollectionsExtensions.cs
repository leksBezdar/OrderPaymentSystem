using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using OrderPaymentSystem.Orders.Application.Abstractions;
using OrderPaymentSystem.Orders.Application.Services;
using OrderPaymentSystem.Orders.Domain;
using OrderPaymentSystem.Orders.Domain.Models;
using OrderPaymentSystem.Orders.Domain.Options;
using OrderPaymentSystem.Orders.Web.BackgroundServices;
using System.Text;

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

    // Сервисы из application слоя
    public static WebApplicationBuilder AddApplicationServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<ICartsService, CartsService>();
        builder.Services.AddScoped<IOrdersService, OrdersService>();
        builder.Services.AddScoped<IMerchantService, MerchantService>();

        return builder;
    }

    // Сервисы из интеграций, пока пусто
    public static WebApplicationBuilder AddIntegrationServices(this WebApplicationBuilder builder)
    {
        return builder;
    }

    public static WebApplicationBuilder AddBackgroundServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddHostedService<CreateOrderConsumer>();
        return builder;
    }

    public static WebApplicationBuilder AddBearerAuthentication(this WebApplicationBuilder builder)
    {
        builder.Services
            .AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.UseSecurityTokenValidators = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(
                        builder.Configuration["Authentication:TokenPrivateKey"]!)),
                    ValidIssuer = "test",
                    ValidAudience = "test",
                    // ValidateIssuer = true,
                    // ValidateAudience = true,
                    // ValidateLifetime = true,
                    // ValidateIssuerSigningKey = true
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = false,
                    ValidateIssuerSigningKey = false
                };
            });
        builder.Services.AddAuthorizationBuilder()
            .AddPolicy("Admin", policy => policy.RequireRole(RolesEnum.Admin.Value))
            .AddPolicy("Merchant", policy => policy.RequireRole(RolesEnum.Merchant.Value))
            .AddPolicy("User", policy => policy.RequireRole(RolesEnum.User.Value));

        builder.Services.AddTransient<IAuthService, AuthService>();
        builder.Services.AddDefaultIdentity<UserEntity>(options =>
        {
            options.SignIn.RequireConfirmedAccount = false;
            options.Password.RequiredLength = 6;
            options.Password.RequireNonAlphanumeric = false;
        })
            .AddEntityFrameworkStores<OrdersDbContext>()
            .AddUserManager<UserManager<UserEntity>>()
            .AddUserStore<UserStore<UserEntity, Domain.Aggregates.User.IdentityRole, OrdersDbContext, long>>();

        return builder;
    }

    public static WebApplicationBuilder AddOptions(this WebApplicationBuilder builder)
    {
        builder.Services.Configure<AuthOptions>(builder.Configuration.GetSection("Authentication"));
        builder.Services.Configure<RabbitMqOptions>(builder.Configuration.GetSection("RabbitMQ"));

        return builder;
    }

    public static WebApplicationBuilder AddProblemDetailsConfiguration(this WebApplicationBuilder builder)
    {
        builder.Services.AddProblemDetails(options =>
        {
            options.CustomizeProblemDetails = ctx =>
            {
                ctx.ProblemDetails.Extensions.Add("instance", ctx.HttpContext.Request.Path);
                ctx.ProblemDetails.Extensions.Add("traceId", ctx.HttpContext.TraceIdentifier);
            };
        });

        return builder;
    }
}