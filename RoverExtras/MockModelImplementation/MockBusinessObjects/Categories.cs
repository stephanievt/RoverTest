using System.Collections;
using RoverTest.ModelApplicationData;

namespace RoverExtras.MockModelImplementation.MockBusinessObjects
{
    [RoverCollection]
    public class Categories : RoverObjectCollection, IEnumerable<Category>
    {
        private readonly List<Category> _categories = [];

        public void Add(Category category)
        {
            _categories.Add(category);
        }

        public IEnumerator<Category> GetEnumerator()
        {
            return _categories.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
