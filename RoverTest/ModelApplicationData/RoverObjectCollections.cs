using System.Reflection;

namespace RoverTest.ModelApplicationData
{
    //This is the object factory with a collection of each object type.
    //An object type per data table, for example.
    public abstract class RoverObjectCollections
    {
       
        internal string _className;

        protected RoverObjectCollections()
        {
            RegisterCollections();
            Type derivedType = this.GetType();
            _className = derivedType.Name;
        }

        protected Dictionary<string, string> ObjectCollections { get; set; } = [];

        public abstract RoverObjectCollection GetAllRoverCollections();

        public void RegisterCollections()
        {
            Type derivedType = this.GetType();
            Assembly assembly = Assembly.GetAssembly(derivedType);

            var typesWithMyAttribute = assembly.GetTypes()
                .Where(t => t.GetCustomAttribute<RoverCollectionAttribute>() != null);

            foreach (var type in typesWithMyAttribute)
            {
                string typeFullName = type.FullName;
                string typeName = type.Name;
                ObjectCollections.Add(typeName, typeFullName);
            }
        }
    }
}
