namespace DeniraParser;
using System.Text;
using Newtonsoft.Json;

public class TelegramMessageSender (string botToken)
{
    private readonly string _botToken = botToken;
    private readonly string _apiUrl = $"https://api.telegram.org/bot{botToken}/sendMessage";
    public async Task SendMessageAsync(long chatId, string message)
    {
        using var client = new HttpClient();
        
        var payload = new
        {
            chat_id = chatId,
            text = message
        };

        var content = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");

        var response = await client.PostAsync(_apiUrl, content);
        
        if (!response.IsSuccessStatusCode) throw new Exception($"Ошибка отправки сообщения: {response.RequestMessage}");
    }
}
