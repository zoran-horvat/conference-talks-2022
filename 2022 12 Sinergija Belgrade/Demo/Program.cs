                                                                                                                                                                                                        using System.Collections.Immutable;
                                                                                                                                                                                                        Console.Clear();


var data = new Part[]
{
    new Part(Guid.NewGuid(), new("BC547"), "BC547", "NPN transistor complement to BC556"),
    new Part(Guid.NewGuid(), new("1K"), "1K", "Resistor 1 kilo-ohm"),
    new Part(Guid.NewGuid(), new("470uF"), "470uF", "Capacitor 470uF, 25V")
};

foreach (Part part in data)
{
    var (_, _, name, description) = part;
    Console.WriteLine($"{name,10} - {description}");
}

var dataWithDuplicates = data
    .Concat(data.Skip(1))
    .Concat(data.Skip(2))
    .ToList();

Console.WriteLine();
foreach (var part in dataWithDuplicates.Distinct())
{
    Console.WriteLine($"{part.Name,10} - {part.Description}");
}

Dictionary<Part, int> partCounts = dataWithDuplicates
    .GroupBy(part => part)
    .ToDictionary(
        group => group.Key,
        group => group.Count());

Console.WriteLine();
foreach (var partCount in partCounts)
{
    Console.WriteLine($"{partCount.Key.Name,10} x {partCount.Value}");
}

var inventory = new InventoryItem[]
{
    new Part(Guid.NewGuid(), new("BC547"), "BC547", "NPN transistor complement to BC556"),
    new Part(Guid.NewGuid(), new("1K"), "1K", "Resistor 1 kilo-ohm"),
    new ExpirableMaterial(Guid.NewGuid(), new("Paste"), "Soldering paste", DateOnly.FromDateTime(DateTime.Today.AddYears(2))),
    new Part(Guid.NewGuid(), new("470uF"), "470uF", "Capacitor 470uF, 25V"),
    new Material(Guid.NewGuid(), new("Wire"), "Soldering wire"),
};

inventory.Print();

AssemblyInstructions asm1 = new()
{
    Title = "Moon station",
    Description = "Assembly instruction for the permanent settlement on the Moon"
};

AssemblyInstructions asm2 = new(
    title: "Earth habitat",
    description: "Assembly instruction for the permanent human settlement");

Part x = new(Guid.NewGuid(), new("thing"), "A thing", "");
Part y = x with { Description = "Don't omit this..." };

AssemblyInstructions asm3 = new(asm2) { Description = "Nah, just kidding..." };

public record struct StockKeepingUnit
{
    public string Value { get; init; }

    public StockKeepingUnit(string value) =>
        Value = value.ToUpper();
}

public abstract record InventoryItem(Guid Id, StockKeepingUnit Sku);
// InventoryItem = Part + Material + ExpirableMaterial

public sealed record Part(
    Guid Id, StockKeepingUnit Sku,
    string Name, string Description) 
    : InventoryItem(Id, Sku);
// Part = Guid * StockKeepingUnit * string * string

public sealed record Material(
    Guid Id, StockKeepingUnit Sku,
    string Description)
    : InventoryItem(Id, Sku);

public sealed record ExpirableMaterial(
    Guid Id, StockKeepingUnit Sku,
    string Description, DateOnly UseBefore)
    : InventoryItem(Id, Sku);

public class AssemblyInstructions
{
    public required string Title { get; init; }
    public required string Description { get; init; }
    private ImmutableList<InventoryItem> Items { get; init; }

    public IEnumerable<(Part part, int quantity)> Parts =>
        this.Items.OfType<Part>()
            .GroupBy(item => item)
            .Select(group => (group.Key, group.Count()));

    public IEnumerable<InventoryItem> Materials =>
        this.Items.Where(item => item is Material || item is ExpirableMaterial);

    private AssemblyInstructions(ImmutableList<InventoryItem> items) =>
        Items = items;

    [System.Diagnostics.CodeAnalysis.SetsRequiredMembers]
    private AssemblyInstructions(
        string title, string description,
        ImmutableList<InventoryItem> items)
        : this(items) =>
        (Title, Description) = (title, description);

    public AssemblyInstructions()
        : this(ImmutableList<InventoryItem>.Empty) {  }

    [System.Diagnostics.CodeAnalysis.SetsRequiredMembers]
    public AssemblyInstructions(string title, string description)
        : this(title, description, ImmutableList<InventoryItem>.Empty) { }

    [System.Diagnostics.CodeAnalysis.SetsRequiredMembers]
    public AssemblyInstructions(AssemblyInstructions other)
        : this(other.Title, other.Description, other.Items) { }

    public AssemblyInstructions Add(InventoryItem item) =>
         new(this) { Items = Items.Add(item) };
}

public static class InventoryUiExtensions
{
    public static void Print(this IEnumerable<InventoryItem> items)
    {
        Console.WriteLine();
        foreach (var item in items.Select(Printable))
        {
            Console.WriteLine($"{item.header,10} - {item.description}");
        }
    }

    public static (string header, string description) Printable(
        this InventoryItem item) => item switch
        {
            Part part => (part.Name, part.Description),
            Material material => ("Material", material.Description),
            ExpirableMaterial expirable => ("Material", $"{expirable.Description} use before {expirable.UseBefore}"),
            _ => throw new System.Diagnostics.UnreachableException()
        };
}
