namespace Demo;

public static class OptionExtensions
{
    public static Option<T> Optional<T>(this T obj) =>
        new Some<T>(obj);

    public static Option<T> NoneIfNull<T>(this T? obj) =>
        obj is not null ? new Some<T>(obj) : new None<T>();

    public static Option<TResult> Map<T, TResult>(
        this Option<T> optional, Func<T, TResult> map) =>
        optional is Some<T> some ? map(some.Content).Optional()
        : new None<TResult>();

    public static Option<T> Filter<T>(
        this Option<T> optional, Func<T, bool> predicate) =>
        optional is Some<T> some && !predicate(some.Content) ? new None<T>()
        : optional;

    public static T Reduce<T>(
        this Option<T> optional, T @default) =>
        optional is Some<T> some ? some.Content : @default;

    public static T Reduce<T>(
        this Option<T> optional, Func<T> @default) =>
        optional is Some<T> some ? some.Content : @default();
}

