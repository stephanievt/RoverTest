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
            throw new NotImplementedException();
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
