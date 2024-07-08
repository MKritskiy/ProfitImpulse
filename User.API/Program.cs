using Users.API.Database;
using Users.API.Services.General;

var builder = WebApplication.CreateBuilder(args);


builder.Configuration.AddJsonFile("appsettings.json");



builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));
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
