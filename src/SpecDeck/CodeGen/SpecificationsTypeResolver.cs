using System.Collections.Generic;
using SpecDeck.CodeGen.Models;

namespace SpecDeck.CodeGen
{
    internal static class SpecificationsTypeResolver
    {
        private static readonly Dictionary<string, SpecificationGenerationTicket[]> SpecificationTypes =
            new Dictionary<string, SpecificationGenerationTicket[]>()
            {
                {
                    "numeric", new[]
                    {
                        new SpecificationGenerationTicket("numeric", "Equals", "=="),
                        new SpecificationGenerationTicket("numeric", "GreaterThan", ">"),
                        new SpecificationGenerationTicket("numeric", "LessThan", "<"),
                        new SpecificationGenerationTicket("numeric", "GreaterThanOrEquals", ">="),
                        new SpecificationGenerationTicket("numeric", "LessThanOrEquals", "<="),
                        new SpecificationGenerationTicket("numeric", "InRange", "in"),
                    }
                },
                {
                    "string", new[]
                    {
                        new SpecificationGenerationTicket("string", "Equals", "=="),
                        new SpecificationGenerationTicket("string", "StartsWith", "starts with"),
                        new SpecificationGenerationTicket("string", "EndsWith", "ends with"),
                        new SpecificationGenerationTicket("string", "Contains", "contains"),
                        // new SpecificationGenerationTicket("string", "Regex", "regex")
                    }
                },
                {
                    "date", new[]
                    {
                        new SpecificationGenerationTicket("date", "Equals", "=="),
                        new SpecificationGenerationTicket("date", "GreaterThan", ">"),
                        new SpecificationGenerationTicket("date", "LessThan", "<"),
                        new SpecificationGenerationTicket("date", "GreaterThanOrEquals", ">="),
                        new SpecificationGenerationTicket("date", "LessThanOrEquals", "<="),
                        new SpecificationGenerationTicket("date", "InRange", "in")
                    }
                }
            };

        private static readonly Dictionary<string, string> TypesMapping = new Dictionary<string, string>()
        {
            { "int", "numeric" },
            { "long", "numeric" },
            { "double", "numeric" },
            { "float", "numeric" },
            { "decimal", "numeric" },

            { "string", "string" },

            { "DateTime", "date" },
            { "DateTimeOffset", "date" },
            { "DateOnly", "date" },
            { "TimeOnly", "date" }
        };

        internal static SpecificationGenerationTicket[] Resolve(string type)
        {
            var typeWithoutNamespace = type.Contains(".") ? type.Substring(type.LastIndexOf('.') + 1) : type;
            var isTypeMatched = TypesMapping.ContainsKey(typeWithoutNamespace);
            if (!isTypeMatched)
            {
                return new[] { new SpecificationGenerationTicket("unmatched", "Equals", "==") };
            }

            var isSpecTypeMatched = SpecificationTypes.ContainsKey(TypesMapping[typeWithoutNamespace]);
            if (!isSpecTypeMatched)
            {
                return new[] { new SpecificationGenerationTicket("unmatched", "Equals", "==") };
            }

            return SpecificationTypes[TypesMapping[typeWithoutNamespace]];
        }
    }
}
