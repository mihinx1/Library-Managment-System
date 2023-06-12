using System.Collections.Concurrent;

namespace IPSTemplate.AppServer.Helpers;

public static class ConcurrencyExtensions
{
    public static void AddRange<T>(this ConcurrentBag<T> @this, IEnumerable<T> toAdd)
    {
        foreach (var element in toAdd)
            @this.Add(element);
    }
}
