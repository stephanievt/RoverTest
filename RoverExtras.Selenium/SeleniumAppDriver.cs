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
        /// Returns null if the attribute is not a FindsByAttribute (allowing other drivers to handle it).
        /// </summary>
        public override IElement CreateElement(Type elementInterfaceType, Attribute locatorAttribute)
        {
            // Only process FindsBy attributes - ignore others (LocateBy, etc.)
            if (locatorAttribute is not FindsByAttribute findsBy)
                return null;

            // Convert FindsBy attribute to Selenium By locator
            By by = CreateByLocator(findsBy);

            // Map interface types to Selenium concrete implementations
            if (elementInterfaceType == typeof(ITextbox))
            {
                return new Textbox(this, by);
            }
            else if (elementInterfaceType == typeof(IButton))
            {
                return new Button(this, by);
            }
            else if (elementInterfaceType == typeof(IElement))
            {
                return new Element(this, by);
            }

            throw new NotSupportedException(
                $"Element type '{elementInterfaceType.Name}' is not supported by Selenium driver");
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
