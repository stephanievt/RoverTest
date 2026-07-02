using Microsoft.Playwright;
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

        

    }
}
