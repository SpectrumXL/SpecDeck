namespace SpecDeck.CodeGen.Factories
{
    public static class SpecificationsTemplates
    {
        public const string GeneralTemplate = @"using SpecDeck.Core;
using System.Linq.Expressions;
using {0};

namespace {0}.Specs.{1};

public class {4}{6}Spec : Specification<{2}>
{{
    private readonly {3} _v;

    public {4}{6}Spec({3} v)
    {{
        _v = v;
    }}

    public override Expression<Func<{2}, bool>> ToExpression()
        => (t => t.{4} {5} _v);
}}";

        public const string InRangeTemplate = @"using SpecDeck.Core;
using System.Linq.Expressions;
using {0};

namespace {0}.Specs.{1};

public class {4}{6}Spec : Specification<{2}>
{{
    private readonly {3} _min;
    private readonly {3} _max;

    public {4}{6}Spec({3} min, {3} max)
    {{
        _min = min;
        _max = max;
    }}

    public override Expression<Func<{2}, bool>> ToExpression()
        => (t => t.{4} >= _min && t.{4} <= _max);
}}";

        private const string MethodBasedTemplate = @"
using SpecDeck.Core;
using System.Linq.Expressions;
using {0};

namespace {0}.Specs.{1};

public class {4}{6}Spec : Specification<{2}>
{{
	private readonly {3} _v;

    public {4}{6}Spec({3} v)
    {{
        _v = v;
    }}

	public override Expression<Func<{2}, bool>> ToExpression()
		=> (t => t.{4}.{{method}}(_v));
}}";

        public static readonly string EqualsTemplate = MethodBasedTemplate.Replace("{{method}}", "Equals");
        public static readonly string ContainsTemplate = MethodBasedTemplate.Replace("{{method}}", "Contains");
        public static readonly string StartsWithTemplate = MethodBasedTemplate.Replace("{{method}}", "StartsWith");
        public static readonly string EndsWithTemplate = MethodBasedTemplate.Replace("{{method}}", "EndsWith");
    }
}
