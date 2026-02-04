using System.Reflection;
using System.Runtime;

namespace RoverTest.ModelUserInterface
{

    public class RoverPages
    {

        internal string _className;

        public RoverEcu RoverEcu { get; set; }

        public AppDriver AppDriver => RoverEcu.AppDriver;

        // This is a string of KEYS which is the class name with the value 
        // of a fully qualified string to create the page instance.
        public Dictionary<string, string> Pages { get; } = [];

        public RoverPages()
        {
            // ReSharper disable once VirtualMemberCallInConstructor
            RegisterPages();
            Type derivedType = this.GetType();
            _className = derivedType.Name;
        }

        public RoverPageBase GetPage(string pageName)
        {
            bool registered = Pages.ContainsKey(pageName);
            if (!registered) throw new AmbiguousImplementationException("No rover page by that name.");

            List<Type> theTypes = RoverInternals.GetDerivedClasses<RoverPageBase>().ToList();
            RoverPageBase returnObject = null;
            foreach (var currentType in theTypes)
            {
                if (pageName == currentType.Name)
                {
                    returnObject = (RoverPageBase)Activator.CreateInstance(currentType);
                    break;
                }
            }

            return returnObject;
        }

        /// <summary>
        /// Users of this framework will add pages (POM abstraction) to the list by class name
        /// as with Pages.Add("pageHome");
        /// </summary>
        public void RegisterPages()
        {
            Type derivedType = this.GetType();
            Assembly assembly = Assembly.GetAssembly(derivedType);

           
            var derivedTypes = RoverInternals.GetDerivedClasses<RoverPageBase>();

            foreach (var type in derivedTypes)
            {
                string typeFullName = type.FullName;
                string typeName = type.Name;
                Pages.Add(typeName, typeFullName);
            }
           
        }

        
    }
}
