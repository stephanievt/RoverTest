namespace RoverTest.ModelUserInterface
{
    public class RoverPageFactory
    {
        internal string _className;

        // This is a string of KEYS which is the class name with the value 
        // of a fully qualified string to create the page instance.
        public Dictionary<string, string> Pages { get; } = [];

        public AppDriver AppDriver { get; }

        public RoverPageFactory(AppDriver appDriver)
        {
            AppDriver = appDriver;
            Type derivedType = this.GetType();
            _className = derivedType.Name;
        }

        // New generic method to return the specific page type
        public T GetPage<T>() where T : RoverPageBase
        {
            Type pageType = typeof(T);
            return (T)Activator.CreateInstance(pageType, AppDriver);
        }

        /// <summary>
        /// Users of this framework will add pages (POM abstraction) to the list by class name
        /// as with Pages.Add("pageHome");
        /// </summary>
        public void RegisterPages()
        {
            IEnumerable<Type> derivedTypes = RoverInternals.GetDerivedClasses<RoverPageBase>();

            foreach (var type in derivedTypes)
            {
                string typeFullName = type.FullName;
                string typeName = type.Name;
                Pages.Add(typeName, typeFullName);
            }
        }
    }
}