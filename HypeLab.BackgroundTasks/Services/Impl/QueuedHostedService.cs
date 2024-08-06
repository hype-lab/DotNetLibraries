using HypeLab.BackgroundTasks.BT;
using HypeLab.BackgroundTasks.Data.Exceptions;
using Microsoft.Extensions.Hosting;

namespace HypeLab.BackgroundTasks.Services.Impl
{
    public class QueuedHostedService : BackgroundService, IQueuedHostedService
    {
        private readonly IBackgroundTaskQueue _taskQueue;

        public QueuedHostedService(IBackgroundTaskQueue taskQueue)
        {
            _taskQueue = taskQueue;
        }

        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await BackgroundProcessing(stoppingToken).ConfigureAwait(false);
        }

        private async Task BackgroundProcessing(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                Func<CancellationToken, Task>? workItem = await _taskQueue.DequeueAsync(stoppingToken).ConfigureAwait(false);

                if (workItem != null)
                {
                    try
                    {
                        await workItem(stoppingToken);
                    }
                    catch (Exception ex)
                    {
                        // log error
                        throw new BackgroundProcessingErrorException($"Error occurred during background processing.{ex.Message}", ex);
                    }
                }
            }
        }
    }
}
