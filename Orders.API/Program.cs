using Orders.API.Database;
using Orders.API.Dto;
using Orders.API.Models;
using Orders.API.Repositories;
using Orders.API.Services;
using Orders.API.Services.Helpers;

var builder = WebApplication.CreateBuilder(args);
builder.Services.Configure<ConnectionStringOptions>(builder.Configuration.GetSection("ConnectionStrings"));
DbHelper.Initialize(builder.Configuration);

builder.Services.AddScoped<IOrderRepository, OrderRepository>();

builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IOrderUpdateRepository, OrderUpdateRepository>();
builder.Services.AddScoped<IRequestApiHelper, RequestApiHelper>();

builder.Services.AddHttpClient();
builder.Services.AddControllers();


var app = builder.Build();

app.MapControllers();

app.Run();
