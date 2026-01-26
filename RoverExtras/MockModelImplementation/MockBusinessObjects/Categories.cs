using System.Collections;
using RoverTest.ModelApplicationData;

namespace RoverExtras.MockModelImplementation.MockBusinessObjects
{
    public class Categories : RoverObjectCollection, IEnumerable<Category>
    {
        private readonly List<Category> _categories;

        public Categories()
        {
           _categories = LoadRoverData<Category>();
        }

        public void Add(Category category)
        {
            _categories.Add(category);
        }

        public int Count => _categories.Count;

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
