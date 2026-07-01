using System.Collections;
using RoverTest.ModelUserInterface;

namespace RoverExtras.Playwright
{
    public class Elements : IElements
    {
        private readonly List<Element> _elements = [];

        // Internal constructor for finding multiple elements from a parent element
        public Elements(Element parentElement, string childLocator)
        {
            // Get all matching locators scoped to the parent
            var scopedLocator = parentElement.Locator.Locator(childLocator);
            var locatorCount = scopedLocator.CountAsync().GetAwaiter().GetResult();

            for (int i = 0; i < locatorCount; i++)
            {
                // Create an element for each matched locator using nth(i)
                var element = new Element(parentElement, childLocator, i);
                _elements.Add(element);
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
