using Sphagnum.Client;
using Sphagnum.Common.Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton(new ConnectionFactory()
{
    //UserRights = UserRights.All,
    Login = "root",
    Password = "root",
    Hostname = "test_server",
    Port = 8081,
});
builder.Services.AddSingleton<ClientDefault>();
var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();

app.Run();
