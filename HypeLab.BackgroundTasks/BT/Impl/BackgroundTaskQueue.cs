using System.Threading.Channels;

namespace HypeLab.BackgroundTasks.BT.Impl
{
    public class BackgroundTaskQueue(int capacity) : IBackgroundTaskQueue
    {
        private readonly Channel<Func<CancellationToken, Task>> _queue = Channel.CreateBounded<Func<CancellationToken, Task>>(capacity);

        public async Task QueueBackgroundWorkItemAsync(Func<CancellationToken, Task> workItem)
        {
            ArgumentNullException.ThrowIfNull(workItem);

            await _queue.Writer.WriteAsync(workItem).ConfigureAwait(false);
        }

        public async Task<Func<CancellationToken, Task>?> DequeueAsync(CancellationToken cancellationToken)
        {
            return await _queue.Reader.ReadAsync(cancellationToken).ConfigureAwait(false);
        }
    }
}
