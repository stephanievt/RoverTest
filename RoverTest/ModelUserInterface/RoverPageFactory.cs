namespace RoverTest.ModelUserInterface
{
    public class RoverPageFactory
    {

        // This is a string of KEYS which is the class name with the value 
        // of a fully qualified string to create the page instance.
        public Dictionary<string, string> Pages { get; } = [];

        public AppDriver AppDriver { get; }

        public RoverPageFactory(AppDriver appDriver)
        {
            AppDriver = appDriver;
            RegisterPages();

        }

        public object GetPage(string pageName)
        {
            if (!Pages.TryGetValue(pageName, out string fullyQualifiedTypeName))
            {
                throw new InvalidOperationException(
                    $"Page '{pageName}' not found in registered pages. " +
                    $"Available pages: {string.Join(", ", Pages.Keys)}");
            }

            Type pageType = Type.GetType(fullyQualifiedTypeName);

            if (pageType == null)
            {
                throw new InvalidOperationException(
                    $"Could not load type '{fullyQualifiedTypeName}' for page '{pageName}'.");
            }

            object pageInstance = Activator.CreateInstance(pageType, AppDriver);
            return pageInstance;
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
                //string typeFullName = type.FullName;
                string assemblyQualifiedName = type.AssemblyQualifiedName;
                string typeName = type.Name;
                Pages.Add(typeName, assemblyQualifiedName);
            }
        }
    }
}