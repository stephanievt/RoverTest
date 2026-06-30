namespace RoverTest.ModelUserInterface;

[AttributeUsage(AttributeTargets.Class)]
public class PageActionAttribute : Attribute
{
    public Type PageType { get; }
    public string DisplayName { get; }

    // ReSharper disable once ConvertToPrimaryConstructor
    public PageActionAttribute(Type pageType, string displayName = null)
    {
        PageType = pageType;
        DisplayName = displayName;
    }
}