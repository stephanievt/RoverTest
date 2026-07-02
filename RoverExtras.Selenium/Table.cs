using OpenQA.Selenium;
using RoverTest.ModelUserInterface;

namespace RoverExtras.Selenium
{
    public class Table : Element, ITable
    {
        private SeleniumAppDriver _appDriver;
        public Table(SeleniumAppDriver appDriver, By by, Element parent) : base(appDriver, by, parent)
        {
            _appDriver = appDriver;
        }

        public Table(SeleniumAppDriver appDriver, By by) : base(appDriver, by)
        {
            _appDriver = appDriver;
        }

        /// <summary>
        /// While the underlying Selenium uses 1-based indexing,
        /// this is handled in the method for compatibility with the
        /// 0-based indexing we are accustomed to.
        /// </summary>
        /// <param name="rowIndex"></param>
        /// <param name="colIndex"></param>
        /// <returns></returns>
        public IElement GetCell(int rowIndex, int colIndex)
        {
            // I am doing each var separately for readability.
            // I could make this fewer lines, and choose not to.
            rowIndex += 1; // XPath positions start at 1
            colIndex += 1;
            string cssSelector = $"tr:nth-child({rowIndex}) > :nth-child({colIndex})";
            By by = By.CssSelector(cssSelector);
            Element ele = new Element(_appDriver, by, this);
            return ele;
        }

        /// <summary>
        /// While the underlying Selenium uses 1-based indexing,
        /// this is handled in the method for compatibility with the
        /// 0-based indexing we are accustomed to.
        /// </summary>
        /// <param name="colIndex"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public IElements GetCellsFromColumn(int colIndex)
        {
            colIndex = colIndex + 1;
            //string cssSelector = $"tr > :nth-child({colIndex})";
            string cssSelector = $"tbody > tr > :nth-child({colIndex})";
            By by = By.CssSelector(cssSelector);
            Elements elements = new Elements(_appDriver, by, this);
            return elements;
        }
    }
}
