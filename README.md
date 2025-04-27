# GrpcChatService

## 專案簡介
GrpcChatService 是一個基於 gRPC 的聊天應用程式範例，展示如何使用 gRPC 技術進行高效的雙向通訊。專案包含以下三個主要部分：

1. **ChatSolution.Protos**: 定義 gRPC 的 proto 檔案，包含服務與訊息的定義。
2. **GrpcChatService**: gRPC 服務端，負責處理客戶端的請求並回應。
3. **GrpcChatClient**: gRPC 客戶端，模擬用戶端與服務端進行通訊。

## 專案結構
```
GrpcChatService.sln
ChatSolution.Protos/
    chat.proto
    ChatSolution.Protos.csproj
GrpcChatClient/
    GrpcChatClient.csproj
    Program.cs
GrpcChatService/
    GrpcChatService.csproj
    Program.cs
    Protos/
        greet.proto
    Services/
        GreeterService.cs
```

## 使用方式

### 1. 啟動服務端
1. 確保已安裝 .NET SDK。
2. 在終端機中進入 `GrpcChatService` 資料夾。
3. 執行以下命令啟動服務：
   ```bash
   dotnet run
   ```

### 2. 啟動客戶端
1. 在終端機中進入 `GrpcChatClient` 資料夾。
2. 執行以下命令啟動客戶端：
   ```bash
   dotnet run
   ```

## 注意事項
- 確保 `launchSettings.json` 中的服務端位址與客戶端設定一致。
- 若遇到連線問題，請檢查防火牆或服務端是否正確啟動。

## 技術細節
- **gRPC**: 用於高效能的 RPC 通訊。
- **.NET 6+**: 使用最新的 .NET 平台進行開發。
- **ProtoBuf**: 定義 gRPC 的訊息格式。

## 聯絡方式
如有任何問題，請聯絡專案開發者。
