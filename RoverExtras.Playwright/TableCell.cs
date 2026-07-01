namespace RoverExtras.Playwright
{
    public class TableCell(Element parentRow, int columnIndex)
        : Element(parentRow, $"td:nth-of-type({columnIndex + 1})");
}
