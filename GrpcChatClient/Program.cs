using Chat;
using Grpc.Core;
using Grpc.Net.Client;

System.Console.WriteLine("請輸入使用者名稱:");
string userName = Console.ReadLine() ?? "Anonymous";

// 建立 gRPC 通道
// 注意：這裡的 URL 需要與 gRPC 服務端一致
var channel = GrpcChannel.ForAddress("https://localhost:7073");
var client = new ChatService.ChatServiceClient(channel);

using var chat = client.Chat();

// 啟動背景 Task 來接收訊息
var receiveTask = Task.Run(async () =>
{
    try
    {
        await foreach (var resp in chat.ResponseStream.ReadAllAsync())
        {
            Console.WriteLine($"[{resp.User}]：{resp.Message}");
        }
        // 加入判斷 斷線
        Console.WriteLine("【系統訊息】伺服器關閉了連線。");
    }
    catch (RpcException ex)
    {
        if (ex.StatusCode == StatusCode.Cancelled)
        {
            Console.WriteLine("【系統訊息】連線已取消。");
        }
        else
        {
            Console.WriteLine($"【系統訊息】連線中斷！錯誤：{ex.Status.Detail}");
        }
        Environment.Exit(0); // 結束程式
    }
    catch (Exception ex)
    {
        Console.WriteLine($"【系統訊息】遇到異常：{ex.Message}");
    }
});
// 先送一個初始訊息給 Server，註冊暱稱（重要！）

await chat.RequestStream.WriteAsync(new ChatMessage
{
    User = userName,
    Message = "進入聊天室",
    Timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds()
});

// 開始正常輸入聊天訊息
while (true)
{
    try
    {
        var input = Console.ReadLine();
        if (string.IsNullOrEmpty(input)) break;

        await chat.RequestStream.WriteAsync(new ChatMessage
        {
            User = userName,
            Message = input,
            Timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds()
        });
    }
    catch (RpcException ex) when (ex.StatusCode == StatusCode.Cancelled)
    {
        Console.WriteLine("連線已關閉，無法發送訊息。");
        break;
    }
    catch (RpcException ex)
    {
        if (ex.StatusCode == StatusCode.Cancelled)
        {
            Console.WriteLine("連線已關閉（Cancelled）。無法再發送訊息。");
        }
        else if (ex.StatusCode == StatusCode.OK)
        {
            Console.WriteLine("連線正常結束（OK）。");
        }
        else
        {
            Console.WriteLine($"發生錯誤: {ex.Status.Detail}");
        }
        break;
    }
}

await chat.RequestStream.CompleteAsync();
await receiveTask; // 等待接收 Task 完成