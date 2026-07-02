using RoverTest.ModelUserInterface;

namespace RoverExtras.Playwright
{
    public class Table(PlaywrightAppDriver driver, LocatorType locatorType, string locatorString)
        : Element(driver, locatorType, locatorString), ITable
    {
        public IElement GetCell(int rowIndex, int colIndex)
        {
            string locatorString = $"tbody tr:nth-child({rowIndex}) td:nth-child({colIndex})";
            return new Element(driver, LocatorType.LocatorMethod, locatorString);
        }

        public IElements GetCellsFromColumn(int colIndex)
        {
            colIndex = colIndex + 1;
            Elements elements = new Elements(this, LocatorType.LocatorMethod, $"tbody tr td:nth-child({colIndex})");
            return elements;
        }
    }
}
