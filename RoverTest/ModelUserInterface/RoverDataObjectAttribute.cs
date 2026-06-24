namespace RoverTest.ModelUserInterface
{
    [AttributeUsage(AttributeTargets.Property)]
    public class RoverDataObjectAttribute(string name) : Attribute
    {
        public string Name { get; set; } = name;
        
    }
}
