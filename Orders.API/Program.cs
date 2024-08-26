using Helpers.Database;
using Orders.API.Repositories;
using Orders.API.Services;
using Helpers;

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
