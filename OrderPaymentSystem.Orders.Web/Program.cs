using OrderPaymentSystem.Orders.Web.Extensions;
using OrderPaymentSystem.Orders.Web.Middlewares;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder
    .AddBearerAuthentication()
    .AddOptions()
    .AddSwagger()
    .AddData()
    .AddApplicationServices()
    .AddIntegrationServices()
    .AddBackgroundServices()
    .AddProblemDetailsConfiguration();

var app = builder.Build();

app.UseMiddleware<GlobalExceptionMiddleware>();

app.UseAuthentication();
app.UseAuthorization();
app.UseSwagger();
app.UseSwaggerUI();
app.MapControllers();

app.Run();