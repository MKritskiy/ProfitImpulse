using Microsoft.Extensions.Options;
using Helpers.Database;
using Users.API.Services.General;

var builder = WebApplication.CreateBuilder(args);


var jwtSettings = builder.Configuration.GetSection("JwtSettings").Get<JwtSettings>();
if (jwtSettings == null)
{
    throw new InvalidOperationException("JwtSettings section is missing or not configured correctly.");
}
builder.Services.AddSingleton(jwtSettings);


builder.Services.Configure<ConnectionStringOptions>(builder.Configuration.GetSection("ConnectionStrings"));

builder.Services.AddControllers();
DbHelper.Initialize(builder.Configuration);

builder.Services.AddScoped<Users.API.Repositories.IUserRepository, Users.API.Repositories.UserRepository>();
builder.Services.AddScoped<Users.API.Services.Encrypt.IEncrypt, Users.API.Services.Encrypt.Encrypt>();
builder.Services.AddScoped<Users.API.Services.Token.ITokenGenerator, Users.API.Services.Token.TokenGenerator>();
builder.Services.AddScoped<Users.API.Services.UserService.IUserService, Users.API.Services.UserService.UserService>();



var app = builder.Build();

app.MapControllers();

app.Run();
