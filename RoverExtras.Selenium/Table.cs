using OpenQA.Selenium;
using RoverTest.ModelUserInterface;
#pragma warning disable CS9107 // Parameter is captured into the state of the enclosing type and its value is also passed to the base constructor. The value might be captured by the base class as well.

namespace RoverExtras.Selenium
{
    public class Table(AppDriver appDriver, By by) : Element(appDriver, by), ITable
    {


        //TODO: Do I even need this? This may go away with new classes.
        public Element TableElement { get; set; } = new Element(appDriver, by);


        public Elements Rows
        {
            get
            {
                By by = By.CssSelector("tbody > tr");
                Elements rows = new Elements(appDriver, by, TableElement);
                return rows;
            }
        }

        
        public IElement GetRow(int rowIndex)
        {

            return Rows[rowIndex];

        }

        public IElement GetCell(IElement row, int columnIndex)
        {
            Element localRow = (Element)row;
            By by = By.TagName("td");
            
            return new Element(appDriver, by, localRow);
        }

        public IElements GetCellsFromColumn(int columnIndex)
        {
            throw new NotImplementedException();
        }

        public IElement GetCell(Element row, int columnIndex)
        {
            By by = By.TagName("td");
            Elements element = new Elements(appDriver, by, row);
            return element[columnIndex];
        }
    }
}
