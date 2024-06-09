using SpecDeck.Example;
using SpecDeck.Example.Specs.Customer;

var customers = new List<Customer>()
{
    new()
    {
        Id = 1,
        Name = "John Doe",
        Email = "jdoe@gmail.com"
    },
    new()
    {
        Id = 2,
        Name = "Jane Doe",
        Email = "jadoe@gmail.com"
    }
};

var spec = CustomerSpecs.ByEmailSpec("jadoe@gmail.com") | CustomerSpecs.ByNameSpec("John Doe");
var result = customers.Where(spec.ToPredicate()).ToList();
Console.WriteLine($"Matched {result.Count} customers!");
