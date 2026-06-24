namespace RoverTest.ModelUserInterface
{
    public class RoverPageFactory
    {
        internal string _className;
        private readonly Dictionary<Type, Type> _interfaceToImplementationMap = new();

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
        public T GetPage<T>() where T : class
        {
            Type requestedType = typeof(T);
            Type pageType;

            // If T is an interface, resolve it to a concrete implementation
            if (requestedType.IsInterface)
            {
                if (_interfaceToImplementationMap.TryGetValue(requestedType, out var implementationType))
                {
                    pageType = implementationType;
                }
                else
                {
                    // Auto-discover: Find a class that implements this interface and derives from RoverPageBase
                    pageType = RoverInternals.GetDerivedClasses<RoverPageBase>()
                        .FirstOrDefault(t => requestedType.IsAssignableFrom(t));

                    if (pageType == null)
                    {
                        throw new InvalidOperationException(
                            $"No implementation found for interface {requestedType.Name}. " +
                            $"Register it using RegisterImplementation<{requestedType.Name}, TImplementation>()");
                    }

                    // Cache for future use
                    _interfaceToImplementationMap[requestedType] = pageType;
                }
            }
            else
            {
                pageType = requestedType;
            }

            return (T)Activator.CreateInstance(pageType, AppDriver);
        }

        // Allow explicit registration of interface-to-implementation mappings
        public void RegisterImplementation<TInterface, TImplementation>()
            where TInterface : class
            where TImplementation : RoverPageBase, TInterface
        {
            _interfaceToImplementationMap[typeof(TInterface)] = typeof(TImplementation);
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