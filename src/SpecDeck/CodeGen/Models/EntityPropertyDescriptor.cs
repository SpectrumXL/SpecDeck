namespace SpecDeck.CodeGen.Models
{
    public class EntityPropertyDescriptor
    {
        public string Name { get; set; } = null!;

        public string Type { get; set; } = null!;

        public bool IsCollection { get; set; }
        public bool IsNullable { get; set; }
    }
}
