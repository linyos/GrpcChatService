// 使用 Serilog 來記錄日誌
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .WriteTo.Console()
    .WriteTo.File("logs/log.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

var builder = WebApplication.CreateBuilder(args);

// 使用 Serilog 作為記錄器
builder.Host.UseSerilog();

// 加入服務到容器
builder.Services.AddGrpc();
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddSingleton<ChatState>();


//伺服器端允許 HTTP/2 明文
builder.WebHost.ConfigureKestrel(options =>
{   
    // HTTP/2 with TLS
    options.ListenLocalhost(5093, o => 
    {
        o.UseHttps(); // 啟用 HTTPS
        o.Protocols = Microsoft.AspNetCore.Server.Kestrel.Core.HttpProtocols.Http2;
    });
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.MapBlazorHub();
app.MapGrpcService<GreeterService>();




// 註解掉這一行，讓 gRPC 的說明消失
// app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

// 使用 Components/Pages 資料夾下的 _Host
app.MapFallbackToPage("/_Host");

app.Run();