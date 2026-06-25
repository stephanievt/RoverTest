using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using RoverTest;
using RoverTest.ModelUserInterface;
using SeleniumExtras.PageObjects;
using SeleniumExtras.WaitHelpers;
using System.Reflection;

namespace RoverExtras.Selenium;

public class Element : IElement
{
    public readonly IWebElement WebElement;
    private readonly IWebDriver WebDriver;
    
    public By ByFinder { get; }

    public bool Exists { get; private set; }

    // For the ELEMENTS Collection ONLY
    internal Element(AppDriver appDriver, IWebElement webElement, By by)
    {
        ByFinder = by;
        WebDriver = RoverInternals.GetTypedObject<IWebDriver>(appDriver.Driver);
        WebElement = webElement;
        Exists = true;

    }
    public Element(AppDriver appDriver, By by, Element parent)
    {
        
        IWebElement webParent = parent.WebElement;
        ByFinder = by;
        WebDriver = RoverInternals.GetTypedObject<IWebDriver>(appDriver.Driver);

        WebDriverWait wait = new WebDriverWait(WebDriver, TimeSpan.FromSeconds(30)); // Waits up to 10 seconds
        WebElement = wait.Until(ExpectedConditions.ElementToBeClickable(by));
        try
        {
            WebElement = webParent.FindElement(by);
            Exists = true;
        }
        catch (NoSuchElementException)
        {
            Exists = false;
        }
    }

    public Element(AppDriver appDriver, By by) 
    {
        ByFinder = by;
        WebDriver = RoverInternals.GetTypedObject<IWebDriver>(appDriver.Driver);

        WebDriverWait wait = new WebDriverWait(WebDriver, TimeSpan.FromSeconds(30)); // Waits up to 10 seconds
        WebElement = wait.Until(ExpectedConditions.ElementToBeClickable(by));
        try
        {
            WebElement = WebDriver.FindElement(by);
            Exists = true;
        }
        catch (NoSuchElementException)
        {
            Exists = false;
        }
        
    }

   

    
    public void Click()
    {
        WebElement.Click();
        //TODO: This is not entirely right. But the click on an element causes an action.
        // A table or whatever cannot report itself properly. This may go elsewhere
        // really replaced with the right thing when 
        // remembering selenium timing shit.
        Thread.Sleep(2000);
    }

    public void Highlight()
    {
        string highlightScript = "arguments[0].style.border='2px solid red'; arguments[0].style.background='yellow';";
        IJavaScriptExecutor jsExecutor = (IJavaScriptExecutor)WebDriver;
        jsExecutor.ExecuteScript(highlightScript, WebElement);
    }

    public string Text => WebElement.Text;


    public bool Visible => WebElement.Displayed;

    /// <summary>
    /// Initializes all properties with [FindsBy] attributes on a page object.
    /// This method uses reflection to find the appropriate Element type in the page's namespace.
    /// </summary>
    public static void InitializeElements<TPage>(TPage page, AppDriver driver) where TPage : class
    {
        Type pageType = typeof(TPage);
        PropertyInfo[] properties = pageType.GetProperties(BindingFlags.Public | BindingFlags.Instance);

        // Find Element type in the page's assembly - look for [Namespace].OnscreenElements.Element
        var elementTypeName = $"{pageType.Namespace}.OnescreenElements.Element";
        var elementType = pageType.Assembly.GetTypes()
            .FirstOrDefault(t => t.FullName == elementTypeName && typeof(IElement).IsAssignableFrom(t));

        // If not found, fall back to RoverExtras.Selenium.Element
        if (elementType == null)
        {
            elementType = typeof(Element);
        }

        foreach (var property in properties)
        {
            var attribute = property.GetCustomAttribute<FindsByAttribute>();

            if (attribute != null && typeof(IElement).IsAssignableFrom(property.PropertyType))
            {
                // Convert How enum to By locator
                By byLocator = ConvertToBy(attribute.How, attribute.Using);

                // Create instance using the discovered element type
                var constructor = elementType.GetConstructor(new[] { typeof(AppDriver), typeof(By) });

                if (constructor == null)
                {
                    throw new InvalidOperationException(
                        $"Element type '{elementType.FullName}' must have a constructor with signature (AppDriver, By)");
                }

                IElement element = (IElement)constructor.Invoke(new object[] { driver, byLocator });
                property.SetValue(page, element);
            }
        }
    }

    /// <summary>
    /// Converts SeleniumExtras.PageObjects.How enum to OpenQA.Selenium.By locator
    /// </summary>
    private static By ConvertToBy(How how, string @using)
    {
        return how switch
        {
            How.Id => By.Id(@using),
            How.Name => By.Name(@using),
            How.TagName => By.TagName(@using),
            How.ClassName => By.ClassName(@using),
            How.CssSelector => By.CssSelector(@using),
            How.LinkText => By.LinkText(@using),
            How.PartialLinkText => By.PartialLinkText(@using),
            How.XPath => By.XPath(@using),
            _ => throw new ArgumentException($"Unsupported How type: {how}")
        };
    }
}