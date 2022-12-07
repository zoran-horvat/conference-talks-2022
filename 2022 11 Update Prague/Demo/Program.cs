using Demo;

IEnumerable<Person> persons =
    JsonSerializer.Deserialize<InputPerson[]>(@"[
        { ""firstName"": ""Edsger"", ""lastName"": ""Dijkstra"" },
        { ""firstName"": ""Grace"", ""lastName"": ""Hopper"" },
        { ""firstName"": ""Ada"" },
        { },
        { ""firstName"": ""Aristotle"" },
        { ""firstName"": ""Charles"", ""lastName"": ""Babbage"" },
        { }
    ]", new JsonSerializerOptions() { PropertyNameCaseInsensitive = true })
    .ToPersons().OfType<ActualPerson>().Cast<Person>() ?? Array.Empty<Person>();

persons.DemoReport();
