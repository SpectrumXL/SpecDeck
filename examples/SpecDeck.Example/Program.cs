using SpecDeck.Example;
using static SpecDeck.Example.Specs.Customer.CustomerSpecs;



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

var spec = EmailContainsSpec("ja") | EmailEndsWithSpec(".com");
var result = customers.Where(spec.ToPredicate()).ToList();
Console.WriteLine($"Matched {result.Count} customers!");
