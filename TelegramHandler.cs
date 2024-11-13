namespace DeniraParser;
using System.Text;
using Newtonsoft.Json;

public class TelegramHandler (string botToken)
{
    private readonly string _botToken = botToken.Trim();
    public async Task SendMessageAsync(string chatId, string message)
    {
        string apiUrl = $"https://api.telegram.org/bot{_botToken}/sendMessage";
        using HttpClient client = new();
        
        var payload = new
        {
            chat_id = chatId,
            text = message
        };

        var content = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");

        var response = await client.PostAsync(apiUrl, content);
        
        if (!response.IsSuccessStatusCode) throw new Exception($"Ошибка отправки сообщения: {response.RequestMessage}");
    }

    public async Task<string> GetBotUsername()
    {
        string apiUrl = $"https://api.telegram.org/bot{_botToken}/getMe";
        using HttpClient client = new();

        var response = await client.GetAsync(apiUrl);
        
        if (!response.IsSuccessStatusCode) return string.Empty;

        BotInfoResponse? jsonResult = JsonConvert.DeserializeObject<BotInfoResponse>(await response.Content.ReadAsStringAsync());
        return jsonResult?.Result?.UserName ?? string.Empty;
    }

    public async Task<bool> IsValidChatId(string chatId)
    {
        string apiUrl = $"https://api.telegram.org/bot{_botToken}/getChat";
        using HttpClient client = new();

        var payload = new { chat_id = chatId };
        var content = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");

        var response = await client.PostAsync(apiUrl, content);
        
        return response.IsSuccessStatusCode;
    }
}
