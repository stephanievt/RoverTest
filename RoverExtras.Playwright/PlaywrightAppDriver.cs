using Microsoft.Playwright;
using RoverExtras.Playwright.PlaywrightAttributes;
using RoverTest.ModelUserInterface;

namespace RoverExtras.Playwright
{
    public class PlaywrightAppDriver : AppDriver
    {
        private readonly IPage page;
        private readonly IBrowser browser;
        private readonly IPlaywright playwright;

        public PlaywrightAppDriver(string location) : base(location)
        {

            playwright = Microsoft.Playwright.Playwright.CreateAsync().Result;
            browser = playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
            {
                Headless = false
            }).Result;
            page = browser.NewPageAsync().Result;

        }

        // Ok for playwright the "driver" or the thing from which we get
        // page objects is called a "page"
        // I will do this instance and abstract out the playwright bits later,
        // but for now this is just a wrapper around the playwright page
        public override object Driver => page;
        public override async Task NavigateAsync()
        {
            await page.GotoAsync(Location);
        }

        public override void Dispose()
        {
            page.CloseAsync().GetAwaiter().GetResult();
            browser.CloseAsync().GetAwaiter().GetResult();
            browser.DisposeAsync().GetAwaiter().GetResult();
            playwright.Dispose();
        }

        public override byte[] TakeScreenshot()
        {
            return page.ScreenshotAsync(new PageScreenshotOptions
            {
                FullPage = true,
                Type = ScreenshotType.Png
            }).GetAwaiter().GetResult();
        }

        public async Task Load()
        {

            await page.GotoAsync(Location);

        }

        /// <summary>
        /// Creates Playwright-specific element instances based on the [LocateBy] attribute.
        /// Uses reflection to dynamically find concrete implementations in the RoverExtras.Playwright namespace.
        /// Returns null if the attribute is not a LocateByAttribute (allowing other drivers to handle it).
        /// </summary>
        public override IElement CreateElement(Type elementInterfaceType, Attribute locatorAttribute)
        {
            // Only process LocateBy attributes - ignore others (FindsBy, etc.)
            if (locatorAttribute is not LocateByAttribute locateBy)
                return null;

            // Find concrete implementation in this assembly that implements the interface
            var concreteType = FindConcreteElementType(elementInterfaceType);

            if (concreteType == null)
            {
                throw new NotSupportedException(
                    $"Could not find a concrete Playwright element type that implements '{elementInterfaceType.Name}'. " +
                    $"Ensure you have a class in the RoverExtras.Playwright namespace that implements this interface.");
            }

            // Get the constructor: (PlaywrightAppDriver, LocatorType, string)
            var constructor = concreteType.GetConstructor(new[]
            {
                typeof(PlaywrightAppDriver),
                typeof(LocatorType),
                typeof(string)
            });

            if (constructor == null)
            {
                throw new InvalidOperationException(
                    $"Concrete type '{concreteType.Name}' does not have a constructor with signature " +
                    $"({nameof(PlaywrightAppDriver)}, {nameof(LocatorType)}, string)");
            }

            // Invoke the constructor to create the element
            return (IElement)constructor.Invoke(new object[] { this, locateBy.How, locateBy.Using });
        }

        /// <summary>
        /// Finds a concrete class in this assembly that implements the specified interface.
        /// Prefers classes in the RoverExtras.Playwright namespace.
        /// </summary>
        private Type FindConcreteElementType(Type interfaceType)
        {
            var assembly = typeof(PlaywrightAppDriver).Assembly;
            var ourNamespace = typeof(PlaywrightAppDriver).Namespace;

            // Find all types in this assembly that:
            // 1. Are classes (not interfaces or abstracts)
            // 2. Implement the target interface
            // 3. Are in the same namespace as this driver (RoverExtras.Playwright)
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

    }
}
