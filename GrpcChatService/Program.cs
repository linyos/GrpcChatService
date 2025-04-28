
// �s�WLOG
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .WriteTo.Console()
    .WriteTo.File("logs/log.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

var builder = WebApplication.CreateBuilder(args);


//  Serilog  Logger
builder.Host.UseSerilog();

// Add services to the container.
builder.Services.AddGrpc();
builder.Services.AddRazorPages(); 
builder.Services.AddServerSideBlazor();
builder.Services.AddSingleton<ChatState>();


var app = builder.Build();





app.MapGrpcService<GreeterService>();
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();