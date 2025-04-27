using GrpcChatService.Services;
using Serilog;

// �s�WLOG
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .WriteTo.Console()
    .WriteTo.File("logs/log.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

var builder = WebApplication.CreateBuilder(args);
// �ϥ� Serilog ���N���� Logger

builder.Host.UseSerilog();

// Add services to the container.
builder.Services.AddGrpc();

var app = builder.Build();

// �M�g ChatService ���I
app.MapGrpcService<GreeterService>();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();