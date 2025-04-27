using GrpcChatService.Services;
using Serilog;

// 新增LOG
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .WriteTo.Console()
    .WriteTo.File("logs/log.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

var builder = WebApplication.CreateBuilder(args);
// 使用 Serilog 取代內建 Logger

builder.Host.UseSerilog();

// Add services to the container.
builder.Services.AddGrpc();

var app = builder.Build();

// 映射 ChatService 端點
app.MapGrpcService<GreeterService>();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();