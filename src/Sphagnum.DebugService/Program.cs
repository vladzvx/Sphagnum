using Sphagnum.Common.Contracts.Login;
using Sphagnum.Server;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddSingleton<ConnectionFactory>();
builder.Services.AddHostedService<BrokerHost>();

var app = builder.Build();
app.MapControllers();

app.Run();
