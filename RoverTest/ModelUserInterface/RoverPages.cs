using System.Reflection;

namespace RoverTest.ModelUserInterface
{
    /// <summary>
    /// Rover Page Factory. ABSTRACTED class that
    /// is exposed to and implemented by the rover consumer app automation.
    /// </summary>
    public abstract class RoverPages
    {

        internal string _className;

        public RoverEcu RoverEcu { get; set; }

        public AppDriver AppDriver => RoverEcu.AppDriver;

        // This is a string of KEYS which is the class name with the value 
        // of a fully qualified string to create the page instance.
        public Dictionary<string, string> Pages { get; } = [];

        protected RoverPages()
        {
            // ReSharper disable once VirtualMemberCallInConstructor
            RegisterPages();
            Type derivedType = this.GetType();
            _className = derivedType.Name;
        }

        public abstract RoverPageBase GetPage(string pageName);

        /// <summary>
        /// Users of this framework will add pages (POM abstraction) to the list by class name
        /// as with Pages.Add("pageHome");
        /// </summary>
        public void RegisterPages()
        {
            Type derivedType = this.GetType();
            Assembly assembly = Assembly.GetAssembly(derivedType);

            var typesWithMyAttribute = assembly.GetTypes()
                .Where(t => t.GetCustomAttribute<RoverPageAttribute>() != null);

            foreach (var type in typesWithMyAttribute)
            {
                string typeFullName = type.FullName;
                string typeName = type.Name;
                Pages.Add(typeName, typeFullName);
            }
           
        }

        
    }
}
