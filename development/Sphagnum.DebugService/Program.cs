using Sphagnum.Common.Infrastructure.Services;
using Sphagnum.Server.Broker.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddSingleton(new ConnectionFactory()
{
    Port = 8081,
});
builder.Services.AddSingleton<MessagesProcessor>();
builder.Services.AddSingleton<ConnectionsManager>();
builder.Services.AddSingleton<ConnectionsManager>();
builder.Services.AddHostedService<ConnectionsReciever>();


var app = builder.Build();
app.MapControllers();

app.Run();
