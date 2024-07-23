using Inventories.API.Database;
using Inventories.API.Repositories;
using Inventories.API.Services;

var builder = WebApplication.CreateBuilder(args);

DbHelper.Initialize(builder.Configuration);

builder.Services.AddScoped<IStockRepository, StockRepository>();

builder.Services.AddScoped<IStockService, StockService>();
builder.Services.AddScoped<IStockUpdateRepository, StockUpdateRepository>();
builder.Services.AddHttpClient();


var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run();
