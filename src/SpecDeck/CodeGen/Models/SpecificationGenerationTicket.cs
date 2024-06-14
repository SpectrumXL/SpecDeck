namespace SpecDeck.CodeGen.Models
{
    internal class SpecificationGenerationTicket
    {
        public string Type { get; set; }
        public string Name { get; set; }
        public string Operation { get; set; }

        public SpecificationGenerationTicket(string type, string name, string operation)
        {
            Type = type;
            Name = name;
            Operation = operation;
        }
    }
}
