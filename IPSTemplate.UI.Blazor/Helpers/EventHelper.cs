namespace IPSTemplate.UI.Blazor.Helpers;

public static class EventHelper
{
    private static void Cancel(CancellationTokenSource? cts)
    {
        cts?.Cancel();
        try
        {
            cts?.Dispose();
        }
        catch
        { }
    }

    public static Action Debounce(Action action, int milliseconds = 300)
    {
        CancellationTokenSource? lastCts = null;
        return () =>
        {
            Cancel(lastCts);
            var cts = lastCts = new();
            Task.Delay(milliseconds).ContinueWith(_ => action(), cts.Token);
        };
    }

    public static Action<T> Debounce<T>(Action<T> action, int milliseconds = 300)
    {
        CancellationTokenSource? lastCts = null;
        return arg =>
        {
            Cancel(lastCts);
            var cts = lastCts = new();
            Task.Delay(milliseconds).ContinueWith(_ => action(arg), cts.Token);
        };
    }

    public static Func<Task<TResult>> Debounce<TResult>(Func<TResult> function, int milliseconds = 300)
    {
        CancellationTokenSource? lastCts = null;
        return () =>
        {
            Cancel(lastCts);
            var cts = lastCts = new();
            return Task.Delay(milliseconds).ContinueWith(_ => function(), cts.Token);
        };
    }

    public static Func<T, Task<TResult>> Debounce<T, TResult>(Func<T, TResult> function, int milliseconds = 300)
    {
        CancellationTokenSource? lastCts = null;
        return arg =>
        {
            Cancel(lastCts);
            var cts = lastCts = new();
            return Task.Delay(milliseconds).ContinueWith(_ => function(arg), cts.Token);
        };
    }
}
