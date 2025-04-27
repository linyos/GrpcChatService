using Grpc.Core;
using GrpcChatService;
using Chat;

namespace GrpcChatService.Services
{
    public class GreeterService : ChatService.ChatServiceBase
    {
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
            //
            await foreach (var msg in requestStream.ReadAllAsync())
            {
                Console.WriteLine($"{msg.User} 說: {msg.Message}");
                await responseStream.WriteAsync(msg);
            }
        }
    }
}