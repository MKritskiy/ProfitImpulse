using Profiles.API.Repositories;
using Profiles.API.Services;
using Helpers.Database;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<ConnectionStringOptions>(builder.Configuration.GetSection("ConnectionStrings"));
DbHelper.Initialize(builder.Configuration);

builder.Services.AddScoped<IProfileRepository, ProfileRepository>();
builder.Services.AddScoped<IProfileService, ProfileService>();
builder.Services.AddControllers();

var app = builder.Build();

app.MapControllers();

app.Run();
