namespace Demo;

public static class OptionalLinq
{
    public static Option<T> FirstOrNone<T>(
        this IEnumerable<T> seq)
    {
        using IEnumerator<T> enumerator = seq.GetEnumerator();
        if (!enumerator.MoveNext()) return None<T>.Value;
        return enumerator.Current.Optional();
    }

    public static IEnumerable<TResult> SelectOptional<T, TResult>(
        this IEnumerable<T> seq, Func<T, Option<TResult>> map)
    {
        foreach (T item in seq)
        {
            if (map(item) is Some<TResult> some)
                yield return some.Content;
        }
    }
}

