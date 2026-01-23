using System.Collections;
using RoverTest.ModelUserInterface;

namespace RoverExtras.MockModelImplementation.MockUserInterface
{
    public class MockElements : IElements
    {
        private readonly List<MockElement> elements = [];
        
        public void Add(MockElement element)
        {
            elements.Add(element);
        }

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
