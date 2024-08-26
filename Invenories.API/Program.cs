using Helpers;
using Helpers.Database;
using Inventories.API.Repositories;
using Inventories.API.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.Configure<ConnectionStringOptions>(builder.Configuration.GetSection("ConnectionStrings"));

DbHelper.Initialize(builder.Configuration);

builder.Services.AddScoped<IStockRepository, StockRepository>();

builder.Services.AddScoped<IStockService, StockService>();
builder.Services.AddScoped<IStockUpdateRepository, StockUpdateRepository>();
builder.Services.AddScoped<IRequestApiHelper, RequestApiHelper>();



builder.Services.AddHttpClient();
builder.Services.AddControllers();


var app = builder.Build();

app.MapControllers();

app.Run();
