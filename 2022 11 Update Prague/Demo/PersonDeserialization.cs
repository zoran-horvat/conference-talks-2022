namespace Demo;

public static class PersonDeserialization
{
    public static IEnumerable<Person> ToPersons(
        this IEnumerable<InputPerson?>? rows) =>
        (rows ?? Enumerable.Empty<InputPerson?>()).SelectOptional(
            row => Person.TryCreate(row?.FirstName, row?.LastName));

    public static IEnumerable<ActualPerson> ToActualPersons(
        this IEnumerable<InputPerson?>? rows) =>
        rows.ToPersons().Cast<ActualPerson>();
}

