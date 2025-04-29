
public class ChatState
{
    // 儲存所有在線的 ResponseStream
    
    
    public ConcurrentDictionary<string, IServerStreamWriter<ChatMessage>> Clients { get; }
      = new ConcurrentDictionary<string, IServerStreamWriter<ChatMessage>>();

    public event Action? OnChange;
    public void NotifyStateChanged() => OnChange?.Invoke();
}