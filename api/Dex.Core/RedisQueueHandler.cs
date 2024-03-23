namespace Dex.Core;

public interface IRedisQueueHandler : IDisposable
{
    string Name { get; }

    Task Setup(CancellationToken token);
}

public abstract class RedisQueueHandler<T>(
    IRedisService redis,
    ILogger logger) : IRedisQueueHandler
{
    private IRedisList<T>? _queue;
    private bool _running = false;

    private readonly List<IDisposable> _subscriptions = [];
    private readonly IRedisService _redis = redis;
    private readonly ILogger _logger = logger;
    private CancellationToken? _token;

    public abstract string Name { get; }
    public virtual string QueueName => $"{Name}:queue";
    public virtual IRedisList<T> Queue => _queue ??= _redis.List<T>(QueueName);
    public virtual bool ShouldCancel => _token?.IsCancellationRequested ?? false;

    public abstract Task On(T item);

    public virtual async Task Setup(CancellationToken token)
    {
        _token = token;
        await Observe(QueueName, TriggerQueue);

        _ = Task.Run(TriggerQueue, token);
    }

    public virtual async Task TriggerQueue()
    {
        if (_running || ShouldCancel) return;

        _running = true;
        try
        {
            _logger.LogInformation("[QUEUE: {name}] - Starting dequeue", Name);
            var anyU = await HandleQueue(Queue);
            _logger.LogInformation("[QUEUE: {name}] - Finished update", Name);

            if (anyU)
            {
                _running = false;
                await TriggerQueue();
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "[QUEUE: {name}] - Error occurred while handling queue", Name);
        }
        finally
        {
            _running = false;
        }
    }

    public virtual async Task<bool> HandleQueue(IRedisList<T> queue)
    {
        var any = false;

        while (true)
        {
            if (ShouldCancel) break;

            var item = await queue.Pop();
            if (item == null || ShouldCancel) break;

            await On(item);
            any = true;
        }

        return any;
    }

    public virtual async Task Observe(string name, Func<Task> on)
    {
        var obs = await _redis.Observe(name);
        _subscriptions.Add(obs.Subscribe(t => on()));
    }

    public virtual void Dispose()
    {
        foreach (var sub in _subscriptions.ToArray())
        {
            sub.Dispose();
            _subscriptions.Remove(sub);
        }
    }
}
