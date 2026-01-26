using System.Reflection;
using System.Runtime;

namespace RoverTest.ModelApplicationData
{
    //TODO: This is the FACE of business objects.
    // And it is IN the Rover code not implementation code.
    // Does a template lay this down?
    // Any way you slice it, we get everything with simple inheritance.
    //This is the object factory with a collection of each object type.
    //An object type per data table, for example.
    public class RoverObjectCollections
    {
       
        public RoverObjectCollections()
        {
            RegisterCollections();
        }

        protected Dictionary<string, string> ObjectCollections { get; set; } = [];

        public RoverObjectCollection GetRoverCollection(string name)
        {
            bool registered = ObjectCollections.ContainsKey(name);
            if (!registered) throw new AmbiguousImplementationException("No rover collection by that name.");

            List<Type> theTypes = RoverInternals.GetDerivedClasses<RoverObjectCollection>().ToList();
            RoverObjectCollection returnObject = null;
            foreach (var currentType in theTypes)
            {
                if (name == currentType.Name)
                {
                    returnObject = (RoverObjectCollection)Activator.CreateInstance(currentType);
                    break;
                }
            }

            return returnObject;

        }

        public void RegisterCollections()
        {
            Type derivedType = this.GetType();
            Assembly.GetAssembly(derivedType);


            var typesWithBaseClass = RoverInternals.GetDerivedClasses<RoverObjectCollection>();

            foreach (var type in typesWithBaseClass)
            {
                string typeFullName = type.FullName;
                string typeName = type.Name;
                ObjectCollections.Add(typeName, typeFullName);
            }
        }
    }
}
