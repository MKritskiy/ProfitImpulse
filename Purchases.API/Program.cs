using Purchases.API.Database;
using Purchases.API.Repositories;
using Purchases.API.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.Configure<ConnectionStringOptions>(builder.Configuration.GetSection("ConnectionStrings"));
DbHelper.Initialize(builder.Configuration);

builder.Services.AddScoped<IPurchaseRepository, PurchaseRepository>();

builder.Services.AddScoped<IPurchaseService, PurchaseService>();
builder.Services.AddScoped<IPurchaseUpdateRepository, PurchaseUpdateRepository>();
builder.Services.AddHttpClient();
builder.Services.AddControllers();


var app = builder.Build();

app.MapControllers();

app.Run();