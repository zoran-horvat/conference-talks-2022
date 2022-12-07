namespace Demo;

public abstract record Person()
{
    public static Option<Person> TryCreate(string? firstName, string? lastName) =>
        lastName is null ? Create(firstName).Optional()
        : firstName is null ? None<Person>.Value
        : new CommonPerson(firstName, lastName).Optional<Person>();

    public static Person Create(string? name) =>
        name is null ? Create() : new MononymousPerson(name);

    public static Person Create() => new UnknownPerson();
}

public abstract record ActualPerson() : Person
{
    public abstract string FullName { get; }
}

public record CommonPerson(string FirstName, string LastName) : ActualPerson
{
    public override string FullName => $"{FirstName} {LastName}";
}

public record MononymousPerson(string Name) : ActualPerson
{
    public override string FullName => Name;
}

public record UnknownPerson() : Person
{
    public static UnknownPerson Instance { get; } = new();
}
