using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using RoverTest.ModelUserInterface;

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

        
    }
}
