using Sphagnum.Client;
using Sphagnum.Common.Contracts.Login;
using Sphagnum.Common.Contracts.Messaging;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton(new ConnectionFactory()
{
    UserRights = UserRights.All,
    Login = "root",
    Password = "root",
    Hostname = "test_server",
    Port = 8081,
});
builder.Services.AddSingleton<IMessagingClient, ClientDefault>();
var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();

app.Run();
