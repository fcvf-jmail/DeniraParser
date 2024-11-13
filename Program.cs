using DeniraParser;
using dotenv.net;

DotEnv.Load(new DotEnvOptions(envFilePaths: [Path.Combine(Directory.GetCurrentDirectory(), ".env")]));
var telegramSender = new TelegramMessageSender(DotEnv.Read()["botToken"]);

Func<Task> parseDenira = async() => 
{
    DateOnly nearestParsedDate = await Parser.ParseNearestDate();
    string content = LastNearestDateTxtHandler.Read().Length < 10 ? DateOnly.MaxValue.ToString("yyyy-MM-dd") : LastNearestDateTxtHandler.Read();
    DateOnly lastNearestDate = DateOnly.ParseExact(content, "yyyy-MM-dd");

    if (lastNearestDate > nearestParsedDate)
    {
        await telegramSender.SendMessageAsync(chatId: 1386450473, message: $"Появилась свободная запись на {nearestParsedDate}");
        LastNearestDateTxtHandler.Rewrite(nearestParsedDate.ToString("yyyy-MM-dd"));
    }
};

// Создаем сервис с переданным кодом
var recurringTaskService = new RecurringTaskService(
    taskToExecute: async () => await parseDenira(),
    interval: TimeSpan.FromSeconds(60)
);

var cancellationTokenSource = new CancellationTokenSource();
await recurringTaskService.StartAsync(cancellationTokenSource.Token);

