using dotenv.net;

namespace DeniraParser;

static class ConfigManager
{

    public static async Task<(TelegramHandler, string)> ConfigureBotToken()
    {
        string dotenvPath = Path.Combine(Directory.GetCurrentDirectory(), ".env");
        DotEnv.Load(new DotEnvOptions(envFilePaths: [dotenvPath]));

        TelegramHandler telegramHandler = new(string.Empty);
        string botUsername = string.Empty;

        try 
        {
            string botToken = DotEnv.Read()["botToken"];
            telegramHandler = new (botToken);
            botUsername = await telegramHandler.GetBotUsername();
        }
        catch {}

        while (botUsername == string.Empty)
        {
            Console.WriteLine("Please enter valid bot token");
            string botToken = Console.ReadLine() ?? "";

            telegramHandler = new(botToken);
            botUsername = await telegramHandler.GetBotUsername();
            if (botUsername != string.Empty) File.WriteAllText(dotenvPath, $"botToken={botToken}");
        }

        return (telegramHandler, botUsername);
    }
    public async static Task<string> ConfigureChatId(TelegramHandler telegramHandler, string botUsername)
    {
        string chatIdFilePath = Path.Combine(Directory.GetCurrentDirectory(), "chatId.txt");
        string chatId = string.Empty;
        if(File.Exists(chatIdFilePath)) chatId = File.ReadAllText(chatIdFilePath);
        bool isValidChatId = await telegramHandler.IsValidChatId(chatId);

        while (isValidChatId == false)
        {
            Console.WriteLine($"Please enter numeric value of chat id\nYou should have at least one message with your bot (@{botUsername})");
            if (double.TryParse(Console.ReadLine(), out double numericChatId)) chatId = numericChatId.ToString();
            isValidChatId = await telegramHandler.IsValidChatId(chatId);
        }

        File.WriteAllText(chatIdFilePath, chatId);

        return chatId;
    }
}