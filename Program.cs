using DeniraParser;
using dotenv.net;

string dotenvPath = Path.Combine(Directory.GetCurrentDirectory(), ".env");
DotEnv.Load(new DotEnvOptions(envFilePaths: [dotenvPath]));

(TelegramHandler telegramHandler, string botUsername) = await ConfigManager.ConfigureBotToken();

string chatId = await ConfigManager.ConfigureChatId(telegramHandler, botUsername);

Func<Task> parseDenira = async() => 
{
    DateOnly nearestParsedDate = await Parser.ParseNearestDate();
    string content = LastNearestDateTxtHandler.Read().Length < 10 ? DateOnly.MaxValue.ToString("yyyy-MM-dd") : LastNearestDateTxtHandler.Read();
    DateOnly lastNearestDate = DateOnly.ParseExact(content, "yyyy-MM-dd");

    if (lastNearestDate > nearestParsedDate)
    {
        var nearestParsedDateStr = nearestParsedDate.ToString("dd.MM.yyyy");
        await telegramHandler.SendMessageAsync(chatId: "1386450473", message: $"Появилась свободная запись на {nearestParsedDateStr}");
        LastNearestDateTxtHandler.Rewrite(nearestParsedDate.ToString("yyyy-MM-dd"));
    }
};

var recurringTaskService = new RecurringTaskService(
    taskToExecute: async () => await parseDenira(),
    interval: TimeSpan.FromSeconds(60)
);

var cancellationTokenSource = new CancellationTokenSource();
await recurringTaskService.StartAsync(cancellationTokenSource.Token);