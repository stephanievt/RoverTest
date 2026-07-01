namespace RoverExtras.Playwright
{
    //TODO: Abstract this out to interface(s) and ... etcetera across
    // implementations. This todo includes
    // all table related changes.
    public class TableRow(Table parentTable, int rowIndex) : Element(parentTable, $"tr:nth-of-type({rowIndex + 1})");
}
