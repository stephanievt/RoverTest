using System.Collections;
using RoverTest.ModelApplicationData;

namespace RoverExtras.MockModelImplementation.MockBusinessObjects
{

    public class Items : RoverObjectCollection, IEnumerable<Item>
    {
        private readonly List<Item> _items;

        public Items()
        {
            _items = LoadRoverData<Item>();
        }

        public int Count => _items.Count;

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
