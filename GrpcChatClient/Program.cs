using Chat;
using Grpc.Core;
using Grpc.Net.Client;

var channel = GrpcChannel.ForAddress("https://localhost:7073");
var client = new ChatService.ChatServiceClient(channel);

using var chat = client.Chat();

// 啟動背景 Task 來接收訊息
var receiveTask = Task.Run(async () =>
{
    while (await chat.ResponseStream.MoveNext())
    {
        var message = chat.ResponseStream.Current;
        Console.WriteLine($"[收到] {message.User} : {message.Message}");
    }
});

// 讀取使用者輸入並送出
while (true)
{
    var input = Console.ReadLine();
    if (string.IsNullOrEmpty(input)) break;

    await chat.RequestStream.WriteAsync(new ChatMessage
    {
        User = "User",
        Message = input,
        Timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds()
    });
}

await chat.RequestStream.CompleteAsync();
await receiveTask; // 等待接收 Task 完成