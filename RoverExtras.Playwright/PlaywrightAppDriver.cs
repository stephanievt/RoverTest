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
        /// Returns null if the attribute is not a LocateByAttribute (allowing other drivers to handle it).
        /// </summary>
        public override IElement CreateElement(Type elementInterfaceType, Attribute locatorAttribute)
        {
            // Only process LocateBy attributes - ignore others (FindsBy, etc.)
            if (locatorAttribute is not LocateByAttribute locateBy)
                return null;

            // Map interface types to Playwright concrete implementations
            if (elementInterfaceType == typeof(ITextbox))
            {
                return new Textbox(this, locateBy.How, locateBy.Using);
            }
            else if (elementInterfaceType == typeof(IButton))
            {
                return new Button(this, locateBy.How, locateBy.Using);
            }
            else if (elementInterfaceType == typeof(IElement))
            {
                return new Element(this, locateBy.How, locateBy.Using);
            }

            throw new NotSupportedException(
                $"Element type '{elementInterfaceType.Name}' is not supported by Playwright driver");
        }
        
    }
}
