namespace DeniraParser;
using Microsoft.Extensions.Hosting;

public class RecurringTaskService (Func<Task> taskToExecute, TimeSpan interval)
{
    private readonly Func<Task> _taskToExecute = taskToExecute;
    private readonly TimeSpan _interval = interval;
    
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            try
            {
                await _taskToExecute();
                Console.WriteLine($"Исполнил задачу {DateTime.Now}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при выполнении задачи: {ex.Message}");
            }
            finally
            {
                await Task.Delay(_interval, cancellationToken);
            }
        }
    }
}
