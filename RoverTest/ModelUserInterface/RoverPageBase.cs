using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace RoverTest.ModelUserInterface
{
    /// <summary>
    /// This abstract class serves as CONTRACT and service provider
    /// to the consumer of the rover framework. Each page of the application
    /// implements RoverPageBase.
    /// Rover pages are created ONLY by factory.
    /// App driver property will be set in factory.
    /// </summary>
    [SuppressMessage("CodeQuality", "IDE0052:Remove unread private members",
    Justification = "Derived classes use fields initialized via reflection")]
    public abstract class RoverPageBase
    {
        private readonly AppDriver _appDriver;
        private readonly Type _elementType;

        public RoverPageAction LastRoverPageAction { get; set; }

        protected RoverPageBase(AppDriver appDriver)
        {
            _appDriver = appDriver;
            _elementType = ResolveElementType(appDriver);
            InitializePageAppDriver(appDriver);
            InitializeElements();
            RegisterRoverPageActions();
        }

        public List<RoverPageAction> AvailableRoverPageActions { get; set; } = [];

        public List<RoverPageAction> AccessedRoverPageActionsStack { get; set; } = [];

        public abstract IElement PageIdentifier { get; }

        public abstract bool Exists { get; }

        /// <summary>
        /// Gets the AppDriver for use in derived classes
        /// </summary>
        protected AppDriver AppDriver => _appDriver;

        /// <summary>
        /// Resolves the Element implementation type based on the AppDriver type.
        /// Convention: Element lives in same namespace as AppDriver (e.g., RoverExtras.Playwright.Element)
        /// </summary>
        private static Type ResolveElementType(AppDriver appDriver)
        {
            var driverType = appDriver.GetType();
            var elementTypeName = $"{driverType.Namespace}.Element";

            var elementType = driverType.Assembly.GetTypes()
                .FirstOrDefault(t => t.FullName == elementTypeName && typeof(IElement).IsAssignableFrom(t));

            if (elementType == null)
            {
                throw new InvalidOperationException(
                    $"Could not find Element implementation for driver type '{driverType.FullName}'. " +
                    $"Expected to find '{elementTypeName}' in assembly '{driverType.Assembly.FullName}'. " +
                    $"Ensure your Element class is in the same namespace as your AppDriver.");
            }

            return elementType;
        }

        /// <summary>
        /// Initializes any field marked with [PageAppDriver] attribute
        /// </summary>
        private void InitializePageAppDriver(AppDriver appDriver)
        {
            Type derivedType = GetType();
            FieldInfo[] fields = derivedType.GetFields(BindingFlags.NonPublic | BindingFlags.Instance);

            foreach (var field in fields)
            {
                var attribute = field.GetCustomAttribute<RoverPageAppDriverAttribute>();
                if (attribute != null)
                {
                    if (field.FieldType.IsAssignableFrom(appDriver.GetType()))
                    {
                        field.SetValue(this, appDriver);
                    }
                    else
                    {
                        throw new InvalidOperationException(
                            $"Cannot assign AppDriver of type {appDriver.GetType().Name} to field of type {field.FieldType.Name}. " +
                            $"Make sure you're passing the correct AppDriver type.");
                    }
                }
            }
        }

        /// <summary>
        /// Initializes properties decorated with [LocateBy] attribute
        /// </summary>
        private void InitializeElements()
        {
            Type derivedType = GetType();
            PropertyInfo[] properties = derivedType.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

            foreach (var property in properties)
            {
                var attribute = property.GetCustomAttributes()
                    .FirstOrDefault(attr => attr.GetType().Name == "LocateByAttribute");

                if (attribute != null && typeof(IElement).IsAssignableFrom(property.PropertyType))
                {
                    var howProperty = attribute.GetType().GetProperty("How");
                    var usingProperty = attribute.GetType().GetProperty("Using");

                    if (howProperty != null && usingProperty != null)
                    {
                        var how = howProperty.GetValue(attribute);
                        var usingValue = (string)usingProperty.GetValue(attribute);

                        IElement element = CreateElement(how, usingValue);
                        property.SetValue(this, element);
                    }
                }
            }
        }

        /// <summary>
        /// Creates an IElement based on the locator type and string.
        /// </summary>
        protected virtual IElement CreateElement(object locatorType, string locatorString)
        {
            var elementType = _elementType;
            Type locatorEnumType = locatorType.GetType();

            var constructor = elementType.GetConstructor([_appDriver.GetType(), locatorEnumType, typeof(string)]);

            if (constructor != null)
            {
                return (IElement)constructor.Invoke([_appDriver, locatorType, locatorString]);
            }

            constructor = elementType.GetConstructor([typeof(AppDriver), locatorEnumType, typeof(string)]);
            if (constructor != null)
            {
                return (IElement)constructor.Invoke([_appDriver, locatorType, locatorString]);
            }

            var availableConstructors = elementType.GetConstructors();
            var constructorInfo = string.Join(", ", availableConstructors.Select(c =>
                $"({string.Join(", ", c.GetParameters().Select(p => p.ParameterType.Name))})"));

            throw new InvalidOperationException(
                $"Could not find matching constructor for Element type '{elementType.FullName}'. " +
                $"Looking for constructor with parameters: ({_appDriver.GetType().Name}, {locatorEnumType.Name}, String). " +
                $"Available constructors: {constructorInfo}");
        }

        public void RegisterRoverPageActions()
        {
            Type derivedType = GetType();
            MethodInfo[] allMethods = derivedType.GetMethods(BindingFlags.Public | BindingFlags.Instance);

            foreach (var method in allMethods)
            {
                var attribute = method.GetCustomAttribute<RoverPageActionMethodAttribute>();

                if (attribute != null)
                {
                    if (attribute.RoverPageName == derivedType.Name)
                    {
                        RoverPageAction pa = new RoverPageAction
                        {
                            MethodName = attribute.RoverMethodName,
                            ResultRoverPage = this
                        };

                        AvailableRoverPageActions.Add(pa);
                    }
                }
            }
        }

        public void ExecuteRoverPageAction(string actionName)
        {
            RoverPageAction action = AvailableRoverPageActions
                .FirstOrDefault(a => a.MethodName == actionName);

            if (action != null)
            {
                var method = GetType().GetMethod(action.MethodName);
                if (method != null)
                {
                    method.Invoke(this, null);
                    AccessedRoverPageActionsStack.Add(action);
                    LastRoverPageAction = action;
                }
            }
            else
            {
                throw new InvalidOperationException($"Action '{actionName}' not found on page {GetType().Name}");
            }
        }
    }
}