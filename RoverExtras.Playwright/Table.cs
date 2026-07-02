using RoverTest.ModelUserInterface;

namespace RoverExtras.Playwright
{
    public class Table(PlaywrightAppDriver driver, LocatorType locatorType, string locatorString)
        : Element(driver, locatorType, locatorString), ITable
    {
        public IElement GetCell(int row, int columnIndex)
        {
            throw new NotImplementedException();
        }

        public IElements GetCellsFromColumn(int colIndex)
        {
            // Get all cells from a specific column across all rows
            // CSS selector for nth column in all rows within this table
            var ele = new Elements(this, $"tbody tr td:nth-of-type({colIndex + 1})");
            return ele;
        }
    }
}
