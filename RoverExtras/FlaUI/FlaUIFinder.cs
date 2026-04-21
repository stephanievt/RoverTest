namespace RoverExtras.FlaUI
{
    [AttributeUsage(AttributeTargets.Property)]
    public class FlaUIFinder : Attribute
    {

        public string FinderString { get; set; }

        public FlauiElementType ElementType { get; set; }

        public FlauiSearchTech SearchTech { get; set; }

        public FlauiFinderTech FinderTech { get; set; } = FlauiFinderTech.A3;
    }
}