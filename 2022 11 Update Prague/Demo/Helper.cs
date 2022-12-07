namespace Demo;

internal static class Helper
{
    public static void DemoReport(this object obj) =>
        new[] { obj }.DemoReport();

    public static void DemoReport<T>(this IEnumerable<T> objects)
    {
        Console.Clear();
        foreach (T obj in objects)
            Console.WriteLine(obj);
        Console.WriteLine();
    }

}
