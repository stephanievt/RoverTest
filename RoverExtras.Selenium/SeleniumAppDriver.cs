using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using RoverTest.ModelUserInterface;
using SeleniumExtras.PageObjects;

namespace RoverExtras.Selenium
{
    public class SeleniumAppDriver : AppDriver
    {
        private readonly string _location;
        private readonly IWebDriver _webDriver;
        public sealed override object Driver { get; }
        public override Task NavigateAsync()
        {
            _webDriver.Url = _location;
            return Task.CompletedTask;
        }

        public override void Dispose()
        {
            _webDriver.Close();
            _webDriver.Dispose();
        }

        public override byte[] TakeScreenshot()
        {
            throw new NotImplementedException();
        }


        public SeleniumAppDriver(string location) : base(location)
        {
            _location = location;
            IWebDriver webDriver = new ChromeDriver();
            _webDriver = webDriver;
            Driver = webDriver;

            
        }

        public WebDriverWait WebDriverWait
        {
            get
            {
                WebDriverWait wait = new WebDriverWait(_webDriver, TimeSpan.FromSeconds(30)); // Waits up to 10 seconds
                return wait;
            }
        }

        /// <summary>
        /// Creates Selenium-specific element instances based on the [FindsBy] attribute.
        /// Uses reflection to dynamically find concrete implementations in the RoverExtras.Selenium namespace.
        /// Returns null if the attribute is not a FindsByAttribute (allowing other drivers to handle it).
        /// </summary>
        public override IElement CreateElement(Type elementInterfaceType, Attribute locatorAttribute)
        {
            // Only process FindsBy attributes - ignore others (LocateBy, etc.)
            if (locatorAttribute is not FindsByAttribute findsBy)
                return null;

            // Convert FindsBy attribute to Selenium By locator
            By by = CreateByLocator(findsBy);

            // Find concrete implementation in this assembly that implements the interface
            var concreteType = FindConcreteElementType(elementInterfaceType);

            if (concreteType == null)
            {
                throw new NotSupportedException(
                    $"Could not find a concrete Selenium element type that implements '{elementInterfaceType.Name}'. " +
                    $"Ensure you have a class in the RoverExtras.Selenium namespace that implements this interface.");
            }

            // Get the constructor: (SeleniumAppDriver, By) or (AppDriver, By)
            var constructor = concreteType.GetConstructor(new[]
            {
                typeof(SeleniumAppDriver),
                typeof(By)
            });

            if (constructor == null)
            {
                // Try with base AppDriver type
                constructor = concreteType.GetConstructor(new[]
                {
                    typeof(AppDriver),
                    typeof(By)
                });
            }

            if (constructor == null)
            {
                throw new InvalidOperationException(
                    $"Concrete type '{concreteType.Name}' does not have a constructor with signature " +
                    $"({nameof(SeleniumAppDriver)}, By) or ({nameof(AppDriver)}, By)");
            }

            // Invoke the constructor to create the element
            return (IElement)constructor.Invoke(new object[] { this, by });
        }

        /// <summary>
        /// Finds a concrete class in this assembly that implements the specified interface.
        /// Prefers classes in the RoverExtras.Selenium namespace.
        /// </summary>
        private Type FindConcreteElementType(Type interfaceType)
        {
            var assembly = GetType().Assembly;
            var ourNamespace = GetType().Namespace;

            // Find all types in this assembly that:
            // 1. Are classes (not interfaces or abstracts)
            // 2. Implement the target interface
            // 3. Are in the same namespace as this driver (RoverExtras.Selenium)
            var candidateTypes = assembly.GetTypes()
                .Where(t => t.IsClass
                    && !t.IsAbstract
                    && interfaceType.IsAssignableFrom(t)
                    && t.Namespace == ourNamespace)
                .ToList();

            // If we found exactly one, use it
            if (candidateTypes.Count == 1)
                return candidateTypes[0];

            // If we found multiple, prefer the one with the shortest name 
            // (e.g., Element over TableRow for IElement)
            if (candidateTypes.Count > 1)
            {
                // For IElement interface, prefer Element class specifically
                if (interfaceType == typeof(IElement))
                {
                    var elementType = candidateTypes.FirstOrDefault(t => t.Name == "Element");
                    if (elementType != null)
                        return elementType;
                }

                // Otherwise return the first match (they're all valid)
                return candidateTypes[0];
            }

            // No matches found
            return null;
        }
        /// <summary>
        /// Converts a FindsByAttribute into a Selenium By locator.
        /// </summary>
        private By CreateByLocator(FindsByAttribute findsBy)
        {
            return findsBy.How switch
            {
                How.Id => By.Id(findsBy.Using),
                How.Name => By.Name(findsBy.Using),
                How.XPath => By.XPath(findsBy.Using),
                How.CssSelector => By.CssSelector(findsBy.Using),
                How.ClassName => By.ClassName(findsBy.Using),
                How.TagName => By.TagName(findsBy.Using),
                How.LinkText => By.LinkText(findsBy.Using),
                How.PartialLinkText => By.PartialLinkText(findsBy.Using),
                _ => throw new ArgumentException($"Unsupported How value: {findsBy.How}")
            };
        }
    }
}
