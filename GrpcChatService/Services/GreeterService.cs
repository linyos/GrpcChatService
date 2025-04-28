
namespace GrpcChatService.Services
{
    public class GreeterService : ChatService.ChatServiceBase
    {

        // 儲存所有在線的訊息
        private static readonly ConcurrentDictionary<string, IServerStreamWriter<ChatMessage>> _clients
        = new ConcurrentDictionary<string, IServerStreamWriter<ChatMessage>>();



        private readonly ILogger<GreeterService> _logger;

        public GreeterService(ILogger<GreeterService> logger)
        {
            _logger = logger;
        }

        // 修正：方法簽章需與 proto 定義一致
        public override async Task Chat(
            IAsyncStreamReader<ChatMessage> requestStream,
            IServerStreamWriter<ChatMessage> responseStream,
            ServerCallContext context)
        {

            string user = string.Empty;
            // 註冊這個Client
            try
            {
                // 這裡先等第一個訊息，用來取得 Client的User名稱
            if (await requestStream.MoveNext())
            {
                var firstMessage = requestStream.Current;
                user = firstMessage.User;

                // 註冊這個Client 
                _clients.TryAdd(user, responseStream);

                 // 廣播進入聊天室的訊息
                 await BroadcastMessage($"{user} 進入聊天室");
                 try
                 {
                     do
                     {
                        var msg = requestStream.Current;
                         Console.WriteLine($"{msg.User} 說: {msg.Message}");
                         // 廣播訊息給所有在線的客戶端
                         await BroadcastMessage($"{msg.User} 說: {msg.Message}");
                         // 等待下一個訊息
                     } while (await requestStream.MoveNext());
                 }
                 finally
                 {
                     // 廣播離開聊天室的訊息
                     _clients.TryRemove(user , out _);
                     await BroadcastMessage($"{user} 離開聊天室");
                     
                 }
            }
            }
            catch (IOException ex)
            {
                _logger.LogError(ex, "IO異常：{Message}", ex.Message);
                 // 處理 IO 異常，例如客戶端斷線
                 //Console.WriteLine($"【Server警告】讀取訊息時連線中斷（IO例外）：{ex.Message}");
                // 處理 IO 異常，例如客戶端斷線
                // Console.WriteLine($"【Server警告】讀取訊息時連線中斷（IO例外）：{ex.Message}");
            }
            catch (RpcException ex)
            {
                // 處理取消的請求
                if (ex.StatusCode == StatusCode.Cancelled)
                {
                    Console.WriteLine("【Server】客戶端主動中止連線。");
                }
                else
                {
                    Console.WriteLine($"【Server】gRPC錯誤：{ex.StatusCode} - {ex.Status.Detail}");
                }
            }
            finally
            {
                // 確保在結束時移除客戶端
                if (!string.IsNullOrEmpty(user))
                {
                    _clients.TryRemove(user, out _);
                    await BroadcastMessage($"{user} 離開聊天室");
                }
            }
            



            
        }



        /// <summary>
        /// 廣播訊息給所有在線的客戶端
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns> <summary>
        private async Task BroadcastMessage(string message)
        {
            var task = _clients.Values.Select(Stream => Stream.WriteAsync(new ChatMessage
            {
                User = "Server",
                Message = message,
                Timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds()
            }));
            await Task.WhenAll(task);
        }
    }
}