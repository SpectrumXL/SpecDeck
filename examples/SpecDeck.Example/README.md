# Spectrum Example Library

## Overview

This example demonstrates the use of the `Spectrum` library to generate specifications for entities using source
generators. The library simplifies the creation and application of specifications to filter collections of entities
or access to persistence via ORMs or any other kind of adapter which works with ExpressionTrees.

## Installation

To use the Spectrum library with source generators, follow these steps:

1. **Install the NuGet package** in your target project:
   ```sh
   dotnet add package YourLibraryName
   ```
2. **Define an entity class and mark it with the `SpectrumSpec` attribute**:
   ```csharp
   namespace Spectrum.Example;
   
    [SpectrumSpec]
    public class Customer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
    }
   ```
3. Use generated specifications to get filtering expression tree:

   ```csharp
   var spec = CustomerSpecs.ByEmailSpec("jadoe@gmail.com") | CustomerSpecs.ByNameSpec("John Doe");
   var result = customers.Where(spec.ToPredicate()).ToList();
   Console.WriteLine($"Matched {result.Count} customers!");
   ```

### Generated Classes and Factory

1. **Generated Specification Classes**:
    - The specification classes are automatically generated for each public property of the entity class marked with
      the `SpectrumSpec` attribute.
    - Each generated class will be named using the pattern `By{PropertyName}Spec`.
    - These classes will inherit from `Specification<T>` and implement the `ToExpression` method to provide the
      filtering
      logic based on the property.

2. **Specification Factory Class**:
    - Alongside the specification classes, a factory class is generated in the same namespace. This factory class helps
      in creating instances of the generated specification classes.
    - The factory class will be named using the pattern `{EntityName}Specs`.
    - It will contain static methods to create instances of each generated specification class.
    - For example, for the `Customer` entity class, the factory class will be named `CustomerSpecs`.

### Combining Specifications

- Specifications can be combined using logical operators:
    - Use the `&` operator to combine specifications with a logical AND.
    - Use the `|` operator to combine specifications with a logical OR.
- The `ToPredicate` method can be used to convert the expression tree into a predicate function. This function can be
  used to filter collections of entities.

<details>
  <summary>Generated Code</summary>

### Specification Classes

Generate specification classes for filtering `Customer` entities:

#### ByEmailSpec

   ```csharp
   using Spectrum.Core;
   using System.Linq.Expressions;
   using Spectrum.Example;
   
   namespace Spectrum.Example.Specs.Customer
   {
       public class ByEmailSpec(string v) : Specification<Spectrum.Example.Customer>
       {
           private readonly string _v = v;
   
           public override Expression<Func<Spectrum.Example.Customer, bool>> ToExpression()
               => (t => t.Email.Equals(_v));
       }
   }
   ```

#### ByIdSpec

   ```csharp
   using Spectrum.Core;
   using System.Linq.Expressions;
   using Spectrum.Example;
   
   namespace Spectrum.Example.Specs.Customer
   {
       public class ByIdSpec(int v) : Specification<Spectrum.Example.Customer>
       {
           private readonly int _v = v;
   
           public override Expression<Func<Spectrum.Example.Customer, bool>> ToExpression()
               => (t => t.Id.Equals(_v));
       }
   }
   ```

#### ByNameSpec

   ```csharp
   using Spectrum.Core;
   using System.Linq.Expressions;
   using Spectrum.Example;
   
   namespace Spectrum.Example.Specs.Customer
   {
       public class ByNameSpec(string v) : Specification<Spectrum.Example.Customer>
       {
           private readonly string _v = v;
   
           public override Expression<Func<Spectrum.Example.Customer, bool>> ToExpression()
               => (t => t.Name.Equals(_v));
       }
   }
   ```

### Specifications Factory

Provide methods to easily create specification instances:
   
   ```csharp
   using Spectrum.Core;
   using System.Linq.Expressions;
   
   namespace Spectrum.Example.Specs.Customer
   {
       public static class CustomerSpecs
       {
           public static Specification<Spectrum.Example.Customer> All<T>() => Specification<Spectrum.Example.Customer>.True;
   
           public static ByIdSpec ByIdSpec(int v) => new(v);
           public static ByNameSpec ByNameSpec(string v) => new(v);
           public static ByEmailSpec ByEmailSpec(string v) => new(v);
       }
   }
   ```
</details>

## Conclusion

This example demonstrates how to use the Spectrum library to create and apply specifications for filtering entities. By
using source generators, the library simplifies the creation of specifications and ensures that the code remains clean
and maintainable.
