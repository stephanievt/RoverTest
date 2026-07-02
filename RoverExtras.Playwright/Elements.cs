using System.Collections;
using RoverTest.ModelUserInterface;

namespace RoverExtras.Playwright
{
    public class Elements : IElements
    {
        private readonly List<Element> _elements = [];

        // Internal constructor for finding multiple elements from a parent element
        public Elements(Element parentElement, LocatorType locatorType, string locatorString)
        {
            var locators = parentElement.Locator.Locator(locatorString).AllAsync().GetAwaiter().GetResult();
            PlaywrightAppDriver pwAppDriver = parentElement.PlaywrightAppDriver;
            foreach (var locator in locators)
            {
                ;
                Element currentElement = new Element(pwAppDriver, locator);
                _elements.Add(currentElement);
            }
        }

        public int Count => _elements.Count;

        public IEnumerator<IElement> GetEnumerator()
        {
            return _elements.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
