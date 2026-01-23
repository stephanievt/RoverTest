using System.Collections;
using RoverTest.ModelApplicationData;

namespace RoverExtras.MockModelImplementation.MockBusinessObjects
{
    //TODO: I probably do not need an attribute. I can get 'em
    // from is a rover object collection.
    [RoverCollection]
    public class Items : RoverObjectCollection, IEnumerable<Item>
    {
        private readonly List<Item> _items = [];

        public void Add(Item item)
        {
            _items.Add(item);
        }
        public IEnumerator<Item> GetEnumerator()
        {
            return _items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
