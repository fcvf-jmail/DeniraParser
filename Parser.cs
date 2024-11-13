namespace DeniraParser;
using System.Text.Json;
using dotenv.net;


public class Parser
{

    public static async Task<DateOnly> ParseNearestDate()
    {
        DotEnv.Load(new DotEnvOptions(envFilePaths: [Path.Combine(Directory.GetCurrentDirectory(), ".env")]));
        var telegramSender = new TelegramMessageSender(DotEnv.Read()["botToken"]);

        string todaysDate = DateTime.Today.ToString("yyyy-MM-dd");

        JsonElement res = await Parser.ParseUrl($"https://n1101645.yclients.com/api/v1/book_dates/1017425?staff_id=3144539&date=&date_from={todaysDate}&date_to=9999-01-01&service_ids%5B%5D=16246136");
        JsonElement bookingDates = res.GetProperty("booking_dates");

        string[] dateArr = bookingDates[0].ToString().Split("-");

        DateOnly nearestDate = new(int.Parse(dateArr[0]), int.Parse(dateArr[1]), int.Parse(dateArr[2]));
        return nearestDate;
    }
    public static async Task<JsonElement> ParseUrl(string url, string auth = "Bearer gtcwf654agufy25gsadh")
    {
        HttpClient httpClient = new();
        using HttpRequestMessage request = new()
        {
            RequestUri = new Uri(url),
            Method = HttpMethod.Get
        };
        request.Headers.Add("Authorization", auth);
        using HttpResponseMessage response = await httpClient.SendAsync(request);
        JsonDocument doc = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
        return doc.RootElement;
    }
}