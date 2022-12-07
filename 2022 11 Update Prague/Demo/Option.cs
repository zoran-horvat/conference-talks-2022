public abstract class Option<T> { }

public class Some<T> : Option<T>
{
    public T Content { get; }
    public Some(T content) => Content = content;

    public override string ToString() =>
        $"Some {Content?.ToString() ?? "<null>"}";
}

public class None<T> : Option<T>
{
    public static Option<T> Value { get; } = new None<T>();
    public override string ToString() => "None";
}
