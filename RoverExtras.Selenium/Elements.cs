using OpenQA.Selenium;
using RoverTest.ModelUserInterface;
using System.Collections;

namespace RoverExtras.Selenium
{
    public class Elements : IElements
    {
        private readonly List<Element> elements = [];

        public Elements(AppDriver appDriver, By by)
        {
            IWebDriver webDriver = (IWebDriver)appDriver.Driver;
            IReadOnlyCollection<IWebElement> webElements = webDriver.FindElements(by);
            foreach (IWebElement webElement in webElements)
            {
                Element currentElement = new Element(appDriver, webElement, by);
                elements.Add(currentElement);
            }

        }

        public Elements(AppDriver appDriver, By by, Element parentElement)
        {
            IReadOnlyCollection<IWebElement> webElements = parentElement.WebElement.FindElements(by);
            foreach (IWebElement webElement in webElements)
            {
                Element currentElement = new Element(appDriver, webElement, by);
                elements.Add(currentElement);
            }
        }

        public Element this[int i] => elements[i];

        public int Count => elements.Count;

        public IEnumerator<IElement> GetEnumerator()
        {
            return elements.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
