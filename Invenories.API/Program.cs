using Inventories.API.Database;
using Inventories.API.Repositories;
using Inventories.API.Services;
using Users.API.Database;

var builder = WebApplication.CreateBuilder(args);
builder.Services.Configure<ConnectionStringOptions>(builder.Configuration.GetSection("ConnectionStrings"));

DbHelper.Initialize(builder.Configuration);

builder.Services.AddScoped<IStockRepository, StockRepository>();

builder.Services.AddScoped<IStockService, StockService>();
builder.Services.AddScoped<IStockUpdateRepository, StockUpdateRepository>();
builder.Services.AddHttpClient();
builder.Services.AddControllers();


var app = builder.Build();

app.MapGet("/", () => "Hello World!");
app.MapControllers();

app.Run();
