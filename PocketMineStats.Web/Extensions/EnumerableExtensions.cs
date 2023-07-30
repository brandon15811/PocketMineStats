﻿namespace PocketMineStats.Extensions;

public static class EnumerableExtensions {
    public static IEnumerable<(T item, int index)> WithIndex<T>(this IEnumerable<T> self)
    {
        return self?.Select((item, index) => (item, index)) ?? new List<(T, int)>();
    }
}