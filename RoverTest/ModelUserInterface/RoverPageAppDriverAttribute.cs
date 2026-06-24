namespace RoverTest.ModelUserInterface
{
    /// <summary>
    /// Marks a field as the page's specific AppDriver implementation.
    /// The RoverPageBase will automatically initialize this field during construction.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class RoverPageAppDriverAttribute : Attribute
    // ReSharper disable once RedundantTypeDeclarationBody
    {
    }
}
