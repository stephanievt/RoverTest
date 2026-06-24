namespace RoverTest.ModelUserInterface
{
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class RoverDataCollectionAttribute(string name) : Attribute
    {
        public string Name { get; set; } = name;
    }
}
