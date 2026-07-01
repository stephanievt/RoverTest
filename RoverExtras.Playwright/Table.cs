using RoverTest.ModelUserInterface;

namespace RoverExtras.Playwright
{
    public class Table(PlaywrightAppDriver driver, LocatorType locatorType, string locatorString)
        : Element(driver, locatorType, locatorString), ITable
    {

        public IElement GetRow(int rowIndex)
        {
            return new TableRow(this, rowIndex);
        }

        public IElement GetCell(IElement row, int columnIndex)
        {
            // Cast IElement back to Element to access the parent-aware constructor
            if (row is not Element elementRow)
            {
                throw new ArgumentException("Row must be an Element instance from this framework", nameof(row));
            }

            return new TableCell(elementRow, columnIndex);
        }

        public IElements GetCellsFromColumn(int columnIndex)
        {
            // Get all cells from a specific column across all rows
            // CSS selector for nth column in all rows within this table
            var ele = new Elements(this, $"tbody tr td:nth-of-type({columnIndex + 1})");
            return ele;
        }
    }
}
